using System;
using System.Collections.Generic;
using System.Text;
using ServerUtilities;

namespace HeroesServer
{
    public class CharacterInfo
    {
        public int ZoneId { get; set; }
        public GameClient Client { get; set; }
        public Character Character { get; set; }

        public void Init()
        {
            Character.StatChanged += Character_StatChanged;
        }

        private void Character_StatChanged(StatType stat, int oldValue, int newValue)
        {
            switch (stat)
            {
                case StatType.HEALTH:
                case StatType.LEVEL:
                    foreach (var item in CharactersManager.Instance.GetPlayers(ZoneId))
                    {
                        item.Value.Client.SendData(new StatSyncPacket(Character.Id, stat, newValue));
                    }
                    break;
                case StatType.STAMINA:
                case StatType.SKILL_POINTS:
                case StatType.EXPERIENCE:
                    if (Client != null)
                    {
                        Client.SendData(new StatSyncPacket(Character.Id, stat, newValue));
                    }
                    break;
            }
        }

        public void Update()
        {
            if (Client != null)
            {
                UpdateClient();
            }

            Character.Update(this);
        }

        private void UpdateClient()
        {
            List<SendPackets.SetPositionsPacket.CharacterSnapshot> snapshots = new List<SendPackets.SetPositionsPacket.CharacterSnapshot>();
            foreach (var item in CharactersManager.Instance.zonesBucket[ZoneId])
            {
                if (item.Value.Character.Id != Character.Id)
                {
                    snapshots.Add(new SendPackets.SetPositionsPacket.CharacterSnapshot()
                    {
                        id = item.Value.Character.Id,
                        pos = item.Value.Character.Position
                    });
                }
            }

            Client.SendData(new SendPackets.SetPositionsPacket(snapshots));
        }

        public void Dispose()
        {
            Character.StatChanged -= Character_StatChanged;

            Client = null;
            Character = null;
        }

    }
}
