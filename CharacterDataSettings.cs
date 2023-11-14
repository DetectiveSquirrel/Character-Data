using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using SharpDX;
using System.Windows.Forms;

namespace CharacterData
{
    public class CharacterDataSettings : ISettings
    {
        // Required
        public ToggleNode Enable { get; set; } = new ToggleNode(false);

        // Health settings
        public ToggleNode HealthToggle { get; set; } = new ToggleNode(true);
        public RangeNode<int> HpPositionX { get; set; } = new RangeNode<int>(92, 0, 2000);
        public RangeNode<int> HpPositionY { get; set; } = new RangeNode<int>(952, 0, 2000);
        public ColorNode HpTextColor { get; set; } = new ColorBGRA(202, 67, 67, 255);
        public ColorNode HpBackColor { get; set; } = new ColorBGRA(0, 0, 0, 255);

        // Mana settings
        public ToggleNode ManaToggle { get; set; } = new ToggleNode(true);
        public RangeNode<int> MpPositionX { get; set; } = new RangeNode<int>(1176, 0, 2000);
        public RangeNode<int> MpPositionY { get; set; } = new RangeNode<int>(952, 0, 2000);
        public ColorNode MpTextColor { get; set; } = new ColorBGRA(47, 111, 247, 255);
        public ColorNode MpBackColor { get; set; } = new ColorBGRA(0, 0, 0, 255);

        // Resistance settings
        public ToggleNode Resistances { get; set; } = new ToggleNode(true);
        public RangeNode<int> ResistanceX { get; set; } = new RangeNode<int>(913, 0, 2000);
        public RangeNode<int> ResistanceY { get; set; } = new RangeNode<int>(980, 0, 2000);
        public RangeNode<int> ResistanceTextSize { get; set; } = new RangeNode<int>(16, 12, 36);
        public ColorNode FireResistanceColor { get; set; } = new ColorBGRA(255, 85, 85, 255);
        public ColorNode ColdResistanceColor { get; set; } = new ColorBGRA(77, 134, 255, 255);
        public ColorNode LightningResistanceColor { get; set; } = new ColorBGRA(253, 245, 75, 255);
        public ColorNode ChaosResistanceColor { get; set; } = new ColorBGRA(255, 91, 179, 255);
        public ColorNode KillsColor { get; set; } = new ColorBGRA(255, 91, 179, 255);

        // Level settings
        public ToggleNode LevelToggle { get; set; } = new ToggleNode(true);
        public ToggleNode ExperienceBar { get; set; } = new ToggleNode(true);
        public RangeNode<int> LastAreaDuration { get; set; } = new RangeNode<int>(600, 1, 1200);
        public RangeNode<int> LevelPositionX { get; set; } = new RangeNode<int>(559, 0, 2000);
        public RangeNode<int> LevelPositionY { get; set; } = new RangeNode<int>(976, 0, 2000);
        public ColorNode LevelTextColor { get; set; } = new ColorBGRA(82, 164, 0, 255);
        public ColorNode LevelBackColor { get; set; } = new ColorBGRA(0, 0, 0, 255);

        // Resolution settings
        public RangeNode<int> ResolutionLeft { get; internal set; } = new RangeNode<int>(551, 0, 2000);
        public RangeNode<int> ResolutionTop { get; internal set; } = new RangeNode<int>(957, 0, 2000);
        public RangeNode<int> ResolutionRight { get; internal set; } = new RangeNode<int>(1371, 0, 2000);
        public RangeNode<int> ResolutionBottom { get; internal set; } = new RangeNode<int>(1061, 0, 2000);

        // Other settings
        public ToggleNode Delveinfo { get; set; } = new ToggleNode(true);
        public RangeNode<int> DelveinfoX { get; set; } = new RangeNode<int>(913, 0, 2000);
        public RangeNode<int> DelveinfoY { get; set; } = new RangeNode<int>(980, 0, 2000);
        public ColorNode DelveinfoSulphiteColor { get; set; } = new ColorBGRA(255, 85, 85, 255);
        public ColorNode DelveinfoAzuriteColor { get; set; } = new ColorBGRA(255, 85, 85, 255);
        public RangeNode<int> DelveTextSpacing { get; set; } = new RangeNode<int>(16, 12, 36);

        // Additional settings
        public ToggleNode DeployedActorObjects { get; set; } = new ToggleNode(false);
        public ColorNode ActorObjectColor { get; set; } = new ColorNode(Color.White);
        public RangeNode<int> ActorObjectX { get; set; } = new RangeNode<int>(500, 0, 2560);
        public RangeNode<int> ActorObjectY { get; set; } = new RangeNode<int>(500, 0, 2560);
        public ToggleNode SoulgainPrev { get; set; } = new ToggleNode(false);
        public RangeNode<int> SoulGainPrevX { get; set; } = new RangeNode<int>(500, 0, 2560);
        public RangeNode<int> SoulGainPrevY { get; set; } = new RangeNode<int>(500, 0, 2560);
        public ToggleNode Wardloop { get; set; } = new ToggleNode(false);
        public RangeNode<int> WardLoopX { get; set; } = new RangeNode<int>(500, 0, 2560);
        public RangeNode<int> WardLoopY { get; set; } = new RangeNode<int>(500, 0, 2560);
        public ColorNode SoulGainPrevColor { get; set; } = new ColorNode(Color.Red);
        public ColorNode WardloopColor { get; set; } = new ColorNode(Color.Red);
    }
}