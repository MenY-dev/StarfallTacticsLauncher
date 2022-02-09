using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StarfallTactics.StarfallTacticsServers
{
    public partial class StarfallProfile
    {
        #region Main

        public JsonNode CreateAllMyPropertyResponse()
        {
            JsonNode doc = new JsonObject
            {
                ["u_bm"] = 1,
                ["u_sfc"] = 999999999,
                ["u_ban"] = 0,
                ["u_charactslotlimit"] = 5,
                ["drop_ship_progression_param_sfc"] = 12,
                ["drop_ship_progression_param_igc"] = 23,
                ["production_points_cost_60_sfc"] = 34,
                ["production_points_cost_60_igc"] = 45,
                ["rush_open_weekly_reward"] = 56
            };

            JsonArray shopItems = new JsonArray();
            JsonArray availableSkinColors = new JsonArray();
            JsonArray availableShipDecals = new JsonArray();
            JsonArray availableShipSkins = new JsonArray();

            if ((Database is null) == false)
            {
                int id = -1;

                foreach (var item in Database.SkinColors)
                {
                    id++;

                    shopItems.Add(new JsonObject
                    {
                        ["id"] = id,
                        ["name"] = item.Name,
                        ["description"] = "Color",
                        ["itemtype"] = 0,
                        ["igcprice"] = 100,
                        ["itemtypespecificjson"] = new JsonObject()
                        {
                            ["skincolor_id"] = item.Id
                        }.ToJsonString()
                    });

                    availableSkinColors.Add(new JsonObject
                    {
                        ["id"] = item.Id
                    });
                }

                foreach (var item in Database.ShipSkins)
                {
                    availableShipSkins.Add(new JsonObject
                    {
                        ["id"] = item.Id
                    });
                }

                foreach (var item in Database.ShipDecals)
                {
                    id++;

                    shopItems.Add(new JsonObject
                    {
                        ["id"] = id,
                        ["name"] = item.Name,
                        ["description"] = "Decal",
                        ["itemtype"] = 2,
                        ["igcprice"] = 100,
                        ["itemtypespecificjson"] = new JsonObject()
                        {
                            ["decal_id"] = item.Id
                        }.ToJsonString()
                    });

                    availableShipDecals.Add(new JsonObject
                    {
                        ["id"] = item.Id
                    });
                }
            }

            doc["shop_items"] = shopItems;
            doc["available_skincolors"] = availableSkinColors;
            doc["available_decals"] = availableShipDecals;
            doc["available_shipskins"] = availableShipSkins;

            return doc;
        }

        public JsonNode CreateRealmsResponse()
        {
            JsonNode response = new JsonObject();
            JsonArray chars = new JsonArray();

            foreach (var item in CharacterModeProfile.Chars)
            {
                chars.Add(CreateCharacterInfoResponse(item));
            }

            JsonArray realms = new JsonArray
            {
                new JsonObject
                {
                    ["id"] = new JsonObject
                    {
                        ["$"] = 0
                    },
                    ["name"] = new JsonObject
                    {
                        ["$"] = "NewRealm"
                    },
                    ["chars"] = chars
                }
            };

            response["elem"] = realms;

            return response;
        }

        public JsonNode CreateCharacterInfoResponse() =>
            CreateCharacterInfoResponse(CurrentCharacter);

        public JsonNode CreateCharacterInfoResponse(Character character)
        {
            return new JsonObject
            {
                ["id"] = new JsonObject
                {
                    ["$"] = character?.Id ?? -1
                },
                ["name"] = new JsonObject
                {
                    ["$"] = character?.Name ?? string.Empty
                },
                ["faction"] = new JsonObject
                {
                    ["$"] = character?.Faction ?? 0
                },
                ["level"] = new JsonObject
                {
                    ["$"] = character?.Level ?? 100
                }
            };
        }

        public JsonNode CreateCharactEditResponse() =>
            CreateCharactEditResponse(CurrentCharacter);

        public JsonNode CreateCharactEditResponse(Character character)
        {
            return new JsonObject
            {
                ["charactid"] = new JsonObject
                {
                    ["$"] = character?.Id ?? 0
                }
            };
        }

        public JsonNode CreateCharactSelectResponse() =>
            CreateCharactSelectResponse(CurrentCharacter);

        public JsonNode CreateCharactSelectResponse(Character character)
        {
            return new JsonObject
            {
                ["charactername"] = new JsonObject
                {
                    ["$"] = character?.Name ?? string.Empty
                },
                ["realmmgrurl"] = new JsonObject
                {
                    ["$"] = "http://127.0.0.1:1500/realmmgr/"
                },
                ["temporarypass"] = new JsonObject
                {
                    ["$"] = TemporaryPass ?? string.Empty
                },
                ["chatacterfaction"] = new JsonObject
                {
                    ["$"] = character?.Faction ?? 0
                },
            };
        }

        #endregion
        #region Ranked Mode

        public JsonNode CreateDraftFleetsResponse()
        {
            JsonArray fleets = new JsonArray();

            fleets.Add(new JsonObject
            {
                ["id"] = new JsonObject
                {
                    ["$"] = 0
                },
                ["name"] = new JsonObject
                {
                    ["$"] = "0"
                },
                ["maxships"] = new JsonObject
                {
                    ["$"] = 20
                },
                ["ships"] = new JsonArray()
            });

            return new JsonObject
            {
                ["fleets"] = fleets,
                ["userbm"] = new JsonObject
                {
                    ["$"] = 1
                }
            };
        }

        #endregion
        #region Discovery Mode

        public JsonNode CreateCharacterDataResponse() =>
            CreateCharacterDataResponse(CurrentCharacter);

        public JsonNode CreateCharacterDataResponse(Character character)
        {
            return new JsonObject
            {
                ["data_result"] = new JsonObject
                {
                    ["$"] = CreateCharacterResponse(character).ToJsonString()
                }
            };
        }

        public JsonNode CreateCharacterResponse() =>
            CreateCharacterResponse(CurrentCharacter);

        public JsonNode CreateCharacterResponse(Character character)
        {
            if (character is null)
                return new JsonObject();

            JsonNode doc = new JsonObject
            {
                ["c_has_premium"] = character.HasPremium,
                ["c_xp_boost"] = character.XpBoost,
                ["c_igc_boost"] = character.IgcBoost,
                ["c_craft_boost"] = character.CraftBoost,
                ["c_premium_minutes_left"] = character.PremiumMinutesLeft,
                ["c_xp_minutes_left"] = character.XpMinutesLeft,
                ["c_igc_minutes_left"] = character.IgcMinutesLeft,
                ["c_craft_minutes_left"] = character.CraftMinutesLeft,
                ["igc"] = character.Igc,
                ["currentdetachment"] = character.CurrentDetachment,
                ["rank"] = character.Rank,
                ["level"] = character.Level,
                ["access_level"] = character.AccessLevel,
                ["ability_cells"] = character.AbilityCells,
                ["reputation"] = character.Reputation,
                ["xp"] = character.Xp,
                ["ship_slots"] = character.ShipSlots,
                ["has_active_session"] = character.HasActiveSession,
                ["selfservice"] = character.SelfService,
                ["char_for_tutorial"] = character.CharForTutorial,
                ["production_points"] = character.ProductionPoints,
                ["production_income"] = character.ProductionIncome,
                ["production_cap"] = character.ProductionCap,
                ["bgc"] = character.Bgc,
                ["bonus_xp"] = character.BonusXp,
                ["end_session_time"] = character.EndSessionTime,
                ["bonus_xp_income_minute_elapsed"] = character.BonusXpIncomeMinuteElapsed,
                ["has_session_results"] = character.HasSessionResults,
                ["indiscoverybattle"] = character.InDiscoveryBattle
            };

            JsonArray ships = new JsonArray();
            JsonArray inventory = new JsonArray();
            JsonArray crafting = new JsonArray();
            JsonArray checkedEvents = new JsonArray();

            foreach (var item in character.Ships)
                ships.Add(CreateShipResponse(item));

            foreach (var item in character.Inventory)
                inventory.Add(CreateInventoryItemResponse(item));

            foreach (var item in character.Crafting)
            {
                crafting.Add(new JsonObject
                {
                    ["id"] = item.CraftingId,
                    ["project_entity"] = item.ProjectEntity,
                    ["queue_position"] = item.QueuePosition,
                    ["production_points_spent"] = 99999,
                });
            }

            checkedEvents.Add(new JsonObject { ["event_id"] = "DiscoveryIntroTutorial" });
            checkedEvents.Add(new JsonObject { ["event_id"] = "DiscoveryMothershipTutorial" });
            checkedEvents.Add(new JsonObject { ["event_id"] = "DiscoveryCaravanTutorial" });
            checkedEvents.Add(new JsonObject { ["event_id"] = "DiscoveryPostCaravanTutorial" });
            checkedEvents.Add(new JsonObject { ["event_id"] = "DiscoveryQuestsTutorial" });
            checkedEvents.Add(new JsonObject { ["event_id"] = "DiscoveryAutopilotTutorial" });
            checkedEvents.Add(new JsonObject { ["event_id"] = "ShipyardBanDropSessionTutorial" });
            checkedEvents.Add(new JsonObject { ["event_id"] = "ShipyardIntroTutorial" });
            checkedEvents.Add(new JsonObject { ["event_id"] = "ShipyardShipEditTutorial" });
            checkedEvents.Add(new JsonObject { ["event_id"] = "ShipyardCraftingTutorial" });
            checkedEvents.Add(new JsonObject { ["event_id"] = "ShipyardShipBuildTutorial" });
            checkedEvents.Add(new JsonObject { ["event_id"] = "FirstChoiceTutorial" });

            doc["ships"] = ships;
            doc["inventory"] = inventory;
            doc["crafting"] = crafting;
            doc["checked_events"] = checkedEvents;

            return doc;
        }

        public JsonNode CreateDiscoveryCharacterResponse() =>
            CreateDiscoveryCharacterResponse(CurrentCharacter);

        public JsonNode CreateDiscoveryCharacterResponse(Character character)
        {
            if (character is null)
                return new JsonObject();

            JsonNode doc = new JsonObject
            {
                ["faction"] = character.Faction,
                ["charactname"] = character.Name,
                ["xp_factor"] = character.XpBoost,
                ["bonus_xp"] = character.BonusXp,
                ["access_level"] = character.AccessLevel,
                ["level"] = character.Level,
            };

            JsonArray ships = new JsonArray();
            JsonArray groups = new JsonArray();

            foreach (var item in character.Ships)
                ships.Add(CreateShipResponse(item));

            doc["ships_list"] = ships;
            doc["ship_groups"] = groups;

            return doc;
        }

        public JsonNode CreateCraftingResponse(IEnumerable<int> newShips = null, IEnumerable<int> newCrafts = null) =>
            CreateCraftingResponse(CurrentCharacter, newShips, newCrafts);

        public JsonNode CreateCraftingResponse(Character character, IEnumerable<int> newShips = null, IEnumerable<int> newCrafts = null)
        {
            if (character is null)
                return new JsonObject();

            JsonNode doc = new JsonObject();
            JsonArray ships = new JsonArray();
            JsonArray inventory = new JsonArray();
            JsonArray crafting = new JsonArray();

            if (newShips is null)
            {
                foreach (var item in character.Ships)
                    ships.Add(CreateShipResponse(item));
            }
            else
            {
                List<int> shipFilter = new List<int>(newShips);

                foreach (var item in character.Ships)
                    if (shipFilter.Contains(item.Id))
                        ships.Add(CreateShipResponse(item));
            }

            foreach (var item in character.Inventory)
                inventory.Add(CreateInventoryItemResponse(item));

            if (newCrafts is null)
            {
                foreach (var item in character.Crafting)
                {
                    crafting.Add(new JsonObject
                    {
                        ["id"] = item.CraftingId,
                        ["project_entity"] = item.ProjectEntity,
                        ["queue_position"] = item.QueuePosition,
                        ["production_points_spent"] = 99999,
                    });
                }
            }
            else
            {
                List<int> craftingFilter = new List<int>(newCrafts);

                foreach (var item in character.Crafting)
                {
                    if (craftingFilter.Contains(item.CraftingId))
                        crafting.Add(new JsonObject
                        {
                            ["id"] = item.CraftingId,
                            ["project_entity"] = item.ProjectEntity,
                            ["queue_position"] = item.QueuePosition,
                            ["production_points_spent"] = 99999,
                        });
                }
            }

            doc["ships"] = ships;
            doc["inventory"] = inventory;
            doc["crafting"] = crafting;

            return new JsonObject
            {
                ["data_result"] = new JsonObject { ["$"] = doc.ToJsonString() }
            };
        }

        public JsonNode CreateShipResponse(Ship ship)
        {
            if (ship is null)
                return new JsonObject();

            JsonNode doc = new JsonObject
            {
                ["id"] = ship.Id + IndexSpace,
                ["data"] = CreateShipDataResponse(ship).ToJsonString(),
                ["position"] = ship.Position,
                ["kills"] = ship.Kills,
                ["death"] = ship.Death,
                ["played"] = ship.Played,
                ["woncount"] = ship.WonCount,
                ["lostcount"] = ship.LostCount,
                ["xp"] = ship.Xp,
                ["level"] = 30,
                ["damagedone"] = ship.DamageDone,
                ["damagetaken"] = ship.DamageTaken,
                ["timetoconstruct"] = ship.TimeToConstruct,
                ["timetorepair"] = ship.TimeToRepair,
                ["is_favorite"] = ship.IsFavorite
            };

            return doc;
        }

        public JsonNode CreateShipDataResponse(Ship ship)
        {
            if (ship is null)
                return new JsonObject();


            JsonNode doc = new JsonObject
            {
                ["hull"] = ship.Hull,
                ["elid"] = ship.Id + IndexSpace,
                ["armor"] = -1,
                ["structure"] = -1,
                ["destroyed"] = 0,
                ["ship_skin"] = ship.ShipSkin,
                ["skin_color_1"] = ship.SkinColor1,
                ["skin_color_2"] = ship.SkinColor2,
                ["skin_color_3"] = ship.SkinColor3,
                ["shipdecal"] = ship.ShipDecal,
            };

            JsonArray hardpoints = new JsonArray();
            JsonArray progression = new JsonArray();

            foreach (var item in ship.HardpointList)
            {
                JsonArray equipments = new JsonArray();

                foreach (var eq in item.EquipmentList)
                {
                    equipments.Add(new JsonObject
                    {
                        ["eq"] = eq.Equipment,
                        ["x"] = eq.X,
                        ["y"] = eq.Y,
                    });
                }

                hardpoints.Add(new JsonObject
                {
                    ["hp"] = item.Hardpoint,
                    ["eqlist"] = equipments
                });
            }

            foreach (var item in ship.Progression)
            {
                progression.Add(new JsonObject
                {
                    ["id"] = item.Id,
                    ["points"] = item.Points
                });
            }

            doc["hplist"] = hardpoints;
            doc["progression"] = progression;

            return doc;
        }

        public JsonNode CreateInventoryItemResponse(InventoryItem item)
        {
            if (item is null)
                return new JsonObject();


            JsonNode doc = new JsonObject
            {
                ["itemtype"] = item.ItemType,
                ["id"] = item.Id,
                ["count"] = item.Count,
                ["unique_data"] = ""
            };

            return doc;
        }

        #endregion
    }
}
