// Decompiled with JetBrains decompiler
// Type: CharacterData.Core.Settings
// Assembly: CharacterData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74E598EA-D86C-4665-83EF-E2CAA5899D71
// Assembly location: F:\Tools\Path of Exile Tools\Macros\plugins\Character Data\CharacterData.dll

using System.Windows.Forms;
using PoeHUD.Hud.Settings;
using PoeHUD.Plugins;
using SharpDX;

namespace CharacterData.Core
{
    public sealed class Settings : SettingsBase
    {
        public Settings()
        {
            Enable = false;
            HealthToggle = true;
            HpPositionX = new RangeNode<int>(92, 0, (int) BasePlugin.API.GameController.Window.GetWindowRectangle().Width);
            HpPositionY = new RangeNode<int>(952, 0, (int) BasePlugin.API.GameController.Window.GetWindowRectangle().Height);
            HpTextColor = new ColorBGRA(202, 67, 67, byte.MaxValue);
            HpBackColor = new ColorBGRA(0, 0, 0, byte.MaxValue);
            ManaToggle = true;
            MpPositionX = new RangeNode<int>(1176, 0, (int) BasePlugin.API.GameController.Window.GetWindowRectangle().Width);
            MpPositionY = new RangeNode<int>(952, 0, (int) BasePlugin.API.GameController.Window.GetWindowRectangle().Height);
            MpTextColor = new ColorBGRA(47, 111, 247, byte.MaxValue);
            MpBackColor = new ColorBGRA(0, 0, 0, byte.MaxValue);
            Resistances = true;
            ResistanceX = new RangeNode<int>(913, 0, (int) BasePlugin.API.GameController.Window.GetWindowRectangle().Width);
            ResistanceY = new RangeNode<int>(980, 0, (int) BasePlugin.API.GameController.Window.GetWindowRectangle().Height);
            ResistanceTextSize = 17;
            FireResistanceColor = new ColorBGRA(byte.MaxValue, 85, 85, byte.MaxValue);
            ColdResistanceColor = new ColorBGRA(77, 134, byte.MaxValue, byte.MaxValue);
            LightningResistanceColor = new ColorBGRA(253, 245, 75, byte.MaxValue);
            ChaosResistanceColor = new ColorBGRA(byte.MaxValue, 91, 179, byte.MaxValue);
            LevelToggle = true;
            ExperienceBar = true;
            LastAreaDuration = new RangeNode<int>(600, 1, 1200);
            SaveToFile = true;
            LevelPositionX = new RangeNode<int>(559, 0, (int) BasePlugin.API.GameController.Window.GetWindowRectangle().Width);
            LevelPositionY = new RangeNode<int>(976, 0, (int) BasePlugin.API.GameController.Window.GetWindowRectangle().Height);
            LevelTextColor = new ColorBGRA(82, 164, 0, byte.MaxValue);
            LevelTextSize = new RangeNode<int>(16, 12, 36);
            LevelBackColor = new ColorBGRA(0, 0, 0, byte.MaxValue);
            Resolution = true;
            ResolutionLeft = new RangeNode<int>(551, 0, (int) BasePlugin.API.GameController.Window.GetWindowRectangle().Width);
            ResolutionTop = new RangeNode<int>(957, 0, (int) BasePlugin.API.GameController.Window.GetWindowRectangle().Height);
            ResolutionRight = new RangeNode<int>(1371, 0, (int) BasePlugin.API.GameController.Window.GetWindowRectangle().Width);
            ResolutionBottom = new RangeNode<int>(1061, 0, (int) BasePlugin.API.GameController.Window.GetWindowRectangle().Height);
            ShowWindow = false;
            BackgroundSettingsTree = false;
            HpManaTree = false;
            HpTree = false;
            MpTree = false;
            PlayerResTree = false;
            InstaceInfoTree = false;

            Delveinfo = true;
            DelveinfoX = new RangeNode<int>(913, 0, (int)BasePlugin.API.GameController.Window.GetWindowRectangle().Width);
            DelveinfoY = new RangeNode<int>(980, 0, (int)BasePlugin.API.GameController.Window.GetWindowRectangle().Height);
            DelveinfoTextSize = 17;
            DelveinfoSulphiteColor = new ColorBGRA(byte.MaxValue, 85, 85, byte.MaxValue);
            DelveinfoAzuriteColor = new ColorBGRA(byte.MaxValue, 85, 85, byte.MaxValue);
            
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

        public int DelveinfoTextSize { get; set; }

        public int ResistanceTextSize { get; set; }

        public ColorNode FireResistanceColor { get; set; }

        public ColorNode ColdResistanceColor { get; set; }

        public ColorNode LightningResistanceColor { get; set; }

        public ColorNode ChaosResistanceColor { get; set; }

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
    }
}