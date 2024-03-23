namespace Dariosoft.Framework.Helpers
{
    public class MiscHelper
    {
        private MiscHelper() { }

        private readonly static Lazy<MiscHelper> _lazy = new Lazy<MiscHelper>(() => new MiscHelper());

        public static MiscHelper Instance => _lazy.Value;

        public System.Net.IPAddress GetLocalIP()
        {
            var addresses = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName());
            return addresses.FirstOrDefault(e => e.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) ??
                addresses.FirstOrDefault(e => e.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6) ??
                addresses.FirstOrDefault() ??
                System.Net.IPAddress.Parse("127.0.0.1");
        }
    }
}
