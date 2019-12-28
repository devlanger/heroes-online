using HeroesServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerUtilities
{
    public class CharacterDB
    {
        public static void SaveCharacters(List<Character> players)
        {
            if (players.Count == 0)
            {
                return;
            }

            HashSet<int> playerIds = new HashSet<int>();

            foreach (var player in players)
            {
                playerIds.Add(player.DatabaseId);
            }

            string ids = DatabaseManager.PackIds(playerIds);

            //Remove items
            //DatabaseManager.ReturnQuery(string.Format("DELETE FROM items WHERE owner_id IN {0} AND window <> 3", ids));
            //Remove account warehouse items
            //DatabaseManager.ReturnQuery(string.Format("DELETE FROM items WHERE owner_id IN {0} AND window = 3", DatabaseManager.PackIds(accountsIds)));
            //DatabaseManager.ReturnQuery(string.Format("DELETE FROM guild_members WHERE player_id IN {0}", ids));
            //DatabaseManager.ReturnQuery(string.Format("DELETE FROM quickslots WHERE player_id IN {0}", ids));

            SaveCharacterStats(players);
        }

        private static void SaveCharacterStats(List<Character> players)
        {
            string sqlItems = "";
            foreach (var player in players)
            {
                sqlItems += string.Format("({0}, {1}, {2}, {3}, {4}, {5}, {6}),", player.DatabaseId, player.GetStatValue(StatType.HEALTH),
                            DatabaseManager.FloatToDouble(player.Position.X), DatabaseManager.FloatToDouble(player.Position.Y), DatabaseManager.FloatToDouble(player.Position.Z), player.GetStatValue(StatType.LEVEL), player.GetStatValue(StatType.EXPERIENCE));
            }

            if (sqlItems.Length > 0)
            {
                sqlItems = sqlItems.Remove(sqlItems.Length - 1);
            }

            string sql = string.Format("INSERT INTO characters ({0}) VALUES {1} ON DUPLICATE KEY UPDATE health=VALUES(health), pos_x=VALUES(pos_x), pos_y=VALUES(pos_y), pos_z=VALUES(pos_z), level=VALUES(level), " +
                        "experience=VALUES(experience)", "id, health, pos_x, pos_y, pos_z, level, experience", sqlItems);

            DatabaseManager.ReturnQuery(sql);
        }
    }
}
