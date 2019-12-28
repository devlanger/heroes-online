using ServerUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoginServer
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseManager.Initialize();
            new LoginPacketsManager().Initialize();
            DatabaseManager db = new DatabaseManager();
            Server server = new Server();
        }
    }
}
