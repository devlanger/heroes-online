using ENet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ServerUtilities
{
    public class Client
    {
        public CharacterSession Session { get; set; }
        public Peer Peer { get; set; }

        public void SendData(PacketBase packet)
        {
            packet.Write();

            byte[] data = packet.GetWriteData();
            Packet p = new Packet();
            p.Create(data);

            Peer.Send(0, ref p);
        }
    }

    public class CharacterSession
    {
        public int id;
        public string hash;
    }
}
