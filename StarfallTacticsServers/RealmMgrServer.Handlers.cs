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
    public partial class RealmMgrServer
    {
        #region Crafting

        public JsonNode HandleStartCraftingRequest(ClientQuery query) =>
            HandleStartCraftingRequest(Profile.CurrentCharacter, query);

        public JsonNode HandleStartCraftingRequest(Character character, ClientQuery query)
        {
            int entity = (int?)query["project_entity"] ?? -1;

            if (character is null || entity < 0)
                return Profile.CreateCraftingResponse(new int[0], new int[0]);

            IEnumerable<int> newCrafts = new int[0];
            CraftingInfo craftingInfo = Profile.CurrentCharacter?.AddCraftingItem(entity);

            if ((craftingInfo is null) == false)
            {
                newCrafts = new[] { craftingInfo.CraftingId };
            }

            return Profile.CreateCraftingResponse(new int[0], newCrafts);
        }

        protected JsonNode HandleAcquireCraftedItemRequest(ClientQuery query) =>
            HandleAcquireCraftedItemRequest(Profile.CurrentCharacter, query);

        protected JsonNode HandleAcquireCraftedItemRequest(Character character, ClientQuery query)
        {
            int craftingId = (int?)query["craftingid"] ?? -1;

            if (character is null || craftingId < 0)
                return Profile.CreateCraftingResponse(new int[0], new int[0]);

            CraftingInfo craftingInfo = Profile.CurrentCharacter.GetCraftingItem(craftingId);
            IEnumerable<int> newShips = new int[0];

            if ((craftingInfo is null) == false)
            {
                Profile.CurrentCharacter.DeleteCraftingItem(craftingInfo);
                Ship newShip = Profile.CurrentCharacter.AddShip(craftingInfo.ProjectEntity);

                if ((newShip is null) == false)
                    newShips = new[] { newShip.Id };
            }

            return Profile.CreateCraftingResponse(newShips, new int[0]);
        }

        protected JsonNode HandleAcquireAllCraftedItemsRequest(ClientQuery query) =>
            HandleAcquireAllCraftedItemsRequest(Profile.CurrentCharacter, query);

        protected JsonNode HandleAcquireAllCraftedItemsRequest(Character character, ClientQuery query)
        {
            if (character is null)
                return Profile.CreateCraftingResponse(new int[0], new int[0]);

            List<int> newShips = new List<int>();

            foreach (var item in character.Crafting)
            {
                if (item is null)
                    continue;

                Ship newShip = Profile.CurrentCharacter.AddShip(item.ProjectEntity);

                if ((newShip is null) == false)
                    newShips.Add(newShip.Id);
            }

            character.Crafting.Clear();

            return Profile.CreateCraftingResponse(newShips, new int[0]);
        }

        #endregion
        #region Ship Editor

        public JsonNode HandleSaveShipRequest(ClientQuery query) =>
            HandleSaveShipRequest(Profile.CurrentCharacter, query);

        public JsonNode HandleSaveShipRequest(Character character, ClientQuery query)
        {
            JsonNode request = JsonNode.Parse((string)query["data"] ?? string.Empty);

            if (request is null || character is null)
                return new JsonObject { ["ok"] = 0 };

            int shipId = (int)(request["elid"] ?? -1);
            Ship ship = character.GetShip(shipId);

            if (ship is null)
                return new JsonObject { ["ok"] = 0 };

            JsonArray hardpoints = request["hplist"]?.AsArray();

            if (hardpoints != null)
            {
                ship.HardpointList.Clear();

                foreach (var item in hardpoints)
                {
                    JsonArray eqlist = item["eqlist"]?.AsArray();

                    if (eqlist is null)
                        continue;

                    List<HardpointEquipment> equipments = new List<HardpointEquipment>();

                    foreach (var eqItem in eqlist)
                    {
                        equipments.Add(new HardpointEquipment
                        {
                            Equipment = (int)(eqItem["eq"] ?? -1),
                            X = (int)(eqItem["x"] ?? 0),
                            Y = (int)(eqItem["y"] ?? 0)
                        });
                    }

                    ship.HardpointList.Add(new ShipHardpoint
                    {
                        Hardpoint = (string)item["hp"] ?? string.Empty,
                        EquipmentList = equipments
                    });
                }
            }

            JsonArray progression = request["progression"]?.AsArray();

            if (progression != null)
            {
                ship.Progression.Clear();

                foreach (var item in progression)
                {
                    ship.Progression.Add(new ShipProgression
                    {
                        Id = (int)(item["id"] ?? -1),
                        Points = (int)(item["points"] ?? 0),
                    });
                }
            }

            return new JsonObject { ["ok"] = 1 };
        }

        public JsonNode HandleShipDeleteRequest(ClientQuery query) =>
            HandleShipDeleteRequest(Profile.CurrentCharacter, query);

        public JsonNode HandleShipDeleteRequest(Character character, ClientQuery query)
        {
            int id = (int?)query["elid"] ?? -1;

            if (character is null || id < 0)
                return new JsonObject { ["ok"] = 0};

            character.DeleteShip(id);

            return new JsonObject { ["ok"] = 1 };
        }

        #endregion
    }
}
