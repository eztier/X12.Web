using System;
using System.Configuration;
using System.Linq;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using OopFactory.X12.Parsing;

namespace X12.Web {
  public partial class Hippa : IHippa {
    bool throwException = Convert.ToBoolean(ConfigurationManager.AppSettings["ThrowExceptionOnSyntaxErrors"]);

    private Stream normalParse(Stream input, Encoding encoding) {
      OopFactory.X12.Parsing.X12Parser parser = new OopFactory.X12.Parsing.X12Parser(throwException);
      Stream stream = new MemoryStream();
      StreamWriter writer = new StreamWriter(stream, encoding);

      var interchanges = parser.ParseMultiple(input, encoding);
      writer.Write(encoding.GetBytes("<Interchanges>"));
      for (int i = 0; i < interchanges.Count; i++) {
        var interchange = interchanges[i].Serialize();
        writer.Write(encoding.GetBytes(interchange));
      }
      writer.Write(encoding.GetBytes("</Interchanges>"));
      writer.Flush();

      stream.Position = 0;
      return stream;
    }

    private Stream chunkParse(Stream input, Encoding encoding, int maxBatchSize) {
      OopFactory.X12.Parsing.X12Parser parser = new OopFactory.X12.Parsing.X12Parser(throwException);
      Stream stream = new MemoryStream();
      StreamWriter writer = new StreamWriter(stream, encoding);

      writer.Write(encoding.GetBytes("<Interchanges>"));
      // Break up output files by batch size
      X12StreamReader reader = new X12StreamReader(stream, encoding);
      X12FlatTransaction currentTransactions = reader.ReadNextTransaction();
      X12FlatTransaction nextTransaction = reader.ReadNextTransaction();
      int i = 1;
      while (!string.IsNullOrEmpty(nextTransaction.Transactions.First())) {
        if (currentTransactions.GetSize() + nextTransaction.GetSize() < maxBatchSize
            && currentTransactions.IsaSegment == nextTransaction.IsaSegment
            && currentTransactions.GsSegment == nextTransaction.GsSegment) {
          currentTransactions.Transactions.AddRange(nextTransaction.Transactions);
        } else {
          var interchange = parser.ParseMultiple(currentTransactions.ToString()).First().Serialize();
          writer.Write(encoding.GetBytes(interchange));
          currentTransactions = nextTransaction;
        }
        nextTransaction = reader.ReadNextTransaction();
      }

      var finalInterchange = parser.ParseMultiple(currentTransactions.ToString()).First().Serialize();
      writer.Write(encoding.GetBytes(finalInterchange));

      writer.Write(encoding.GetBytes("</Interchanges>"));
      writer.Flush();

      stream.Position = 0;
      return stream;
    }

    public Stream X12Parser(Stream postData) {
      Stream stream = null;
      MemoryStream peekableStream = null;
      int maxBatchSize = 10 * 1012 * 1012; // 10 Mbytes
      if (ConfigurationManager.AppSettings["MaxBatchSize"] != null)
        maxBatchSize = Convert.ToInt32(ConfigurationManager.AppSettings["MaxBatchSize"]);

      byte[] header = new byte[6];

      peekableStream = Util.CopyStreamToMemory(postData);
      peekableStream.Read(header, 0, 6);
      peekableStream.Position = 0;
      Encoding encoding = (header[1] == 0 && header[3] == 0 && header[5] == 0) ? Encoding.Unicode : Encoding.UTF8;

      if (peekableStream.Length <= maxBatchSize)
        stream = normalParse(peekableStream, encoding);
      else
        stream = chunkParse(peekableStream, encoding, maxBatchSize);

      WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
      OperationContext clientContext = OperationContext.Current;

      clientContext.OperationCompleted += new EventHandler((o, s) => {
        if (stream != null)
          stream.Dispose();
      });

      return stream;
    }
  }
}
