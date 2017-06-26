using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace X12.Web {

  [ServiceContract(Namespace = "x12.web.wcf.rest.website")]
  public interface IHippa {
    [OperationContract]
    [WebInvoke(Method = "POST",
      BodyStyle = WebMessageBodyStyle.Bare,
      RequestFormat = WebMessageFormat.Xml,
      ResponseFormat = WebMessageFormat.Xml,
      UriTemplate = "transform/xml/x12")]
    Stream TransformToX12(Stream postData);

    [OperationContract]
    [WebInvoke(Method = "POST",
      BodyStyle = WebMessageBodyStyle.Bare,
      RequestFormat = WebMessageFormat.Xml,
      ResponseFormat = WebMessageFormat.Xml,
      UriTemplate = "transform/x12/xml")]
    Stream X12Parser(Stream postData);

    [OperationContract]
    [WebInvoke(Method = "POST",
      BodyStyle = WebMessageBodyStyle.Bare,
      RequestFormat = WebMessageFormat.Xml,
      ResponseFormat = WebMessageFormat.Xml,
      UriTemplate = "claims/transform/837/xml")]
    Stream Transform837ToClaimDocument(Stream postData);
  }
}
