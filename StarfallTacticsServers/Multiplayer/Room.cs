using StarfallTactics.StarfallTacticsServers.Instances;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Multiplayer
{
    public class Room
    {
        public Guid Id { get; }

        public int Size { get; set; } = 1;

        public RoomState State { get; set; } = RoomState.None;

        public Dictionary<Player, QueueEntry> Queue { get; } = new Dictionary<Player, QueueEntry>();

        public Instance Instance { get; set; }

        public QueueEntry this[Player i]
        {
            get
            {
                if (Queue.TryGetValue(i, out QueueEntry value))
                    return value;

                return null;
            }
        }

        public Player this[QueueEntry i]
        {
            get
            {
                foreach (var item in Queue)
                    if (item.Value == i)
                        return item.Key;

                return null;
            }
        }

        public Room()
        {
            Id = Guid.NewGuid();
        }

        public virtual void AddPlayer(Player player, QueueEntry entry)
        {
            RemovePlayer(player);
            Queue.Add(player, entry);
        }

        public virtual QueueEntry AddPlayer(Player player)
        {
            QueueEntry entry = new QueueEntry(player);
            RemovePlayer(player);
            Queue.Add(player, entry);
            return entry;
        }

        public virtual void RemovePlayer(Player player)
        {
            if (Queue.ContainsKey(player))
                Queue.Remove(player);
        }

        public virtual Player GetPlayer(int id)
        {
            if (id < 0)
                return null;

            foreach (var item in Queue.Keys)
                if (item.Id == id)
                    return item;

            return null;
        }

        public virtual Player GetPlayer(string auth)
        {
            if (string.IsNullOrWhiteSpace(auth))
                return null;

            foreach (var item in Queue.Keys)
                if (item.Auth == auth)
                    return item;

            return null;
        }
    }
}
