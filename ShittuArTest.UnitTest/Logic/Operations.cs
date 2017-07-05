using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using ShittuArTest.UnitTest.Util;

namespace ShittuArTest.UnitTest.Logic
{

    public class Operations<T>
    {
        [JsonProperty("@id")]
        public Guid? Id { get; private set; }
        protected static T _data { get; set; }

        public static T Find(Guid? id)
        {
            var helper = new DbHelper();
            var filePath = helper.GetDbFilePath(_data);
            var savedDoc = XDocument.Load(filePath);

            var xmlRoot = helper.GetDbFileRoot(_data);
            var dbFileName = helper.GetDbFileName(_data);

            var savedXmlData = savedDoc.Element(xmlRoot).Elements(dbFileName).FirstOrDefault(item => item.Attribute("id").Value == id.ToString());

            //element does not exist, we can just return a default value for Type
            if (savedXmlData == null)
            {
                return default(T);
            }
            XmlDocument xmlToSerialize = new XmlDocument();
            xmlToSerialize.LoadXml(Convert.ToString(savedXmlData));
            string serializedData = JsonConvert.SerializeXmlNode(xmlToSerialize, Newtonsoft.Json.Formatting.Indented, true);

            return JsonConvert.DeserializeObject<T>(serializedData);
        }
        public void Save()
        {
            var helper = new DbHelper();
            var dbFileName = helper.GetDbFileName(_data);
            var filePath = helper.GetDbFilePath(_data);
            var xmlRoot = helper.GetDbFileRoot(_data);

            //Sets the ID of the data
            Id = helper.GetId();

            //Convert to JSON string before converting the string to XML data
            var serializedData = JsonConvert.SerializeObject(_data);
            var xmlData = JsonConvert.DeserializeXNode(serializedData, dbFileName, true);

            //Ensure directory always exists
            if (!Directory.Exists(StringKeys.DATABASE_FILE_PATH))
            {
                Directory.CreateDirectory(StringKeys.DATABASE_FILE_PATH);
            }

            if (File.Exists(filePath))
            {
                //Load previous file and merge with the new XML data before saving
                var prevDoc = XDocument.Load(filePath);
                var mergedDoc = helper.Merge(prevDoc, xmlData, xmlRoot, dbFileName);
                mergedDoc.Save(filePath, SaveOptions.None);
            }
            else
            {
                //Create a new document with a root element and merge with the new XML data before saving
                var newDoc = XDocument.Parse($"<{xmlRoot}></{xmlRoot}>", LoadOptions.None);
                var mergedDoc = helper.Merge(newDoc, xmlData, xmlRoot, dbFileName);
                mergedDoc.Save(filePath, SaveOptions.None);
            }
        }
        public void Delete()
        {
            var helper = new DbHelper();
            var filePath = helper.GetDbFilePath(_data);
            var xmlRoot = helper.GetDbFileRoot(_data);
            var dbFileName = helper.GetDbFileName(_data);

            var savedDoc = XDocument.Load(filePath);
            savedDoc.Element(xmlRoot).Elements(dbFileName).FirstOrDefault(item => item.Attribute("id").Value == Id.ToString()).Remove();
            savedDoc.Save(filePath, SaveOptions.None);

            Id = null;
        }


    }
}
