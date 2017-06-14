using System;
using System.IO;
using System.Xml;

namespace X12.Web {
  static internal class Util {
    static internal MemoryStream CopyStreamToMemory(Stream inputStream) {
      MemoryStream ret = new MemoryStream();
      const int BUFFER_SIZE = 1024;
      byte[] buf = new byte[BUFFER_SIZE];

      int bytesread = 0;
      while ((bytesread = inputStream.Read(buf, 0, BUFFER_SIZE)) > 0)
        ret.Write(buf, 0, bytesread);

      ret.Position = 0;
      return ret;
    }

    public static string AsString(this XmlDocument xmlDoc) {
      using (StringWriter sw = new StringWriter()) {
        using (XmlTextWriter tx = new XmlTextWriter(sw)) {
          xmlDoc.WriteTo(tx);
          string strXmlText = sw.ToString();
          return strXmlText;
        }
      }
    }

    public static string RemoveXmlDeclaration(this string xmlString) {
      var xml = new XmlDocument();

      try {
        xml.LoadXml(xmlString);

        if (xml.FirstChild.NodeType == XmlNodeType.XmlDeclaration) {
          xml.RemoveChild(xml.FirstChild);
        }

        return xml.AsString();
      } catch (Exception e) {
        // no op
      }
      return "";
    }
  }
}
