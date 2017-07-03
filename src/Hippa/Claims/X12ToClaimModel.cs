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
    public Stream X12ToClaimModel(Stream input) {
      var service = new ProfessionalClaimToHcfa1500FormTransformation("");

      // send the x12 stream in to obtain a claim object
      var document = service.Transform837ToClaimDocument(input);
      var hcfaclaim = service.TransformClaimToHcfa1500(document.Claims.First());

      var xml = hcfaclaim.Serialize();
      return new MemoryStream(Encoding.UTF8.GetBytes(xml));
    }
  }
}
