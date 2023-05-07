using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Components;
using ExileCore.PoEMemory.MemoryObjects;
using System;
using System.Collections.Generic;
using static System.String;

namespace CharacterData.Utils
{
    public class PartyElements
    {
        public PartyElements(Core.Core core)
        {
            Core = core;
        }

        private Core.Core Core { get; }

        public static List<string> ListOfPlayersInParty()
        {
            var playersInParty = new List<string>();

            try
            {
                var baseWindow = CharacterData.Core.Core.MainPlugin.GameController.Game.IngameState.UIRoot.Children[1].Children[18];
                if (baseWindow != null)
                {
                    var partyList = baseWindow.Children[0]?.Children[0]?.Children;
                    foreach (var player in partyList)
                    {
                        if (player != null && player.ChildCount >= 3)
                        {
                            playersInParty.Add(player?.Children[0].Text);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //CharacterData.Core.Core.MainPlugin.LogError("Character: " + e.StackTrace, 5);
            }

            return playersInParty;
        }

        public static List<PartyElementWindow> GetPlayerInfoElementList(List<Entity> entityList, int child)
        {
            var playersInParty = new List<PartyElementWindow>();

            try
            {
                var baseWindow = CharacterData.Core.Core.MainPlugin.GameController.IngameState.IngameUi.PartyElement;
                if (baseWindow != null)
                {
                    var partElementList = baseWindow.Children[0].Children[0].Children;
                    if (partElementList != null)
                    {
                        foreach (var partyElement in partElementList)
                        {
                            var playerName = partyElement?.Children[0].Text;

                            var newElement = new PartyElementWindow
                            {
                                PlayerName = playerName
                            };

                            // get entity
                            foreach (var entity in entityList)
                            {
                                if (entity != null && entity.GetComponent<Player>().PlayerName == playerName)
                                {
                                    newElement.Data.PlayerEntity = entity;
                                }
                            }

                            //get party element
                            newElement.Element = partyElement;

                            //party element swirly tp thingo
                            newElement.TPButton = partyElement?.Children[partyElement?.ChildCount == 4 ? 3 : 2];

                            playersInParty.Add(newElement);
                        }
                    }
                }
            }
            catch (Exception)
            {
                //CharacterData.Core.Core.MainPlugin.LogError("Character: " + e.StackTrace, 5);
            }

            return playersInParty;
        }
    }

    public class PartyElementWindow
    {
        public string PlayerName { get; set; } = Empty;
        public PlayerData Data { get; set; } = new PlayerData();
        public Element Element { get; set; } = new Element();
        public Element TPButton { get; set; } = new Element();

        public override string ToString()
        {
            return $"PlayerName: {PlayerName}, Data.PlayerEntity.Distance: {Data.PlayerEntity.Distance(Entity.Player).ToString() ?? "Null"}";
        }
    }

    public class PlayerData
    {
        public Entity PlayerEntity { get; set; } = null;
    }
}