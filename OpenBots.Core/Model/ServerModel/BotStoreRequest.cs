using System;
using OpenBots.Core.Enums;

namespace OpenBots.Core.Model.ServerModel
{
    public class BotStoreRequest
    {
        public Guid WorkerID { get; set; }
        public string BotStoreName { get; set; }
        public BotStoreRequestType RequestType { get; set; }
    }
}
