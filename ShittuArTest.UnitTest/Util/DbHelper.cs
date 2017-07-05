using System;
using System.Xml.Linq;

namespace ShittuArTest.UnitTest.Util
{
    class DbHelper
    {
        /// <summary>
        /// Get a new ID
        /// </summary>
        /// <returns>A new Guid</returns>
        public Guid GetId() => Guid.NewGuid();

        /// <summary>
        /// Merges two <see cref="XDocument"/>s
        /// </summary>
        /// <param name="main">The main <see cref="XDocument"/> that will be returned</param>
        /// <param name="toAdd">The <see cref="XDocument"/> to be added to the main <see cref="XDocument"/></param>
        /// <param name="rootElement"> The string name of the <see cref="XDocument.Root"/> element </param>
        /// <param name="typeName">The <see cref="XElement"/> of the type to retrieve</param>
        /// <returns>A merged <see cref="XDocument"/></returns>
        public XDocument Merge(XDocument main, XDocument toAdd, string rootElement, string typeName)
        {
            var root = main.Element(rootElement);
            foreach (var sChild in toAdd.Elements(typeName))
            {
                root.Add(sChild);
            }
            return main;
        }

        /// <summary>
        /// Gets the path where the database file is stored
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the data the Db contains</typeparam>
        /// <param name="data">The data to save</param>
        /// <returns>The path to the DB file</returns>
        public string GetDbFilePath<T>(T data) => $"{StringKeys.DatabaseFilePath}{GetDbFileName(data)}.xml";

        /// <summary>
        /// Gets the file name of the database file
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the data the Db contains</typeparam>
        /// <param name="data">The data to save</param>
        /// <returns>The file name to the DB file</returns>
        public string GetDbFileName<T>(T data) => data.GetType().Name;

        /// <summary>
        /// Gets the root element of the database file stored
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the data the Db contains</typeparam>
        /// <param name="data">The data to save</param>
        /// <returns>The root element of the DB file</returns>
        public string GetDbFileRoot<T>(T data) => $"{GetDbFileName(data)}Db";
    }
}
