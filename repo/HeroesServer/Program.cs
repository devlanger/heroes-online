using ServerUtilities;
using System;

namespace HeroesServer
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseManager.Initialize();
            new GamePacketsManager().Initialize();
            CharactersManager characters = new CharactersManager();
            DatabaseManager db = new DatabaseManager();
            Server server = new Server();
        }
    }
}
