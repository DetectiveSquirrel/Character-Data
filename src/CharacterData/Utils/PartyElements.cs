using System;
using System.Collections.Generic;
using System.Linq;
using CharacterData.Core;
using Flipping_Bot.RemoteMemoryObjects;
using PoeHUD.Controllers;
using PoeHUD.Models;
using PoeHUD.Plugins;
using PoeHUD.Poe;
using PoeHUD.Poe.Components;
using PoeHUD.Poe.Elements;
using SharpDX;
using static System.String;

namespace CharacterData.Utils
{
    public class PartyElements
    {
        public PartyElements(Main core)
        {
            Core = core;
        }

        private Main Core { get; }


        public static List<string> ListOfPlayersInParty()
        {
            var playersInParty = new List<string>();

            try
            {
                var baseWindow = new CustomIngameUIElements(GameController.Instance.Game.IngameState.IngameUi.Address).PartyList;
                if (baseWindow != null)
                {
                    var partyList = baseWindow.Children[0]?.Children[0]?.Children;
                    foreach (var player in partyList)
                    {
                        if (player != null && player.ChildCount > 3)
                            playersInParty.Add(player?.Children[0]?.AsObject<EntityLabel>()?.Text);
                    }
                }

            }
            catch (Exception e)
            {
                BasePlugin.LogError("Character: " + e.Message, 5);
            }

            return playersInParty;
        }

        public static List<PartyElementWindow> GetPlayerInfoElementList(List<EntityWrapper> entityList)
        {
            var playersInParty = new List<PartyElementWindow>();

            try
            {
                var baseWindow = new CustomIngameUIElements(GameController.Instance.Game.IngameState.IngameUi.Address).PartyList;
                if (baseWindow != null)
                {
                    var partElementList = baseWindow.Children[0]?.Children[0]?.Children;
                    if (partElementList != null)
                    {
                        foreach (var partyElement in partElementList)
                        {
                            var playerName = partyElement?.Children[0]?.AsObject<EntityLabel>()?.Text;

                            var newElement = new PartyElementWindow();
                            newElement.PlayerName = playerName;

                            // get entity
                            foreach (var entity in entityList)
                                if (entity != null && entity.GetComponent<Player>().PlayerName == playerName)
                                    newElement.Data.PlayerEntity = entity;
                            

                            //get party element
                            newElement.Element = partyElement;


                            playersInParty.Add(newElement);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                BasePlugin.LogError("CharacterData.GetPlayerInfoElementList(): " + e.Message, 5);
            }

            return playersInParty;
        }
    }

    public class PartyElementWindow
    {
        public string PlayerName { get; set; } = Empty;
        public PlayerData Data { get; set; } = new PlayerData();
        public Element Element { get; set; } = new Element();

        public override string ToString()
        {
            return $"PlayerName: {PlayerName}, Data.PlayerEntity.Distance: {Data.PlayerEntity?.InternalEntity.Distance.ToString() ?? "Null"}";
        }
    }

    public class PlayerData
    {
        public EntityWrapper PlayerEntity { get; set; } = null;
    }
}