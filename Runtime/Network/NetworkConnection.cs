using System;
using System.Collections.Generic;
using Lidgren.Network;
using StarloadCommons.Network.Packet;
using UnityEditor;

namespace StarloadCommons.Network
{
    public class NetworkConnection
    {
        private NetConnection _connection;
        private Dictionary<NetworkChannel, INetworkHandler> _networkHandlers = new Dictionary<NetworkChannel, INetworkHandler>();
        
        public NetworkConnection(NetConnection connection)
        {
            _connection = connection;
        }

        public void SetConnection(NetConnection connection)
        {
            _connection = connection;
        }
        
        public void SendPacket<T>(IPacket<T> packet, NetDeliveryMethod method)
        {
            UInt16 packetId = Packets.Instance.GetPacketId(packet);
            
            NetOutgoingMessage message = _connection.Peer.CreateMessage(packet.Size());
            message.Write(packetId);
            packet.Write(message);

            NetworkChannel channel = Packets.Instance.GetChannel(packetId);
            _connection.SendMessage(message, method, (int)channel);
        }

        public void SendToAll<T>(IPacket<T> packet, NetDeliveryMethod method)
        {
            if (!_connection.Peer.GetType().IsInstanceOfType(typeof(NetServer)))
                throw new Exception("The Peer is not an instance of NetServer. Are you in client-side !?");
            UInt16 packetId = Packets.Instance.GetPacketId(packet);
            NetOutgoingMessage message = _connection.Peer.CreateMessage(packet.Size());
            message.Write(packetId);
            packet.Write(message);

            NetworkChannel channel = Packets.Instance.GetChannel(packetId);
            ((NetServer)_connection.Peer).SendToAll(message, method, (int)channel);
        }

        public void AddNetworkChannel(NetworkChannel channel, INetworkHandler handler)
        {
            _networkHandlers.Add(channel, handler);
        }

        public INetworkHandler GetNetworkHandler(NetworkChannel channel)
        {
            _networkHandlers.TryGetValue(channel, out INetworkHandler handler);
            return handler;
        }
    }
}