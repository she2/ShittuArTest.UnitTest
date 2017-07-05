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
        DbHelper _helper;
        static string _filePath;
        static string _xmlRoot;
        static string _dbFileName;

        public static T Find(Guid? id)
        {
            var savedDoc = XDocument.Load(_filePath);

            var savedXmlData = savedDoc.Element(_xmlRoot).Elements(_dbFileName).FirstOrDefault(item => item.Attribute("id").Value == id.ToString());

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
            //Sets the ID of the data
            Id = _helper.GetId();

            //Convert to JSON string before converting the string to XML data
            var serializedData = JsonConvert.SerializeObject(_data);
            var xmlData = JsonConvert.DeserializeXNode(serializedData, _dbFileName, true);

            //Ensure directory always exists
            if (!Directory.Exists(StringKeys.DATABASE_FILE_PATH))
            {
                Directory.CreateDirectory(StringKeys.DATABASE_FILE_PATH);
            }

            if (File.Exists(_filePath))
            {
                //Load previous file and merge with the new XML data before saving
                var prevDoc = XDocument.Load(_filePath);
                var mergedDoc = _helper.Merge(prevDoc, xmlData, _xmlRoot, _dbFileName);
                mergedDoc.Save(_filePath, SaveOptions.None);
            }
            else
            {
                //Create a new document with a root element and merge with the new XML data before saving
                var newDoc = XDocument.Parse($"<{_xmlRoot}></{_xmlRoot}>", LoadOptions.None);
                var mergedDoc = _helper.Merge(newDoc, xmlData, _xmlRoot, _dbFileName);
                mergedDoc.Save(_filePath, SaveOptions.None);
            }
        }
        public void Delete()
        {
            var savedDoc = XDocument.Load(_filePath);
            savedDoc.Element(_xmlRoot).Elements(_dbFileName).FirstOrDefault(item => item.Attribute("id").Value == Id.ToString()).Remove();
            savedDoc.Save(_filePath, SaveOptions.None);

            Id = null;
            //_data = default(T);
        }

        /// <summary>
        /// Initializes the variables for the <see cref="Operations{T}"/>
        /// </summary>
        protected void Initialize()
        {
            _helper = new DbHelper();
            _dbFileName = _helper.GetDbFileName(_data);
            _filePath = _helper.GetDbFilePath(_data);
            _xmlRoot = _helper.GetDbFileRoot(_data);
        }
    }
}
