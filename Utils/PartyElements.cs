using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.MemoryObjects;
using ExileCore.Shared.Enums;
using System;
using System.Collections.Generic;

namespace CharacterData.Utils
{
    public class PartyElements
    {
        private const int BaseWindowChildIndex = 1;
        private const int PartyListIndex = 0;
        private const int PlayerNameChildIndex = 0;
        private const int TPButtonIndex = 3;

        public static List<string> ListOfPlayersInParty(GameController gameController)
        {
            var playersInParty = new List<string>();

            try
            {
                var baseWindow = gameController.Game.IngameState.UIRoot.Children[BaseWindowChildIndex]?.Children[18];
                if (baseWindow != null)
                {
                    var partyList = baseWindow.Children[PartyListIndex]?.Children[0]?.Children;
                    foreach (var player in partyList)
                    {
                        if (player != null && player.ChildCount >= 3)
                        {
                            playersInParty.Add(player.Children[PlayerNameChildIndex].Text);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return playersInParty;
            }

            return playersInParty;
        }

        public static List<PartyElementWindow> GetPlayerInfoElementList(GameController gameController)
        {
            var playersInParty = new List<PartyElementWindow>();

            try
            {
                var baseWindow = gameController.IngameState.IngameUi.PartyElement;
                if (baseWindow != null)
                {
                    var partElementList = baseWindow.Children[0]?.Children[0]?.Children;
                    if (partElementList != null)
                    {
                        foreach (var partyElement in partElementList)
                        {
                            var playerName = partyElement?.Children[PlayerNameChildIndex].Text;

                            var newElement = new PartyElementWindow
                            {
                                PlayerName = playerName,
                                Element = partyElement,
                                TPButton = partyElement?.Children[partyElement.ChildCount == 4 ? TPButtonIndex : TPButtonIndex - 1]
                            };

                            // Get entity
                            foreach (var entity in gameController.EntityListWrapper.ValidEntitiesByType[EntityType.Player])
                            {
                                if (entity != null && entity.GetComponent<ExileCore.PoEMemory.Components.Player>().PlayerName == playerName)
                                {
                                    newElement.Player = entity;
                                    break; // No need to continue if entity found
                                }
                            }

                            playersInParty.Add(newElement);
                        }
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
        public Entity Player { get; set; } = null;
        public Element Element { get; set; } = new Element();
        public Element TPButton { get; set; } = new Element();

        public override string ToString()
        {
            return $"PlayerName: {PlayerName}, Data.PlayerEntity.Distance: {Player?.Distance(Entity.Player).ToString() ?? "Null"}";
        }
    }
}
