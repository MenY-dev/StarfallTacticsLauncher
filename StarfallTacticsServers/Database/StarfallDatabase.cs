using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers.Database
{
    public class StarfallDatabase
    {
        [JsonPropertyName("items")]
        public List<ItemEntry> Items { get; set; } = new List<ItemEntry>();

        [JsonPropertyName("discovery_items")]
        public List<ItemEntry> DiscoveryItems { get; set; } = new List<ItemEntry>();

        [JsonPropertyName("ships_npc_criterion")]
        public List<ShipEntry> CriterionShips { get; set; } = new List<ShipEntry>();

        [JsonPropertyName("ships_yoba")]
        public List<ShipEntry> YobaShips { get; set; } = new List<ShipEntry>();

        [JsonPropertyName("ships_deprived")]
        public List<ShipEntry> DeprivedShips { get; set; } = new List<ShipEntry>();

        [JsonPropertyName("ships_eclipse")]
        public List<ShipEntry> EclipseShips { get; set; } = new List<ShipEntry>();

        [JsonPropertyName("ships_vanguard")]
        public List<ShipEntry> VanguardShips { get; set; } = new List<ShipEntry>();

        [JsonPropertyName("ships_npc_free_traders")]
        public List<ShipEntry> FreeTradersShips { get; set; } = new List<ShipEntry>();

        [JsonPropertyName("ships_npc_mineworkers_union")]
        public List<ShipEntry> MineworkersUnionShips { get; set; } = new List<ShipEntry>();

        [JsonPropertyName("ships_npc_nebulords")]
        public List<ShipEntry> NebulordsShips { get; set; } = new List<ShipEntry>();

        [JsonPropertyName("ships_npc_pyramid")]
        public List<ShipEntry> PyramidShips { get; set; } = new List<ShipEntry>();

        [JsonPropertyName("ships_npc_screechers")]
        public List<ShipEntry> ScreechersShips { get; set; } = new List<ShipEntry>();

        [JsonPropertyName("ships_npc_raid")]
        public List<ShipEntry> RaidShips { get; set; } = new List<ShipEntry>();

        [JsonPropertyName("ships_warpbeacons")]
        public List<ShipEntry> WarpbeaconsShips { get; set; } = new List<ShipEntry>();

        [JsonPropertyName("ships_battlegrounds")]
        public List<ShipEntry> BattlegroundsShips { get; set; } = new List<ShipEntry>();

        [JsonPropertyName("ship_decals")]
        public List<ShipDecalEntry> ShipDecals { get; set; } = new List<ShipDecalEntry>();

        [JsonPropertyName("ship_skins")]
        public List<ShipSkinEntry> ShipSkins { get; set; } = new List<ShipSkinEntry>();

        [JsonPropertyName("skin_colors")]
        public List<SkinColorEntry> SkinColors { get; set; } = new List<SkinColorEntry>();

        public Faction GetShipFaction(int hull)
        {
            foreach (var item in NebulordsShips)
                if (item.Hull == hull)
                    return Faction.Nebulords;

            foreach (var item in PyramidShips)
                if (item.Hull == hull)
                    return Faction.Pyramid;

            foreach (var item in ScreechersShips)
                if (item.Hull == hull)
                    return Faction.Screechers;

            foreach (var item in CriterionShips)
                if (item.Hull == hull)
                    return Faction.Criterion;

            foreach (var item in YobaShips)
                if (item.Hull == hull)
                    return Faction.Yoba;

            foreach (var item in DeprivedShips)
                if (item.Hull == hull)
                    return Faction.Deprived;

            foreach (var item in EclipseShips)
                if (item.Hull == hull)
                    return Faction.Eclipse;

            foreach (var item in VanguardShips)
                if (item.Hull == hull)
                    return Faction.Vanguard;

            foreach (var item in FreeTradersShips)
                if (item.Hull == hull)
                    return Faction.FreeTraders;

            foreach (var item in MineworkersUnionShips)
                if (item.Hull == hull)
                    return Faction.MineworkersUnion;

            return Faction.Other;
        }
    }
}
