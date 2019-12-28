using ServerUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesServer
{
    public class StatSyncPacket : PacketBase
    {
        public StatSyncPacket(int id, StatType stat, int value) : base()
        {
            writer.Write((byte)5);
            writer.Write(id);
            writer.Write((byte)stat);
            writer.Write(value);
        }

        public override void Read(Client client)
        {
        }

        public override void Write()
        {
        }
    }
}
