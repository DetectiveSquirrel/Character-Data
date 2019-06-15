// Decompiled with JetBrains decompiler
// Type: CharacterData.Utils.LocalPlayer
// Assembly: CharacterData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74E598EA-D86C-4665-83EF-E2CAA5899D71
// Assembly location: F:\Tools\Path of Exile Tools\Macros\plugins\Character Data\CharacterData.dll

using PoeHUD.Framework.Helpers;
using PoeHUD.Models;
using PoeHUD.Plugins;
using PoeHUD.Poe.Components;
using SharpDX;

namespace CharacterData.Utils
{
    public class LocalPlayer
    {
        public static EntityWrapper Entity => BasePlugin.API.GameController.Player;

        public static long Experience => Entity.GetComponent<Player>().XP;

        public static string Name => Entity.GetComponent<Player>().PlayerName;

        public static int Level => Entity.GetComponent<Player>().Level;

        public static Stats Stat => Entity.GetComponent<Stats>();

        public static Life Health => Entity.GetComponent<Life>();

        public static AreaInstance Area => BasePlugin.API.GameController.Area.CurrentArea;

        public static uint AreaHash => BasePlugin.API.GameController.Game.IngameState.Data.CurrentAreaHash;

        public static Vector2 PlayerToScreen => BasePlugin.API.GameController.Game.IngameState.Camera.WorldToScreen(Entity.Pos.Translate(0.0f, 0.0f, -170f), Entity);

        public static bool HasBuff(string buffName) => Entity.GetComponent<Life>().HasBuff(buffName);
    }
}