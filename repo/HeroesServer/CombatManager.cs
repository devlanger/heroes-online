using HeroesServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerUtilities
{
    public class CombatManager
    {
        public class HitData
        {
            public CharacterInfo attacker;
            public List<CharacterInfo> targets = new List<CharacterInfo>();
        }

        public static void HitTarget(HitData data)
        {
            foreach (var item in data.targets)
            {
                if(item.Character.IsDead)
                {
                    continue;
                }

                item.Character.ChangeStatValue(StatType.HEALTH, -10);

                if(item.Character.GetStatValue(StatType.HEALTH) <= 0)
                {
                    item.Character.IsDead = true;
                }

                if(item.Character.IsDead)
                {
                    data.attacker.Character.ChangeStatValue(StatType.EXPERIENCE, item.Character.ExpReward);
                    CharactersManager.Instance.RemoveCharacter(item);
                }
            }
        }
    }
}
