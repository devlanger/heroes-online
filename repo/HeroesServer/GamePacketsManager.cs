using HeroesServer;
using HeroesServer.ReceivedPackets;
using HeroesServer.SendPackets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HeroesServer
{
    public class GamePacketsManager : PacketsManager<GamePacketsManager>
    {
        public GamePacketsManager()
        {
            Instance = this;
        }

        public override void Initialize()
        {
            packets = new Dictionary<byte, Type>()
            {
                { 1, typeof(HeroesServer.ReceivedPackets.JoinGamePacket) },
                { 2, typeof(HeroesServer.ReceivedPackets.PlayerStatePacket) },
                { 3, typeof(HeroesServer.ReceivedPackets.HitPacket) },
            };

            Console.WriteLine("Packets manager initialized.");
        }
    }
}
