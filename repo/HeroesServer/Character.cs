using ServerUtilities;
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

            stats[StatType.HEALTH] = 100;
            stats[StatType.LEVEL] = 1;
        }


        public virtual void Hit(CombatManager.HitData data)
        {
        }

        public virtual void Die(CharacterInfo character, CombatManager.HitData data)
        {

        }

        public int GetStatValue(StatType stat)
        {
            if(!stats.ContainsKey(stat))
            {
                return 0;
            }

            return stats[stat];
        }

        public virtual void Update(CharacterInfo info)
        {

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
                        ChangeStatValue(StatType.SKILL_POINTS, 5);
                    }
                    break;
                case StatType.STAMINA:
                    if (newValue >= 100)
                    {
                        newValue = 100;
                    }
                    break;
            }
        }

        public virtual void SetData(CharactersManager.SpawnData data)
        {
            Name = data.name;

            SetStat(StatType.MAX_HEALTH, data.health);
            SetStat(StatType.HEALTH, data.health);
            SetStat(StatType.LEVEL, data.lvl);
        }
    }
}
