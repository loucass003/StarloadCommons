using Lidgren.Network;

namespace StarloadCommons.Network
{
    public class UserProfile : ISerializableData<NetBuffer>
    {
        public string name;
        
        public void Read(NetBuffer buf)
        {
            name = buf.ReadString();
        }

        public void Write(NetBuffer buf)
        {
            buf.Write(name);
        }

        public int Size()
        {
            return name.Length;
        }
    }
}