using System;
using System.Collections.Generic;
using StarloadCommons.Network.Login.Client;

namespace StarloadCommons.Network.Packet
{
    public class Packets
    {
        public static Packets Instance = new Packets();
        
        private readonly Dictionary<UInt16, Type> _packets = new Dictionary<UInt16, Type>();
        private readonly Dictionary<Type, UInt16> _packetsReverse = new Dictionary<Type, UInt16>();

        private readonly Dictionary<UInt16, NetworkChannel> _packetsChannel = new Dictionary<ushort, NetworkChannel>();
        
        public Packets()
        {
            if (Instance != null)
                return;
            Instance = this;
            RegisterPacket(typeof(CPacketLoginStart), NetworkChannel.LOGIN);
        }

        private void RegisterPacket(Type packet, NetworkChannel channel)
        {
            if (_packets.Count > UInt16.MaxValue)
                throw new Exception("Max packets reach");
            UInt16 id = (UInt16) _packets.Count;
            _packets.Add(id, packet);
            _packetsReverse.Add(packet, id);
            _packetsChannel.Add(id, channel);
        }

        public UInt16 GetPacketId<T>(IPacket<T> packet)
        {
            _packetsReverse.TryGetValue(packet.GetType(), out UInt16 id);
            return id;
        }

        public NetworkChannel GetChannel(UInt16 packetId)
        {
            _packetsChannel.TryGetValue(packetId, out NetworkChannel channel);
            return channel;
        }

        public Type GetPacket(NetworkChannel channel, UInt16 packetId)
        {
            _packets.TryGetValue(packetId, out Type type);
            return type;
        }
    }
}
