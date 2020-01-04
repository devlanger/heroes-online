using ServerUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesServer.SendPackets
{
    public class SpawnCharacterPacket : PacketBase
    {
        public SpawnCharacterPacket(bool local, CharacterInfo character) : base()
        {
            writer.Write((byte)2);
            writer.Write(local);

            writer.Write(character.Character.Id);
            writer.Write(character.Character.BaseId);

            writer.Write(character.Character.Name);
            writer.Write((byte)character.Character.GetStatValue(StatType.LEVEL));
            writer.Write(character.Character.GetStatValue(StatType.HEALTH));
            writer.Write(character.Character.GetStatValue(StatType.MAX_HEALTH));

            writer.Write(character.Character.Position.X);
            writer.Write(character.Character.Position.Y);
            writer.Write(character.Character.Position.Z);

            if (local)
            {
                writer.Write((ushort)character.Character.GetStatValue(StatType.SKILL_POINTS));
            }
        }

        public override void Read(Client client)
        {
        }

        public override void Write()
        {

        }
    }
}
