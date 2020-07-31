using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.DumblyConcealer.Tasks.UploadData.Entities;
using System.IO;
using System.Xml;

namespace CMCS.DumblyConcealer.Utilities
{
    public class UploadXmlHelper
    {
        /// <summary>
        /// 从XML加载配置对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public List<TableOrView> LoadConfig()
        {
            string xmlPath = "UploadData.AppConfig.xml";
            List<TableOrView> result = new List<TableOrView>();
            if (File.Exists(xmlPath))
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(File.ReadAllText(xmlPath));

                XmlNodeList list = xdoc.GetElementsByTagName("TableOrView");

                var tables = (new TableOrView()).GetType().GetProperties();
                var props = (new PropertySet()).GetType().GetProperties();

                foreach (XmlNode item in list)
                {
                    TableOrView entity = new TableOrView()
                    {
                        PropertySetDetails = new List<PropertySet>()
                    };

                    foreach (XmlAttribute attr in item.Attributes)
                    {
                        if (tables.Count(a => a.Name.ToLower() == attr.Name.ToLower()) <= 0) continue;
                        var pi = tables.First(a => a.Name.ToLower() == attr.Name.ToLower());
                        pi.SetValue(entity, attr.Value, null);
                    }

                    foreach (XmlNode node in item.ChildNodes)
                    {
                        PropertySet detail = new PropertySet();

                        foreach (XmlAttribute attr in node.Attributes)
                        {
                            if (props.Count(a => a.Name.ToLower() == attr.Name.ToLower()) <= 0) continue;
                            var pi = props.First(a => a.Name.ToLower() == attr.Name.ToLower());
                            pi.SetValue(detail, attr.Value, null);
                        }
                        entity.PropertySetDetails.Add(detail);
                    }
                    result.Add(entity);
                }
            }
            return result;
        }
    }
}
