using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace HeroesServer
{
    public class Zone
    {
        public int Id;
        public List<Location> locations = new List<Location>();

        public Location GetLocation(Vector3 position)
        {
            Location loc = null;
            float distVal = 999999;

            foreach (var item in locations)
            {
                Vector3 pos = item.position;
                float dist = Vector3.Distance(pos, position);
                if (dist < distVal)
                {
                    loc = item;
                    distVal = dist;
                }
            }

            return loc;
        }
    }
}
