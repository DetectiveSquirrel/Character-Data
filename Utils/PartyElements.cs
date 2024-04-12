using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CharacterData.Utils;

public class PartyElements
{
    private const int PlayerNameChildIndex = 0;

    public static List<PartyElementWindow> GetPlayerInfoElementList(GameController gameController)
    {
        var playersInParty = new List<PartyElementWindow>();

        try
        {
            var baseWindow = gameController.IngameState.IngameUi.PartyElement;
            var partElementList = baseWindow?.Children[0]?.Children[0]?.Children;
            if (partElementList != null)
            {
                foreach (var partyElement in partElementList)
                {
                    var playerName = partyElement?.Children[PlayerNameChildIndex].Text;

                    var newElement = new PartyElementWindow
                    {
                        PlayerName = playerName,
                        Element = partyElement
                    };

                    // Get entity
                    foreach (var entity in gameController.EntityListWrapper.ValidEntitiesByType[EntityType.Player]
                                 .Where(entity => entity != null && entity.GetComponent<Player>().PlayerName == playerName))
                    {
                        newElement.Player = entity;
                        break; // No need to continue if entity found
                    }

                    playersInParty.Add(newElement);
                }
            }
        }
        catch (Exception)
        {
            return playersInParty;
        }

        return playersInParty;
    }
}

public class PartyElementWindow
{
    public string PlayerName { get; set; } = string.Empty;
    public Entity Player { get; set; }
    public Element Element { get; set; } = new();

    public override string ToString() => $"PlayerName: {PlayerName}, Data.PlayerEntity.Distance: {Player?.Distance(Entity.Player).ToString(CultureInfo.InvariantCulture) ?? "Null"}";
}