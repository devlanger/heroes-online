using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;

namespace HeroesServer
{
    public class CharactersManager
    {
        public static CharactersManager Instance;
        public Dictionary<int, CharacterInfo> characters = new Dictionary<int, CharacterInfo>();
        public Dictionary<int, Dictionary<int, CharacterInfo>> zonesBucket = new Dictionary<int, Dictionary<int, CharacterInfo>>();
        public Dictionary<int, SpawnData> mobsProto = new Dictionary<int, SpawnData>();

        public static int lastId = 1;

        private Thread updateThread;

        public struct SpawnData
        {
            public int id;
            public string name;
            public byte lvl;
            public int health;
            public int expReward;
            public ushort min_dmg;
            public ushort max_dmg;
        }

        public CharactersManager()
        {
            Instance = this;

            zonesBucket.Add(0, new Dictionary<int, CharacterInfo>());
            LoadMonstersDatabase();

            updateThread = new Thread(new ThreadStart(UpdateCharacters));
            updateThread.Start();
        }

        private void LoadMonstersDatabase()
        {
            DataTable mobsProtoTable = DatabaseUtils.ReturnQuery("SELECT * FROM mobs_proto");
            for (int i = 0; i < mobsProtoTable.Rows.Count; i++)
            {
                SpawnData data = new SpawnData()
                {
                    id = (int)mobsProtoTable.Rows[i]["id"],
                    name = (string)mobsProtoTable.Rows[i]["name"],
                    lvl = (byte)mobsProtoTable.Rows[i]["lvl"],
                    health = (int)mobsProtoTable.Rows[i]["health"],
                    expReward = (int)mobsProtoTable.Rows[i]["exp_reward"],
                    max_dmg = (ushort)mobsProtoTable.Rows[i]["max_dmg"],
                    min_dmg = (ushort)mobsProtoTable.Rows[i]["min_dmg"],
                };

                mobsProto.Add(data.id, data);
            }

            DataTable table = DatabaseUtils.ReturnQuery("SELECT * FROM mobs_spawn");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                int baseId = (int)table.Rows[i]["mob_id"];
                Vector3 pos = new Vector3((float)(double)table.Rows[i]["pos_x"], (float)(double)table.Rows[i]["pos_y"], (float)(double)table.Rows[i]["pos_z"]);
                float respawnTime = (int)table.Rows[i]["respawn_time"];
                int zoneId = (int)table.Rows[i]["zone_id"];

                if(!mobsProto.ContainsKey(baseId))
                {
                    Console.WriteLine("Missing mob proto for spawn id: " + baseId);
                    continue;
                }

                SpawnData data = mobsProto[baseId];
                CharacterInfo info = new CharacterInfo()
                {
                    Character = new Monster()
                    {
                        Id = lastId++,
                        BaseId = (short)baseId,
                        Position = pos,
                    },
                    ZoneId = zoneId
                };

                info.Character.SetData(data);

                AddCharacter(info);
            }
        }

        private void UpdateCharacters()
        {
            while (true)
            {
                foreach (var zone in zonesBucket)
                {
                    foreach (var info in zone.Value.Values)
                    {
                        info.Update();
                    }
                }

                Thread.Sleep(100);
            }
        }

        public CharacterInfo AddCharacter(CharacterInfo info)
        {
            int newId = lastId++;
            info.Character.Id = newId;

            characters.Add(info.Character.Id, info);
            zonesBucket[info.ZoneId].TryAdd(info.Character.Id, info);

            foreach (var item in zonesBucket[info.ZoneId])
            {
                if (item.Value.Character.Id != info.Character.Id)
                {
                    if (item.Value.Client != null)
                    {
                        item.Value.Client.SendData(new SendPackets.SpawnCharacterPacket(false, info));
                    }
                }
            }

            info.Init();
            return info;
        }

        public void RemoveCharacter(CharacterInfo info)
        {
            characters.Remove(info.Character.Id);
            zonesBucket[info.ZoneId].Remove(info.Character.Id);

            foreach (var item in zonesBucket[info.ZoneId])
            {
                if (item.Value.Client != null)
                {
                    item.Value.Client.SendData(new SendPackets.DespawnCharacterPacket(info));
                }
            }

            info.Dispose();
        }
        public CharacterInfo GetCharacterInfo(int id)
        {
            return characters[id];
        }

        public bool GetCharacterInfo(int id, out CharacterInfo character)
        {
            if(characters.ContainsKey(id))
            {
                character = characters[id];
                return true;
            }
            else
            {
                character = null;
                return false;
            }
        }

        public Dictionary<int, CharacterInfo> GetPlayers(int zoneId)
        {
            Dictionary<int, CharacterInfo> result = new Dictionary<int, CharacterInfo>();
            foreach (var item in zonesBucket[zoneId].Values.ToList())
            {
                if(item.Client != null)
                {
                    result.Add(item.Character.Id, item);
                }
            }

            return result;
        }
    }

    
}
