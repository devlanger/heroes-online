using ServerUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesServer.SendPackets
{
    public class JoinGamePacket : PacketBase
    {
        public JoinGamePacket(int id, string sessionHash) : base()
        {
            writer.Write((byte)1);

            writer.Write(id);
            writer.Write(sessionHash);
        }

        public override void Read(Client client)
        {
        }

        public override void Write()
        {
        }
    }
}
