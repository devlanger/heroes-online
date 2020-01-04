using HeroesServer.SendPackets;
using ServerUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Text;

namespace LoginServer.ReceivedPackets
{
    public class EnterGamePacket : PacketBase
    {
        public EnterGamePacket() : base()
        {
        }

        public override void Read(Client client)
        {
            Dictionary<EquipmentSlot, ushort> eq = new Dictionary<EquipmentSlot, ushort>();

            foreach (var item in Enum.GetValues(typeof(EquipmentSlot)))
            {
                EquipmentSlot slot = (EquipmentSlot)item;

                eq.Add(slot, reader.ReadUInt16());
            }

            foreach (var item in eq)
            {
                Console.WriteLine(item.Key + " = " + item.Value);
            }

            CharacterSession session = client.Session;
            client.SendData(new JoinGamePacket(session.id, session.hash));
        }

        public override void Write()
        {
        }

        public string Base64Encode()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            return GuidString;
        }
    }
}
