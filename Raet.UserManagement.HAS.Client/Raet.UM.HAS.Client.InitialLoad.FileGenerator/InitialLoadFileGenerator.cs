using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using ProtoBuf.Meta;
using Raet.UM.HAS.DTOs;
using Raet.UM.HAS.Client.InitialLoad;

namespace Raet.UM.HAS.Client.InitialLoad.FileGenerator
{
    public class InitialLoadFileGenerator : IInitialLoadFileGenerator
    {

        public InitialLoadFileGenerator()
        {
            var model = ModelBuilder.GetModel();

            model.CompileInPlace();
        }

        public void GenerateInitialLoadFile(IList<Authorization> authorizations, string outputPath, string tenantId)
        {
            var events = authorizations.Select(x => new EffectiveAuthorizationGrantedEvent()
            {
                FromDateTime = x.From,
                EffectiveAuthorization = x.EffectiveAuthorization
            });
            
            var filePath = string.Format("{0}\\InitialLoad_{1}.bin", outputPath, tenantId);

            using (var file = File.Create(filePath))
            {
                Serializer.Serialize(file, events);
            }
        }
    }
}
