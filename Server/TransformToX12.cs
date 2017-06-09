using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace X12.Web {
  public partial class Hippa : IHippa {
    public Stream TransformToX12(Stream postData) {
      string resp = string.Empty;
      var parser = new OopFactory.X12.Parsing.X12Parser();

      try {
        var xmltext = new StreamReader(postData, Encoding.UTF8).ReadToEnd();
        resp = parser.TransformToX12(xmltext);
      } catch(Exception e) {
        WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
        resp = e.Message;
      }

      WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";
      Stream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(resp));
      OperationContext clientContext = OperationContext.Current;

      clientContext.OperationCompleted += new EventHandler((o, s) => {
        if (stream != null)
          stream.Dispose();
      });

      return stream;
    }
  }
}
