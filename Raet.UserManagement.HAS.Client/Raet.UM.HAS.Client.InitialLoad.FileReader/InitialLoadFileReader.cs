using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using ProtoBuf.Meta;
using Raet.UM.HAS.DTOs;

namespace Raet.UM.HAS.Client.InitialLoad.FileReader
{
    public class InitialLoadFileReader : IInitialLoadFileReader
    {
        public InitialLoadFileReader()
        {
            var model = ModelBuilder.GetModel();
            model.CompileInPlace();
        }

        public IList<EffectiveAuthorizationGrantedEvent> ReadInitialLoadFile(string path)
        {
            IList<EffectiveAuthorizationGrantedEvent> events;
            using (var file = File.OpenRead(path))
            {
                events = Serializer.Deserialize<IList<EffectiveAuthorizationGrantedEvent>>(file);
            }

            return events;
        }
    }
}
