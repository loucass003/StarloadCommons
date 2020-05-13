using System;
using System.Reflection;
using System.Runtime.Serialization;
using Lidgren.Network;
using StarloadCommons.Network.Packet;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

namespace StarloadCommons.Network
{
    public abstract class NetworkSystem
    {
        private NetPeer _peer;

        public NetworkSystem(NetPeer peer)
        {
            _peer = peer;
        }
        
        public void Update()
        {
            NetIncomingMessage message;

            while ((message = _peer.ReadMessage()) != null)
            {
                ReadMessage(message);
                _peer.Recycle(message);
            }
        }

        private void ReadMessage(NetIncomingMessage message)
        {
            switch (message.MessageType)
            {
                case NetIncomingMessageType.Data:
                    ReadPacket(message);
                    break;
                case NetIncomingMessageType.ErrorMessage:
                    Debug.Log($"Network error message {message.ReadString()}");
                    break;
                case NetIncomingMessageType.StatusChanged:
                    ReadStatus(message);
                    break;
                default:
                    Debug.Log($"Unhandled Network message {message.MessageType.ToString()}");
                    break;
            }
        }

        public void ReadStatus(NetIncomingMessage message)
        {
            NetConnectionStatus status = (NetConnectionStatus) message.ReadByte();
            NetworkChannel channel = (NetworkChannel) message.SequenceChannel;
            if (message.SequenceChannel < 0 || message.SequenceChannel > 32)
                channel = NetworkChannel.LOGIN;
            NetworkConnection connection = GetConnection(this, message);
            
            Debug.Log($"Network status changed {status.ToString()}");
            if (status == NetConnectionStatus.Connected)
                OnConnectionCreate(this, message.SenderConnection);
            if (status == NetConnectionStatus.Disconnected)
                OnDisconnect(this, connection, message);
            connection?.GetNetworkHandler(channel).OnStatusChange(status);
        }
        
        public void ReadPacket(NetIncomingMessage message)
        {
            UInt16 packetId = message.ReadUInt16();
            NetworkChannel channel = (NetworkChannel) message.SequenceChannel;
            Type packet = Packets.Instance.GetPacket(channel, packetId);
            NetworkConnection _connection = GetConnection(this, message);
            
            // ConstructorInfo info = packet.GetConstructor(Type.EmptyTypes);
            object o = FormatterServices.GetUninitializedObject(packet);
            o.GetType().InvokeMember("Read", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, o,
                new object[1] {message});
            o.GetType().InvokeMember("HandlePacket", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public, null, o,
                new object[1] {_connection.GetNetworkHandler(channel)}); //TODO: impossible que ca marche du premier coup LOL
        }

        public abstract NetworkConnection GetConnection(NetworkSystem system, NetIncomingMessage message);

        public abstract void OnConnectionCreate(NetworkSystem system, NetConnection netConnection);

        public abstract void OnDisconnect(NetworkSystem system, NetworkConnection connection, NetIncomingMessage message);
    }
}