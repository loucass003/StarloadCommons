using StarloadCommons.Network.Login.Client;

namespace StarloadCommons.Network.Login
{
    public interface INetHandlerLoginServer : INetworkHandler
    {
        void ProcessLoginStart(CPacketLoginStart packetIn);
    }
}