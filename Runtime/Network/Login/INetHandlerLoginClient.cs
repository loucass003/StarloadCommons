using StarloadCommons.Network.Login.Server;

namespace StarloadCommons.Network.Login
{
    public interface INetHandlerLoginClient : INetworkHandler
    {
        void HandleLoginSuccess(SPacketLoginSuccess packetIn);
    }
}