
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum enPacketType { }

public class NetworkHandler
{
    NetworkManager m_NetworkMgr = new NetworkManager();
    Dictionary<UInt16, Action<byte[]>> m_PacketHandlers = new Dictionary<UInt16, Action<byte[]>>();

    // key: packet/message type
    // value: packet/message recv handler
    public void RegistPacketHandler(UInt16 packetType, Action<byte[]> handler)
    {
        if (m_PacketHandlers.ContainsKey(packetType))
        {
            m_PacketHandlers.Remove(packetType);
        }
        m_PacketHandlers.Add(packetType, handler);
    }

    public bool Connect(string ip, UInt16 port)
    {
        return m_NetworkMgr.Connect(ip, port);
    }

    public void Send<T>(T message, bool encoding)
    {
        m_NetworkMgr.SendPacket(message, encoding);
    }

    public T BytesToMessage<T>(byte[] bytes)
    {
        return m_NetworkMgr.BytesToMessage<T>(bytes);
    }

    public void Clear()
    {
        m_NetworkMgr.Disconnect();
    }

    public void Update()
    {
        while (m_NetworkMgr.ReceivedDataSize() >= Marshal.SizeOf<JNET_PROTOCOL.MSG_HDR>())
        {
            byte[] payload;
            if (m_NetworkMgr.ReceivePacketBytes(out payload, true))
            {
                UInt16 packetType = BitConverter.ToUInt16(payload, 0);
                if (m_PacketHandlers.ContainsKey(packetType))
                {
                    m_PacketHandlers[packetType].Invoke(new ArraySegment<byte>(payload, sizeof(UInt16), payload.Length - sizeof(UInt16)).ToArray());
                }
            }
            else
            {
                break;
            }
        }
    }
}