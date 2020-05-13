using Lidgren.Network;
using StarloadCommons.Network.Packet;

namespace StarloadCommons.Network.Login.Server
{
    public class SPacketLoginSuccess : IPacket<INetHandlerLoginClient>
    {
        public void Read(NetIncomingMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void Write(NetOutgoingMessage message)
        {
            throw new System.NotImplementedException();
        }

        public int Size()
        {
            return 0;
        }

        public void HandlePacket(INetHandlerLoginClient handler)
        {
            handler.HandleLoginSuccess(this);
        }
    }
}