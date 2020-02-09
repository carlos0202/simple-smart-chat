namespace Simple.Smart.Chat.App.Helpers
{
    public class BotSettings
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public int RequestedConnectionTimeout { get; set; }
        public string InboundQueue { get; set; }
        public string OutboundQueue { get; set; }
        public string BotName { get; set; }
    }
}
