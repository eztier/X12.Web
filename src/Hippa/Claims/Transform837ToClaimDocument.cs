using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OopFactory.X12.Hipaa.Common;
using OopFactory.X12.Hipaa.Claims;
using OopFactory.X12.Hipaa.Claims.Services;

namespace X12.Web {
  public partial class Hippa : IHippa {

    public Stream Transform837ToClaimDocument(Stream input) {
      var service = new ClaimTransformationService();
      var document = service.Transform837ToClaimDocument(input);      
      string xml = document.Serialize();

      return new MemoryStream(Encoding.UTF8.GetBytes(xml));
    }

  }
}
