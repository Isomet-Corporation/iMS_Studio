using System;
using System.IO;
using System.Xml;

namespace iMS_Studio
{
    public sealed class Settings
    {
        private readonly string _filename;
        private readonly XmlDocument _doc = new XmlDocument();

        private const string emptyFile =
            @"<?xml version=""1.0"" encoding=""utf-8"" ?>
          <configuration>
              <appSettings>
                  <add key=""LastActiveProject"" value=""None"" />
              </appSettings>
          </configuration>";

        public Settings(string path, string filename)
        {
            // strip any trailing backslashes...
            while (path.Length > 0 && path.EndsWith("\\"))
            {
                path = path.Remove(path.Length - 1, 1);
            }

            _filename = Path.Combine(path, filename);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!File.Exists(_filename))
            {
                // Create it...
                _doc.LoadXml(emptyFile);
                _doc.Save(_filename);
            }
            else
            {
                _doc.Load(_filename);
            }

        }

        /// <summary>
        /// Retrieve a value by name.
        /// Returns the supplied DefaultValue if not found.
        /// </summary>
        public string Get(string key, string defaultValue)
        {
            XmlNode node = _doc.SelectSingleNode("configuration/appSettings/add[@key='" + key + "']");

            if (node == null)
            {
                return defaultValue;
            }
            return node.Attributes["value"].Value ?? defaultValue;
        }

        /// <summary>
        /// Write a config value by key
        /// </summary>
        public void Set(string key, string value)
        {
            XmlNode node = _doc.SelectSingleNode("configuration/appSettings/add[@key='" + key + "']");

            if (node != null)
            {
                node.Attributes["value"].Value = value;

                _doc.Save(_filename);
            }
            else {
                string strNamespace = _doc.DocumentElement.NamespaceURI;
                XmlNamespaceManager nsMgr = new XmlNamespaceManager(_doc.NameTable);
                nsMgr.AddNamespace("ns", strNamespace);
                XmlNode root = _doc.SelectSingleNode("/ns:configuration/ns:appSettings", nsMgr);
                if (root != null)
                {
                    XmlNode new_node = _doc.CreateElement("add", strNamespace);
                    XmlAttribute xKey = _doc.CreateAttribute("key");
                    xKey.Value = key;
                    XmlAttribute xValue = _doc.CreateAttribute("value");
                    xValue.Value = value;
                    new_node.Attributes.Append(xKey);
                    new_node.Attributes.Append(xValue);
                    root.AppendChild(new_node);
                    _doc.Save(_filename);
                }
            }
        }

    }
}
