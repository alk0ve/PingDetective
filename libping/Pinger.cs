using System.Net.NetworkInformation;
using System.Text;

namespace libping
{
    public class Pinger
    {
        public static bool isAccessible(string hostnameOrIP)
        {
            bool accessible = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();

                PingOptions options = new PingOptions();
                int timeoutMS = 500; // TODO make configurable
                options.DontFragment = true;
                options.Ttl = 64; // TODO make configurable

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