namespace OpenBots.Core.Model.ServerModel
{
    /// <summary>
    /// Model for sending data to OpenBots Server
    /// </summary>
    public class SocketPackage
    {
        public string PublicKey { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
    }
}
