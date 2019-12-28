using ENet;
using ServerUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HeroesServer
{
    class Server
    {
        public Server()
        {
            ENet.Library.Initialize();
            ClientsManager clients = new ClientsManager();

            DatabaseManager.OnDatabaseTick += () =>
            {
                List<CharacterInfo> characters = CharactersManager.Instance.characters.Values.ToList();

                if (characters.Count != 0)
                {
                    List<Character> charactersForUpdate = new List<Character>();

                    foreach (var player in characters)
                    {
                        charactersForUpdate.Add(player.Character);
                    }

                    if (charactersForUpdate.Count != 0)
                    {
                        CharacterDB.SaveCharacters(charactersForUpdate);

                        //ServerLogger.Log(null, "Updated database | Query time: " + (Server.Time - startTime).ToString(), LoggingLevel.PROD);
                    }
                }
            };

            using (Host server = new Host())
            {
                Console.WriteLine("Game Server started...");
                Address address = new Address();
                address.SetIP("127.0.0.1");
                address.Port = 2601;
                server.Create(address, 100);

                Event netEvent;

                while (!Console.KeyAvailable)
                {
                    bool polled = false;

                    while (!polled)
                    {
                        if (server.CheckEvents(out netEvent) <= 0)
                        {
                            if (server.Service(15, out netEvent) <= 0)
                                break;

                            polled = true;
                        }

                        switch (netEvent.Type)
                        {
                            case EventType.None:
                                break;

                            case EventType.Connect:
                                Console.WriteLine("Client connected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);

                                CharacterInfo info = CharactersManager.Instance.AddCharacter(new CharacterInfo()
                                {
                                    Character = new Hero()
                                    {
                                        Position = new System.Numerics.Vector3(127, 14, 100)
                                    },
                                    ZoneId = 0
                                });

                                GameClient c = new GameClient()
                                {
                                    Peer = netEvent.Peer,
                                    CharacterInfo = info
                                };

                                c.CharacterInfo.Client = c;
                                ClientsManager.Instance.clients.Add(netEvent.Peer.ID, c);
                                break;

                            case EventType.Disconnect:
                                Console.WriteLine("Client disconnected - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);


                                GameClient clientToRemove = (GameClient)ClientsManager.Instance.clients[netEvent.Peer.ID];
                                CharacterDB.SaveCharacters(new List<Character>() { clientToRemove.CharacterInfo.Character });
                                ClientsManager.Instance.clients.Remove(netEvent.Peer.ID);
                                CharactersManager.Instance.RemoveCharacter(clientToRemove.CharacterInfo);
                                break;

                            case EventType.Timeout:
                                Console.WriteLine("Client timeout - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP);
                                break;

                            case EventType.Receive:
                                //Console.WriteLine("Packet received from - ID: " + netEvent.Peer.ID + ", IP: " + netEvent.Peer.IP + ", Channel ID: " + netEvent.ChannelID + ", Data length: " + netEvent.Packet.Length);
                                byte[] buffer = new byte[1024];
                                netEvent.Packet.CopyTo(buffer);

                                GamePacketsManager.ReadBytes(ClientsManager.Instance.clients[netEvent.Peer.ID], buffer, netEvent.Packet.Length);

                                netEvent.Packet.Dispose();
                                break;
                        }
                    }
                }

                server.Flush();
            }
        }
    }
}
