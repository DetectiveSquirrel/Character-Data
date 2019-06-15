// Decompiled with JetBrains decompiler
// Type: CharacterData.Utils.ContentBox
// Assembly: CharacterData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74E598EA-D86C-4665-83EF-E2CAA5899D71
// Assembly location: F:\Tools\Path of Exile Tools\Macros\plugins\Character Data\CharacterData.dll

using PoeHUD.Hud.UI;
using PoeHUD.Plugins;
using SharpDX;
using SharpDX.Direct3D9;

namespace CharacterData.Utils
{
  internal class ContentBox
  {
    public Color BoxColor;
    public Vector2 BoxPositon;
    public Size2 BoxSize;
    public Graphics Graphics;
    public Color TextColor;
    public int TextSize;
    public string TextToDraw;

    public ContentBox()
    {
      Graphics = BasePlugin.API.Graphics;
      TextColor = Color.White;
    }

    public ContentBox(Color textColor)
    {
      Graphics = BasePlugin.API.Graphics;
      TextColor = textColor;
    }

    public void Draw()
    {
      BoxSize = Graphics.MeasureText(TextToDraw, TextSize, FontDrawFlags.Left);
      RectangleF rectangle = new RectangleF(BoxPositon.X - BoxSize.Width / 2, BoxPositon.Y, BoxSize.Width + 2, BoxSize.Height + 4);
      Graphics.DrawBox(rectangle, BoxColor);
      Graphics.DrawFrame(rectangle, 1f, TextColor);
      Graphics.DrawText(TextToDraw, TextSize, BoxPositon, TextColor, FontDrawFlags.Center);
    }
  }
}
