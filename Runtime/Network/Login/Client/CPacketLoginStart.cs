using Lidgren.Network;
using StarloadCommons.Network.Packet;

namespace StarloadCommons.Network.Login.Client
{
    public class CPacketLoginStart : IPacket<INetHandlerLoginServer>
    {
        public UserProfile Profile;

        public CPacketLoginStart(UserProfile profile)
        {
            Profile = profile;
        }

        public void Read(NetIncomingMessage message)
        {
            Profile = new UserProfile();
            Profile.Read(message);
        }

        public void Write(NetOutgoingMessage message)
        {
            Profile.Write(message);
        }

        public int Size()
        {
            return Profile.Size();
        }

        public void HandlePacket(INetHandlerLoginServer handler)
        {
            handler.ProcessLoginStart(this);
        }
    }
}
