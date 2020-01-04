using ServerUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesServer.SendPackets
{
    public class LoginOkPacket : PacketBase
    {
        public LoginOkPacket() : base()
        {
            writer.Write((byte)1);
        }

        public override void Read(Client client)
        {
        }

        public override void Write()
        {
        }
    }
}
