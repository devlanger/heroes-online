using ServerUtilities;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace HeroesServer.ReceivedPackets
{
    public class DashPacket : PacketBase
    {
        public DashPacket() : base()
        {
        }

        public override void Read(Client client)
        {
            ((GameClient)client).CharacterInfo.Character.ChangeStatValue(StatType.STAMINA, -20);
        }

        public override void Write()
        {
        }
    }
}
