using System.IO;

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
  }
}
