using HeroesServer;
using ServerUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HeroesServer
{
    public abstract class PacketsManager<T>
    {
        public static T Instance { get; protected set; }
        protected static Dictionary<byte, Type> packets = new Dictionary<byte, Type>();

        public abstract void Initialize();
        
        public static void ReadBytes(Client client, byte[] buffer, int length)
        {
            MemoryStream stream = new MemoryStream(buffer);
            BinaryReader reader = new BinaryReader(stream);

            while (reader.BaseStream.Position != length)
            {
                byte packetId = reader.ReadByte();
                if (packets.ContainsKey(packetId))
                {
                    try
                    {
                        PacketBase packet = (PacketBase)Activator.CreateInstance(packets[packetId]);
                        packet.SetReader(reader, stream);
                        packet.Read(client);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("No packet with id: " + packetId);
                }
            }
        }
    }
}
