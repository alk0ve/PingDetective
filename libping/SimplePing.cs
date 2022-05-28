using System.Net.NetworkInformation;
using System.Text;

namespace libping
{
    public class SimplePing
    {
        public static bool isAccessible(string hostnameOrIP, int timeoutMS = 500, int ttl = 64)
        {
            bool accessible = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();

                PingOptions options = new PingOptions();
                options.DontFragment = true;
                options.Ttl = ttl;

                string data = "putonmybluesuedeshoesandiboardedtheplane";
                var buffer = Encoding.ASCII.GetBytes(data);

                var reply = pinger.Send(hostnameOrIP, timeoutMS, buffer, options);
                accessible = (reply.Status == IPStatus.Success);

            }
            catch (PingException)
            {
                // ignore exception
                accessible = false;
            }
            finally
            {
                pinger.Dispose();
            }

            return accessible;
        }

    }
}