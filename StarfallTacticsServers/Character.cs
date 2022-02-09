using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public class Character
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } = 0;

        [JsonPropertyName("name")]
        public string Name { get; set; } = "NewCharacter";

        [JsonPropertyName("faction")]
        public int Faction { get; set; } = 0;

        [JsonPropertyName("has_premium")]
        public int HasPremium { get; set; } = 1;

        [JsonPropertyName("xp_boost")]
        public int XpBoost { get; set; } = 1;

        [JsonPropertyName("igc_boost")]
        public int IgcBoost { get; set; } = 1;

        [JsonPropertyName("craft_boost")]
        public int CraftBoost { get; set; } = 1;

        [JsonPropertyName("premium_minutes_left")]
        public int PremiumMinutesLeft { get; set; } = 999999;

        [JsonPropertyName("xp_minutes_left")]
        public int XpMinutesLeft { get; set; } = 999999;

        [JsonPropertyName("igc_minutes_left")]
        public int IgcMinutesLeft { get; set; } = 999999;

        [JsonPropertyName("craft_minutes_left")]
        public int CraftMinutesLeft { get; set; } = 999999;

        [JsonPropertyName("igc")]
        public int Igc { get; set; } = 999999999;

        [JsonPropertyName("currentdetachment")]
        public int CurrentDetachment { get; set; } = 1820250578;

        [JsonPropertyName("rank")]
        public int Rank { get; set; } = 0;

        [JsonPropertyName("level")]
        public int Level { get; set; } = 100;

        [JsonPropertyName("access_level")]
        public int AccessLevel { get; set; } = 7;

        [JsonPropertyName("ability_cells")]
        public int AbilityCells { get; set; } = 7;

        [JsonPropertyName("reputation")]
        public int Reputation { get; set; } = 1;

        [JsonPropertyName("xp")]
        public int Xp { get; set; } = 999999999;

        [JsonPropertyName("ship_slots")]
        public int ShipSlots { get; set; } = 999;

        [JsonPropertyName("has_active_session")]
        public int HasActiveSession { get; set; } = 0;

        [JsonPropertyName("selfservice")]
        public string SelfService { get; set; } = "";

        [JsonPropertyName("char_for_tutorial")]
        public int CharForTutorial { get; set; } = 0;

        [JsonPropertyName("production_points")]
        public int ProductionPoints { get; set; } = 999999999;

        [JsonPropertyName("production_income")]
        public int ProductionIncome { get; set; } = 0;

        [JsonPropertyName("production_cap")]
        public int ProductionCap { get; set; } = 0;

        [JsonPropertyName("bgc")]
        public int Bgc { get; set; } = 999999999;

        [JsonPropertyName("bonus_xp")]
        public int BonusXp { get; set; } = 999999999;

        [JsonPropertyName("end_session_time")]
        public int EndSessionTime { get; set; } = 999999999;

        [JsonPropertyName("bonus_xp_income_minute_elapsed")]
        public int BonusXpIncomeMinuteElapsed { get; set; } = 0;

        [JsonPropertyName("has_session_results")]
        public int HasSessionResults { get; set; } = 0;

        [JsonPropertyName("indiscoverybattle")]
        public int InDiscoveryBattle { get; set; } = 0;

        [JsonPropertyName("ships")]
        public List<Ship> Ships { get; set; } = new List<Ship>();

        [JsonPropertyName("inventory")]
        public List<InventoryItem> Inventory { get; set; } = new List<InventoryItem>();

        [JsonPropertyName("crafting")]
        public List<CraftingInfo> Crafting { get; set; } = new List<CraftingInfo>();

        public void AddInventoryItem(int id, int count) => AddInventoryItem(id, count, 2);

        public void AddInventoryItem(int id, int count, int type)
        {
            if (Inventory is null)
                Inventory = new List<InventoryItem>();

            Inventory.Add(new InventoryItem()
            {
                Id = id,
                Count = count,
                ItemType = type
            });
        }

        public void DeleteInventoryItem(int id)
        {
            if (Inventory is null)
                Inventory = new List<InventoryItem>();

            if (id < 0)
                return;

            for (int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i].Id == id)
                {
                    Inventory.RemoveAt(i);
                    break;
                }
            }
        }

        public void DeleteInventoryItem(InventoryItem item)
        {
            if (item is null)
                return;

            if (Inventory is null)
                Inventory = new List<InventoryItem>();

            if (Inventory.Contains(item))
                Inventory.Remove(item);
        }

        public InventoryItem GetInventoryItem(int id)
        {
            if (Inventory is null)
                Inventory = new List<InventoryItem>();

            if (id < 0)
                return null;

            foreach (var item in Inventory)
                if (item.Id == id)
                    return item;

            return null;
        }

        public Ship AddShip(int hull)
        {
            if (Ships is null)
                Ships = new List<Ship>();

            int id = CreateId(0, Ships, i => i.Id);

            if (id < 0)
                return null;

            Ship ship = new Ship()
            {
                Hull = hull,
                Id = id
            };

            Ships.Add(ship);

            return ship;
        }

        public void DeleteShip(int elid)
        {
            if (Ships is null)
                Ships = new List<Ship>();

            if (elid < 0)
                return;

            for (int i = 0; i < Ships.Count; i++)
            {
                if (Ships[i].Id == elid)
                {
                    Ships.RemoveAt(i);
                    break;
                }
            }
        }

        public void DeleteShip(Ship ship)
        {
            if (ship is null)
                return;

            if (Ships is null)
                Ships = new List<Ship>();

            if (Ships.Contains(ship))
                Ships.Remove(ship);
        }

        public Ship GetShip(int elid)
        {
            if (Ships is null)
                Ships = new List<Ship>();

            if (elid < 0)
                return null;

            foreach (var item in Ships)
                if (item.Id == elid)
                    return item;

            return null;
        }

        public CraftingInfo AddCraftingItem(int entity)
        {
            if (Crafting is null)
                Crafting = new List<CraftingInfo>();

            int id = CreateId(0, Crafting, i => i.CraftingId);

            if (id < 0)
                return null;

            CraftingInfo info = new CraftingInfo()
            {
                CraftingId = id,
                ProjectEntity = entity,
                QueuePosition = 1
            };

            Crafting.Add(info);

            return info;
        }

        public void DeleteCraftingItem(int id)
        {
            if (Crafting is null)
                Crafting = new List<CraftingInfo>();

            if (id < 0)
                return;

            for (int i = 0; i < Crafting.Count; i++)
            {
                if (Crafting[i].CraftingId == id)
                {
                    Crafting.RemoveAt(i);
                    break;
                }
            }
        }

        public void DeleteCraftingItem(CraftingInfo item)
        {
            if (Crafting is null)
                Crafting = new List<CraftingInfo>();

            if (Crafting.Contains(item))
                Crafting.Remove(item);
        }

        public CraftingInfo GetCraftingItem(int id)
        {
            if (Crafting is null)
                Crafting = new List<CraftingInfo>();

            if (id < 0)
                return null;

            foreach (var item in Crafting)
                if (item.CraftingId == id)
                    return item;

            return null;
        }

        protected int CreateId<T>(int startID, IEnumerable<T> list, Func<T, int> walker)
        {
            if (startID < 0 || list is null || walker is null)
                return -1;

            List<T> items = new List<T>(list);
            int id = startID;

            for (int i = 0; i < items.Count; i++)
            {
                bool isEmpty = true;

                foreach (var item in items)
                {
                    if (walker.Invoke(item) == id)
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty == true)
                    return id;

                id++;
            }

            return id;
        }
    }
}
