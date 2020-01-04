using HeroesServer.SendPackets;
using ServerUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Text;

namespace LoginServer.ReceivedPackets
{
    public class LoginPacket : PacketBase
    {
        public LoginPacket() : base()
        {
        }

        public override void Read(Client client)
        {
            string username = reader.ReadString();
            string password = reader.ReadString();

            DataTable table = DatabaseManager.ReturnQuery(string.Format("SELECT * FROM accounts WHERE username='{0}' AND password='{1}'", username, password));
            if (table.Rows.Count > 0)
            {
                int accountId = (int)table.Rows[0]["id"];

                DataTable character = DatabaseManager.ReturnQuery(string.Format("SELECT * FROM characters WHERE account_id={0}", accountId));
                if (character.Rows.Count > 0)
                {
                    int characterId = (int)character.Rows[0]["id"];
                    string sessionHash = Base64Encode();

                    DatabaseManager.InsertQuery(string.Format("UPDATE characters SET session_hash='{0}' WHERE id={1}", sessionHash, accountId));
                    client.Session = new CharacterSession()
                    {
                        id = characterId,
                        hash = sessionHash,
                    };
                    client.SendData(new LoginOkPacket());
                    Console.WriteLine("Log in successfully.");
                }
                else
                {
                    Console.WriteLine("Log in failed.");
                }
            }
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
