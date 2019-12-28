using HeroesServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LoginServer
{
    public class LoginPacketsManager : PacketsManager<LoginPacketsManager>
    {
        public LoginPacketsManager()
        {
            Instance = this;
        }

        public override void Initialize()
        {
            packets = new Dictionary<byte, Type>()
            {
                { 1, typeof(LoginServer.ReceivedPackets.LoginPacket) }
            };

            Console.WriteLine("Packets manager initialized.");
        }
    }
}
