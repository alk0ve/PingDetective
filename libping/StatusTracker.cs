using System.Collections.Generic;

namespace libping
{
    /**
     * Uses ICMP to keep track of states of remote hosts.
     * For each host, exposes the number of pings that reported the
     * same status (i.e. if last 4 pings were successful the return
     * value will be (UP, 4)).
     * 
     * Hostname can always also be a textual IP address.
     * 
     * NOT thread-safe.
     * 
     * Possible performance improvements (if/when deemed necessary):
     *  - don't create a Ping object every time, reuse it
     *  - use async ping API instead of sync (gets its own thread, but each async call still needs its own Ping object)
     *  - run several pings in parallel using async ping API
     */
    public class StatusTracker
    {
        public enum Status { UP, DOWN, UNKNOWN};
        public class HostState
        {
            public string hostname;
            public Status status;
            public uint statusCount;

        }

        public List<HostState> m_hostStates { get; set; }
        private uint m_maxStatusCount = 0;

        public StatusTracker(in List<string> hostnames, uint maxStatusCount = 1000)
        {
            m_maxStatusCount = maxStatusCount;

            foreach (var host in hostnames)
            {
                addHostname(host);
            }
        }

        public void addHostname(in string hostname) 
        {
            m_hostStates.Add(new HostState() { hostname = hostname, status = Status.UNKNOWN, statusCount = 0 });
        }

        public void removeHostname(string hostname)
        {
            m_hostStates.RemoveAll(x => (x.hostname == hostname));
        }

        public void ping()
        {
            foreach (var host in m_hostStates)
            {
                Status newStatus = Status.UNKNOWN;

                if (SimplePing.isAccessible(host.hostname))
                {
                    newStatus = Status.UP;
                }
                else
                {
                    newStatus = Status.DOWN;
                }

                if (newStatus == host.status)
                {
                    host.statusCount++;
                    if (host.statusCount >= m_maxStatusCount)
                    {
                        host.statusCount = m_maxStatusCount;
                    }
                }
                else
                {
                    host.status = newStatus;
                    host.statusCount = 1;
                }
            }
        }
    }
}
