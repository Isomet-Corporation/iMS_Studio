using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using ImsHwServer;
using iMS;

namespace iMS_Studio.Model
{
    public class CompensationModel
    {
        private Channel _channel;
        private ims_system thisIMS;

        public CompensationModel(Channel channel, ims_system ims)
        {
            _channel = channel;
            thisIMS = ims;

        }
    }
}
