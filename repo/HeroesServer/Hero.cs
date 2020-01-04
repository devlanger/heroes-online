using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesServer
{
    public class Hero : Character
    {
        public Nature Nature { get; set; }
        public int SkillPoints { get; set; } = 0;

        private float lastRegenTime;

        public override void Update(CharacterInfo info)
        {
            base.Update(info);

            if(Server.Time > lastRegenTime + 0.5f)
            {
                ChangeStatValue(StatType.STAMINA, 5);
                lastRegenTime = Server.Time;
            }
        }
    }

    public enum Nature
    {
        FIRE = 1,
        LIGHTNING = 2,
        EARTH = 3,
        WOOD = 4
    }
}
