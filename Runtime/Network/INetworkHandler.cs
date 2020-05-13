using Lidgren.Network;

namespace StarloadCommons.Network
{
    public interface INetworkHandler
    {
        void OnStatusChange(NetConnectionStatus status);
    }
}