using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using Newtonsoft.Json;
using SharpDX;
using System.Windows.Forms;

namespace CharacterData
{
    public class CharacterDataSettings : ISettings
    {
        public ToggleNode Enable { get; set; } = new(false);

        [Menu("Health + Mana Font", 1, CollapsedByDefault = false)]
        [JsonIgnore]
        public EmptyNode Empty1 { get; set; }
        [Menu(null, parentIndex = 1)]
        public TextNode GlobeFont { get; set; } = string.Empty;
        [Menu(null, parentIndex = 1)]
        public RangeNode<float> GlobeFontScale { get; set; } = new(1, 1, 30);

        [Menu("Health settings", 2, CollapsedByDefault = false)]
        [JsonIgnore]
        public EmptyNode Empty2 { get; set; }
        [Menu(null, parentIndex = 2)]
        public ToggleNode HealthToggle { get; set; } = new(true);
        [Menu(null, parentIndex = 2)]
        public ColorNode HpTextColor { get; set; } = new ColorBGRA(202, 67, 67, 255);
        [Menu(null, parentIndex = 2)]
        public ColorNode HpBackColor { get; set; } = new ColorBGRA(0, 0, 0, 255);

        [Menu("Mana settings", 3, CollapsedByDefault = false)]
        [JsonIgnore]
        public EmptyNode Empty3 { get; set; }
        [Menu(null, parentIndex = 3)]
        public ToggleNode ManaToggle { get; set; } = new(true);
        [Menu(null, parentIndex = 3)]
        public ColorNode MpTextColor { get; set; } = new ColorBGRA(47, 111, 247, 255);
        [Menu(null, parentIndex = 3)]
        public ColorNode MpBackColor { get; set; } = new ColorBGRA(0, 0, 0, 255);

        [Menu("Resistance settings", 4, CollapsedByDefault = false)]
        [JsonIgnore]
        public EmptyNode Empty4 { get; set; }
        [Menu(null, parentIndex = 4)]
        public ToggleNode Resistances { get; set; } = new(true);
        [Menu(null, parentIndex = 4)]
        public RangeNode<int> ResistanceX { get; set; } = new(913, 0, 2000);
        [Menu(null, parentIndex = 4)]
        public RangeNode<int> ResistanceY { get; set; } = new(980, 0, 2000);
        [Menu(null, parentIndex = 4)]
        public RangeNode<int> ResistanceTextSize { get; set; } = new(16, 12, 36);
        [Menu(null, parentIndex = 4)]
        public ColorNode FireResistanceColor { get; set; } = new ColorBGRA(255, 85, 85, 255);
        [Menu(null, parentIndex = 4)]
        public ColorNode ColdResistanceColor { get; set; } = new ColorBGRA(77, 134, 255, 255);
        [Menu(null, parentIndex = 4)]
        public ColorNode LightningResistanceColor { get; set; } = new ColorBGRA(253, 245, 75, 255);
        [Menu(null, parentIndex = 4)]
        public ColorNode ChaosResistanceColor { get; set; } = new ColorBGRA(255, 91, 179, 255);
        [Menu(null, parentIndex = 4)]
        public ColorNode KillsColor { get; set; } = new ColorBGRA(255, 91, 179, 255);

        [Menu("Level settings", 5, CollapsedByDefault = false)]
        [JsonIgnore]
        public EmptyNode Empty5 { get; set; }
        [Menu(null, parentIndex = 5)]
        public ToggleNode LevelToggle { get; set; } = new(true);
        [Menu(null, parentIndex = 5)]
        public ToggleNode ExperienceBar { get; set; } = new(true);
        [Menu(null, parentIndex = 5)]
        public RangeNode<int> LastAreaDuration { get; set; } = new(600, 1, 1200);
        [Menu(null, parentIndex = 5)]
        public RangeNode<int> LevelPositionX { get; set; } = new(559, 0, 2000);
        [Menu(null, parentIndex = 5)]
        public RangeNode<int> LevelPositionY { get; set; } = new(976, 0, 2000);
        [Menu(null, parentIndex = 5)]
        public ColorNode LevelTextColor { get; set; } = new ColorBGRA(82, 164, 0, 255);
        [Menu(null, parentIndex = 5)]
        public ColorNode LevelBackColor { get; set; } = new ColorBGRA(0, 0, 0, 255);

        [Menu("Background settings", 6, CollapsedByDefault = false)]
        [JsonIgnore]
        public EmptyNode Empty6 { get; set; }

        [Menu(null, parentIndex = 6)]
        public RangeNode<int> ResolutionLeft { get; internal set; } = new(551, 0, 2000);
        [Menu(null, parentIndex = 6)]
        public RangeNode<int> ResolutionTop { get; internal set; } = new(957, 0, 2000);
        [Menu(null, parentIndex = 6)]
        public RangeNode<int> ResolutionRight { get; internal set; } = new(1371, 0, 2000);
        [Menu(null, parentIndex = 6)]
        public RangeNode<int> ResolutionBottom { get; internal set; } = new(1061, 0, 2000);

        [Menu("Delve settings", 7, CollapsedByDefault = false)]
        [JsonIgnore]
        public EmptyNode Empty7 { get; set; }
        [Menu(null, parentIndex = 7)]
        public ToggleNode Delveinfo { get; set; } = new(true);
        [Menu(null, parentIndex = 7)]
        public RangeNode<int> DelveinfoX { get; set; } = new(913, 0, 2000);
        [Menu(null, parentIndex = 7)]
        public RangeNode<int> DelveinfoY { get; set; } = new(980, 0, 2000);
        [Menu(null, parentIndex = 7)]
        public ColorNode DelveinfoSulphiteColor { get; set; } = new ColorBGRA(255, 85, 85, 255);
        [Menu(null, parentIndex = 7)]
        public ColorNode DelveinfoAzuriteColor { get; set; } = new ColorBGRA(255, 85, 85, 255);
        [Menu(null, parentIndex = 7)]
        public RangeNode<int> DelveTextSpacing { get; set; } = new(16, 12, 36);

        [Menu("Deployed Actor / Wardloop settings", 8, CollapsedByDefault = false)]
        [JsonIgnore]
        public EmptyNode Empty8 { get; set; }
        [Menu(null, parentIndex = 8)]
        public ToggleNode DeployedActorObjects { get; set; } = new(false);
        [Menu(null, parentIndex = 8)]
        public ColorNode ActorObjectColor { get; set; } = new(Color.White);
        [Menu(null, parentIndex = 8)]
        public RangeNode<int> ActorObjectX { get; set; } = new(500, 0, 2560);
        [Menu(null, parentIndex = 8)]
        public RangeNode<int> ActorObjectY { get; set; } = new(500, 0, 2560);
        [Menu(null, parentIndex = 8)]
        public ToggleNode SoulgainPrev { get; set; } = new(false);
        [Menu(null, parentIndex = 8)]
        public RangeNode<int> SoulGainPrevX { get; set; } = new(500, 0, 2560);
        [Menu(null, parentIndex = 8)]
        public RangeNode<int> SoulGainPrevY { get; set; } = new(500, 0, 2560);
        [Menu(null, parentIndex = 8)]
        public ToggleNode Wardloop { get; set; } = new(false);
        [Menu(null, parentIndex = 8)]
        public RangeNode<int> WardLoopX { get; set; } = new(500, 0, 2560);
        [Menu(null, parentIndex = 8)]
        public RangeNode<int> WardLoopY { get; set; } = new(500, 0, 2560);
        [Menu(null, parentIndex = 8)]
        public ColorNode SoulGainPrevColor { get; set; } = new(Color.Red);
        [Menu(null, parentIndex = 8)]
        public ColorNode WardloopColor { get; set; } = new(Color.Red);
    }
}