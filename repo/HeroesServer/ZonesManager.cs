using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace HeroesServer
{
    public class ZonesManager
    {
        public Dictionary<int, Zone> zones = new Dictionary<int, Zone>();
    }

    public class Location
    {
        public Vector3 position;
    }
}
