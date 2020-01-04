using System;
using System.Collections.Generic;
using System.Text;
using ServerUtilities;

namespace HeroesServer
{
    public class Monster : Character
    {
        private CharacterInfo target;
        private float lastTargetCheck;
        public int ExpReward { get; set; }

        public override void Hit(CombatManager.HitData data)
        {
            base.Hit(data);

            this.target = data.attacker;
        }

        public override void Die(CharacterInfo character, CombatManager.HitData data)
        {
            if (data.attacker.Client != null)
            {
                data.attacker.Character.ChangeStatValue(StatType.EXPERIENCE, ExpReward);
            }

            CharactersManager.Instance.RemoveCharacter(character);
        }

        public override void SetData(CharactersManager.SpawnData data)
        {
            base.SetData(data);

            ExpReward = data.expReward;
        }

        public override void Update(CharacterInfo info)
        {
            if(target != null)
            {
                if(Server.Time > lastTargetCheck + 0.33f && target != null)
                {
                    CombatManager.HitTarget(new CombatManager.HitData()
                    {
                        attacker = info,
                        targets = new List<CharacterInfo>()
                        {
                            target
                        }
                    });

                    if(target.Character.GetStatValue(StatType.HEALTH) <= 0)
                    {
                        target = null;
                    }

                    lastTargetCheck = Server.Time;
                    Console.WriteLine("Hit target");
                }
            }
        }
    }
}
