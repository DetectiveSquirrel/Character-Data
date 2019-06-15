// Decompiled with JetBrains decompiler
// Type: CharacterData.Utils.ImGuiExtension
// Assembly: CharacterData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74E598EA-D86C-4665-83EF-E2CAA5899D71
// Assembly location: F:\Tools\Path of Exile Tools\Macros\plugins\Character Data\CharacterData.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ImGuiNET;
using PoeHUD.Framework;
using PoeHUD.Hud.Settings;
using PoeHUD.Plugins;
using SharpDX;
using Vector4 = System.Numerics.Vector4;

namespace CharacterData.Utils
{
  public class ImGuiExtension
  {
    public static Vector4 CenterWindow(int width, int height)
    {
      Vector2 center = BasePlugin.API.GameController.Window.GetWindowRectangle().Center;
      return new Vector4(width + center.X - width / 2, height + center.Y - height / 2, width, height);
    }

    public static bool BeginWindow(string title, int x, int y, int width, int height, bool autoResize = false)
    {
      ImGui.SetNextWindowPos(new System.Numerics.Vector2(width + x, height + y), Condition.Appearing, new System.Numerics.Vector2(1f, 1f));
      ImGui.SetNextWindowSize(new System.Numerics.Vector2(width, height), Condition.Appearing);
      return ImGui.BeginWindow(title, autoResize ? WindowFlags.AlwaysAutoResize : WindowFlags.Default);
    }

    public static bool BeginWindowCenter(string title, int width, int height, bool autoResize = false)
    {
      Vector4 vector4 = CenterWindow(width, height);
      ImGui.SetNextWindowPos(new System.Numerics.Vector2(vector4.X, vector4.Y), Condition.Appearing, new System.Numerics.Vector2(1f, 1f));
      ImGui.SetNextWindowSize(new System.Numerics.Vector2(vector4.Z, vector4.W), Condition.Appearing);
      return ImGui.BeginWindow(title, autoResize ? WindowFlags.AlwaysAutoResize : WindowFlags.Default);
    }

    public static int IntSlider(string labelString, int value, int minValue, int maxValue)
    {
      int num = value;
      ImGui.SliderInt(labelString, ref num, minValue, maxValue, "%.00f");
      return num;
    }

    public static int IntSlider(string labelString, string sliderString, int value, int minValue, int maxValue)
    {
      int num = value;
      ImGui.SliderInt(labelString, ref num, minValue, maxValue, sliderString);
      return num;
    }

    public static int IntSlider(string labelString, RangeNode<int> setting)
    {
      int num = setting.Value;
      ImGui.SliderInt(labelString, ref num, setting.Min, setting.Max, "%.00f");
      return num;
    }

    public static int IntSlider(string labelString, string sliderString, RangeNode<int> setting)
    {
      int num = setting.Value;
      ImGui.SliderInt(labelString, ref num, setting.Min, setting.Max, sliderString);
      return num;
    }

    public static float FloatSlider(string labelString, float value, float minValue, float maxValue)
    {
      float num = value;
      ImGui.SliderFloat(labelString, ref num, minValue, maxValue, "%.00f", 1f);
      return num;
    }

    public static float FloatSlider(string labelString, float value, float minValue, float maxValue, float power)
    {
      float num = value;
      ImGui.SliderFloat(labelString, ref num, minValue, maxValue, "%.00f", power);
      return num;
    }

    public static float FloatSlider(string labelString, string sliderString, float value, float minValue, float maxValue)
    {
      float num = value;
      ImGui.SliderFloat(labelString, ref num, minValue, maxValue, sliderString, 1f);
      return num;
    }

    public static float FloatSlider(string labelString, string sliderString, float value, float minValue, float maxValue, float power)
    {
      float num = value;
      ImGui.SliderFloat(labelString, ref num, minValue, maxValue, sliderString, power);
      return num;
    }

    public static float FloatSlider(string labelString, RangeNode<float> setting)
    {
      float num = setting.Value;
      ImGui.SliderFloat(labelString, ref num, setting.Min, setting.Max, "%.00f", 1f);
      return num;
    }

    public static float FloatSlider(string labelString, RangeNode<float> setting, float power)
    {
      float num = setting.Value;
      ImGui.SliderFloat(labelString, ref num, setting.Min, setting.Max, "%.00f", power);
      return num;
    }

    public static float FloatSlider(string labelString, string sliderString, RangeNode<float> setting)
    {
      float num = setting.Value;
      ImGui.SliderFloat(labelString, ref num, setting.Min, setting.Max, sliderString, 1f);
      return num;
    }

    public static float FloatSlider(string labelString, string sliderString, RangeNode<float> setting, float power)
    {
      float num = setting.Value;
      ImGui.SliderFloat(labelString, ref num, setting.Min, setting.Max, sliderString, power);
      return num;
    }

    public static bool Checkbox(string labelString, bool boolValue)
    {
      ImGui.Checkbox(labelString, ref boolValue);
      return boolValue;
    }

    public static bool Checkbox(string labelString, bool boolValue, out bool outBool)
    {
      ImGui.Checkbox(labelString, ref boolValue);
      outBool = boolValue;
      return boolValue;
    }

    public static IEnumerable<Keys> KeyCodes()
    {
      return Enum.GetValues(typeof (Keys)).Cast<Keys>();
    }

    public static Keys HotkeySelector(string buttonName, Keys currentKey)
    {
      if (ImGui.Button(string.Format("{0}: {1} ", buttonName, currentKey)))
        ImGui.OpenPopup(buttonName);
      if (ImGui.BeginPopupModal(buttonName, WindowFlags.NoTitleBar | WindowFlags.NoResize | WindowFlags.NoCollapse))
      {
        ImGui.Text(string.Format("Press a key to set as {0}", buttonName));
        foreach (Keys keyCode in KeyCodes())
        {
          if (WinApi.IsKeyDown(keyCode))
          {
            if (keyCode != Keys.Escape)
            {
              if (keyCode != Keys.RButton)
              {
                if (keyCode != Keys.LButton)
                {
                  ImGui.CloseCurrentPopup();
                  ImGui.EndPopup();
                  return keyCode;
                }
              }
            }
            break;
          }
        }
        ImGui.EndPopup();
      }
      return currentKey;
    }

    public static Keys HotkeySelector(string buttonName, string popupTitle, Keys currentKey)
    {
      if (ImGui.Button(string.Format("{0}: {1} ", buttonName, currentKey)))
        ImGui.OpenPopup(popupTitle);
      if (ImGui.BeginPopupModal(popupTitle, WindowFlags.NoTitleBar | WindowFlags.NoResize | WindowFlags.NoCollapse))
      {
        ImGui.Text(string.Format("Press a key to set as {0}", buttonName));
        foreach (Keys keyCode in KeyCodes())
        {
          if (WinApi.IsKeyDown(keyCode))
          {
            if (keyCode != Keys.Escape)
            {
              if (keyCode != Keys.RButton)
              {
                if (keyCode != Keys.LButton)
                {
                  ImGui.CloseCurrentPopup();
                  ImGui.EndPopup();
                  return keyCode;
                }
              }
            }
            break;
          }
        }
        ImGui.EndPopup();
      }
      return currentKey;
    }

    public static Color ColorPicker(string labelName, Color inputColor)
    {
      SharpDX.Vector4 vector4 = inputColor.ToVector4();
      Vector4 color = new Vector4(vector4.X, vector4.Y, vector4.Z, vector4.W);
      if (ImGui.ColorEdit4(labelName, ref color, ColorEditFlags.AlphaBar))
        return new Color(color.X, color.Y, color.Z, color.W);
      return inputColor;
    }

    public static string ComboBox(string sideLabel, string currentSelectedItem, List<string> objectList, ComboFlags comboFlags = ComboFlags.HeightRegular)
    {
      if (ImGui.BeginCombo(sideLabel, currentSelectedItem, comboFlags))
      {
        string str = currentSelectedItem;
        for (int index = 0; index < objectList.Count; ++index)
        {
          bool isSelected = str == objectList[index];
          if (ImGui.Selectable(objectList[index], isSelected))
            return objectList[index];
          if (isSelected)
            ImGui.SetItemDefaultFocus();
        }
        ImGui.EndCombo();
      }
      return currentSelectedItem;
    }
  }
}
