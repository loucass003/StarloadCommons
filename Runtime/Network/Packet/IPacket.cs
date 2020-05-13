using Lidgren.Network;

namespace StarloadCommons.Network.Packet
{
    public interface IPacket<T>
    {
        void Read(NetIncomingMessage message);
        void Write(NetOutgoingMessage message);

        int Size();
        
        void HandlePacket(T handler);
    }
}
