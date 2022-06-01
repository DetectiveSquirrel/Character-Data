// Decompiled with JetBrains decompiler
// Type: CharacterData.Core.Settings
// Assembly: CharacterData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74E598EA-D86C-4665-83EF-E2CAA5899D71
// Assembly location: F:\Tools\Path of Exile Tools\Macros\plugins\Character Data\CharacterData.dll

using System.Diagnostics.Contracts;
using System.Windows.Forms;
using ExileCore;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using SharpDX;

namespace CharacterData.Core
{
    public class Settings : ISettings
    {
        public Settings()
        {
            HealthToggle = new ToggleNode(true);
            HpPositionX = new RangeNode<int>(92, 0, 2000);
            HpPositionY = new RangeNode<int>(952, 0, 2000);
            HpTextColor = new ColorBGRA(202, 67, 67, byte.MaxValue);
            HpBackColor = new ColorBGRA(0, 0, 0, byte.MaxValue);
            ManaToggle = new ToggleNode(true);
            MpPositionX = new RangeNode<int>(1176, 0, 2000);
            MpPositionY = new RangeNode<int>(952, 0, 2000);
            MpTextColor = new ColorBGRA(47, 111, 247, byte.MaxValue);
            MpBackColor = new ColorBGRA(0, 0, 0, byte.MaxValue);
            Resistances = new ToggleNode(true);
            ResistanceX = new RangeNode<int>(913, 0, 2000);
            ResistanceY = new RangeNode<int>(980, 0, 2000);
            ResistanceTextSize = new RangeNode<int>(16, 12, 36);
            FireResistanceColor = new ColorBGRA(byte.MaxValue, 85, 85, byte.MaxValue);
            ColdResistanceColor = new ColorBGRA(77, 134, byte.MaxValue, byte.MaxValue);
            LightningResistanceColor = new ColorBGRA(253, 245, 75, byte.MaxValue);
            ChaosResistanceColor = new ColorBGRA(byte.MaxValue, 91, 179, byte.MaxValue);
            KillsColor = new ColorBGRA(byte.MaxValue, 91, 179, byte.MaxValue);
            LevelToggle = new ToggleNode(true);
            ExperienceBar = new ToggleNode(true);
            LastAreaDuration = new RangeNode<int>(600, 1, 1200);
            SaveToFile = new ToggleNode(true);
            LevelPositionX = new RangeNode<int>(559, 0, 2000);
            LevelPositionY = new RangeNode<int>(976, 0, 2000);
            LevelTextColor = new ColorBGRA(82, 164, 0, byte.MaxValue);
            LevelTextSize = new RangeNode<int>(16, 12, 36);
            LevelBackColor = new ColorBGRA(0, 0, 0, byte.MaxValue);
            Resolution = new ToggleNode(true);
            ResolutionLeft = new RangeNode<int>(551, 0, 2000);
            ResolutionTop = new RangeNode<int>(957, 0, 2000);
            ResolutionRight = new RangeNode<int>(1371, 0, 2000);
            ResolutionBottom = new RangeNode<int>(1061, 0, 2000);
            ShowWindow = new ToggleNode(true);
            BackgroundSettingsTree = false;
            HpManaTree = false;
            HpTree = false;
            MpTree = false;
            PlayerResTree = false;
            InstaceInfoTree = false;

            Delveinfo = new ToggleNode(true);
            DelveinfoX = new RangeNode<int>(913, 0, 2000);
            DelveinfoY = new RangeNode<int>(980, 0, 2000);
            DelveTextSpacing = new RangeNode<int>(16, 12, 36);
            DelveinfoSulphiteColor = new ColorBGRA(byte.MaxValue, 85, 85, byte.MaxValue);
            DelveinfoAzuriteColor = new ColorBGRA(byte.MaxValue, 85, 85, byte.MaxValue);

            ScourgeJuiceColor = new ColorBGRA(byte.MaxValue, 0, 0, byte.MaxValue);

        }

        public ToggleNode HealthToggle { get; set; }

        public RangeNode<int> HpPositionX { get; set; }

        public RangeNode<int> HpPositionY { get; set; }

        public ColorNode HpTextColor { get; set; }

        public ColorNode HpBackColor { get; set; }

        public ToggleNode ManaToggle { get; set; }

        public RangeNode<int> MpPositionX { get; set; }

        public RangeNode<int> MpPositionY { get; set; }

        public ColorNode MpTextColor { get; set; }

        public ColorNode MpBackColor { get; set; }

        public ToggleNode Resistances { get; set; }

        public RangeNode<int> ResistanceX { get; set; }

        public RangeNode<int> ResistanceY { get; set; }

        public ToggleNode Delveinfo { get; set; }

        public RangeNode<int> DelveinfoX { get; set; }

        public RangeNode<int> DelveinfoY { get; set; }

        public ColorNode DelveinfoSulphiteColor { get; set; }
        public ColorNode DelveinfoAzuriteColor { get; set; }
        public ColorNode ScourgeJuiceColor { get; set; }

        public RangeNode<int> DelveTextSpacing { get; set; }

        public RangeNode<int> ResistanceTextSize { get; set; }

        public ColorNode FireResistanceColor { get; set; }

        public ColorNode ColdResistanceColor { get; set; }

        public ColorNode LightningResistanceColor { get; set; }

        public ColorNode ChaosResistanceColor { get; set; }

        public ColorNode KillsColor { get; set; }

        public ToggleNode LevelToggle { get; set; }

        public ToggleNode ExperienceBar { get; set; }

        public RangeNode<int> LastAreaDuration { get; set; }

        public ToggleNode SaveToFile { get; set; }

        public RangeNode<int> LevelPositionX { get; set; }

        public RangeNode<int> LevelPositionY { get; set; }

        public ColorNode LevelTextColor { get; set; }

        public RangeNode<int> LevelTextSize { get; set; }

        public ColorNode LevelBackColor { get; set; }

        public ToggleNode Resolution { get; set; }

        public RangeNode<int> ResolutionLeft { get; internal set; }

        public RangeNode<int> ResolutionTop { get; internal set; }

        public RangeNode<int> ResolutionRight { get; internal set; }

        public RangeNode<int> ResolutionBottom { get; internal set; }

        public ToggleNode ShowWindow { get; set; }

        public bool BackgroundSettingsTree { get; set; }

        public bool HpManaTree { get; set; }

        public bool HpTree { get; set; }

        public bool MpTree { get; set; }

        public bool PlayerResTree { get; set; }

        public bool InstaceInfoTree { get; set; }
        public ToggleNode DeployedActorObjects { get; set; } = new ToggleNode(false);

        public ColorNode ActorObjectColor { get; set; } = new ColorNode(Color.White);

        public RangeNode<int> ActorObjectX { get; set; } = new RangeNode<int>(500, 0, 2560);

        public RangeNode<int> ActorObjectY { get; set; } = new RangeNode<int>(500, 0, 2560);

        public RangeNode<int> SoulGainPrevX { get; set; } = new RangeNode<int>(500, 0, 2560);

        public RangeNode<int> SoulGainPrevY { get; set; } = new RangeNode<int>(500, 0, 2560);

        public ColorNode SoulGainPrevColor { get; set; } = new ColorNode(Color.Red);
        public HotkeyNode StashToggleHotkey { get; set; } = new HotkeyNode(Keys.None);
        public HotkeyNode InventoryHotkey { get; set; } = new HotkeyNode(Keys.None);

        public ToggleNode Enable { get; set; } = new ToggleNode(true);

        public RangeNode<int> PartyElement { get; set; } = new RangeNode<int>(20, 0, 300);
    }
}