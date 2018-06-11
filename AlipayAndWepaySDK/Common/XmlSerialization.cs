using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MvcSolution
{
    public class XmlSerialization
    {
       
        public static T Xml_Deserialization<T>(string xml)
        {         
           XmlSerializer oXml = new XmlSerializer(typeof(T));
           using (StringReader oStringReader = new StringReader(xml))
           {
              return (T)oXml.Deserialize(oStringReader);
           }
        }                                                     
                   
        public static T Xml_Deserialization<T>(Stream xml)
        {                                                                     
            XmlSerializer oXml = new XmlSerializer(typeof(T));
            T obj = (T)oXml.Deserialize(xml);            
            return obj;
        }
      
        public static T Xml_Deserialization<T>(XmlReader xml)
        {
            XmlSerializer oXml = new XmlSerializer(typeof(T));
            T obj = (T)oXml.Deserialize(xml,"utf-8");
            return obj;
        }                                


        public static StringWriter Xml_Serialization(object obj) 
        {
            StringWriter oString = new StringWriter();
            XmlSerializer oXml = new XmlSerializer(obj.GetType());
            oXml.Serialize(oString,obj);
            return oString;
        }

                                      
        public static string XmlStr_Serialization(object obj,string encodeStyle)
        {
            MemoryStream oMemroyStream = new MemoryStream();
           
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlWriterSettings settings = new XmlWriterSettings ();
            settings.Encoding = Encoding.GetEncoding(encodeStyle);
            settings.Indent = true;
            settings.IndentChars = ("\t");
            settings.OmitXmlDeclaration = false;

            XmlWriter xmlWriter = XmlWriter.Create(oMemroyStream, settings);
            XmlSerializer oXml = new XmlSerializer(obj.GetType());
            oXml.Serialize(xmlWriter, obj, ns);
            xmlWriter.Flush();
            xmlWriter.Close();

            oMemroyStream.Position = 0;
            StreamReader oStreamR = new StreamReader(oMemroyStream);
            String xmlStr= oStreamR.ReadToEnd();
            oMemroyStream.Close();
            oMemroyStream.Dispose();
            oStreamR.Close();
            oStreamR.Dispose();

            return xmlStr;
        }
              
    }
}
