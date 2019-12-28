using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace HeroesServer
{
    public class Character
    {
        public int Id { get; set; }
        public short BaseId { get; set; }
        public int ExpReward { get; set; }
        private Dictionary<StatType, int> stats = new Dictionary<StatType, int>();

        public bool IsDead { get; set; }
        public Vector3 Position { get; set; }
        public string Name { get; set; }
        public int DatabaseId { get; internal set; }

        public delegate void OnStatChanged(StatType stat, int oldValue, int newValue);
        public event OnStatChanged StatChanged = delegate { };

        public Character()
        {
            Name = "Character";

            foreach (var item in Enum.GetValues(typeof(StatType)))
            {
                stats.Add((StatType)item, 0);
            }

            stats[StatType.HEALTH] = 20;
            stats[StatType.LEVEL] = 1;
        }

        public int GetStatValue(StatType stat)
        {
            if(!stats.ContainsKey(stat))
            {
                return 0;
            }

            return stats[stat];
        }

        public void SetStat(StatType stat, int value)
        {
            if (!stats.ContainsKey(stat))
            {
                stats.Add(stat, value);
            }


            int oldValue = stats[stat];

            ChangedStatEffect(stat, ref value);
            stats[stat] = value;

            StatChanged(stat, oldValue, value);
        }

        public void ChangeStatValue(StatType stat, int difference)
        {
            int curVal = GetStatValue(stat);
            SetStat(stat, curVal + difference);
        }

        private void ChangedStatEffect(StatType stat, ref int newValue)
        {
            switch(stat)
            {
                case StatType.EXPERIENCE:
                    if(newValue >= 100)
                    {
                        newValue = 0;
                        ChangeStatValue(StatType.LEVEL, 1);
                    }
                    break;
            }
        }

        public void SetData(CharactersManager.SpawnData data)
        {
            Name = data.name;

            SetStat(StatType.HEALTH, data.health);
            SetStat(StatType.LEVEL, data.lvl);

            ExpReward = data.expReward;
        }
    }
}
