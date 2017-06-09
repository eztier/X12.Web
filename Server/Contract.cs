using System.Runtime.Serialization;
using System.Collections.Generic;
using System.ComponentModel;

namespace X12.Web {
  /*!
   * Contracts
   */
  [DataContract(Namespace = "")]
  public class DataManagementRequest {
    [DataMember]
    public string requestXmlString {
      get;
      set;
    }
  }

  [DataContract(Namespace = "")]
  public class DataManagementResponse {
    [DataMember]
    public string response {
      get;
      set;
    }
  }
}
