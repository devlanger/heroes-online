using HeroesServer;
using ServerUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HeroesServer.ReceivedPackets
{
    public class JoinGamePacket : PacketBase
    {
        public override void Read(Client client)
        {
            int id = reader.ReadInt32();
            string hash = reader.ReadString();
            Console.WriteLine("Join game: " + id + "/" + hash);

            DataTable table = DatabaseManager.ReturnQuery(string.Format("SELECT * FROM characters WHERE id='{0}' AND session_hash='{1}'", id, hash));
            if (table.Rows.Count > 0)
            {
                int dbId = (int)table.Rows[0]["id"];
                string name = (string)table.Rows[0]["nickname"];
                double x = (double)table.Rows[0]["pos_x"];
                double y = (double)table.Rows[0]["pos_y"];
                double z = (double)table.Rows[0]["pos_z"];
                byte lvl = (byte)table.Rows[0]["level"];
                int exp = (int)table.Rows[0]["experience"];

                Console.WriteLine(name);
                CharacterInfo info = ((GameClient)client).CharacterInfo;
                info.Character.DatabaseId = dbId;
                info.Character.Name = name;
                info.Character.Position = new System.Numerics.Vector3((float)x, (float)y, (float)z);
                info.Character.SetStat(StatType.LEVEL, lvl);
                info.Character.SetStat(StatType.EXPERIENCE, exp);

                client.SendData(new HeroesServer.SendPackets.JoinGamePacket());
                client.SendData(new HeroesServer.SendPackets.SpawnCharacterPacket(true, info));

                foreach (var character in CharactersManager.Instance.zonesBucket[0].Values)
                {
                    if (character.Character.Id == info.Character.Id)
                    {
                        continue;
                    }

                    client.SendData(new HeroesServer.SendPackets.SpawnCharacterPacket(false, character));
                }
            }
        }

        public override void Write()
        {
        }
    }
}
