
// Type: LD57.InputManager
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

#nullable disable
namespace LD57
{
  internal static class InputManager
  {
    private static KeyboardState m_keyboard = new KeyboardState();
    private static KeyboardState m_keyboardPrev = new KeyboardState();
    private static GamePadState m_gamepad = new GamePadState();
    private static GamePadState m_gamepadPrev = new GamePadState();
    private static Dictionary<string, Keys> m_keyboardInputs = new Dictionary<string, Keys>();
    private static Dictionary<string, Buttons> m_gamepadInputs = new Dictionary<string, Buttons>();
    private static List<string> m_inputNames = new List<string>();
    private static HashSet<string> m_pressed = new HashSet<string>();
    private static HashSet<string> m_released = new HashSet<string>();
    private static string m_rebind = (string) null;

    public static void SetInput(string input, Keys key, Buttons button)
    {
      if (!InputManager.m_keyboardInputs.ContainsKey(input))
        InputManager.m_inputNames.Add(input);
      InputManager.m_keyboardInputs[input] = key;
      InputManager.m_gamepadInputs[input] = button;
    }

    public static void UpdateInput(KeyboardState k, GamePadState g)
    {
      InputManager.m_pressed.Clear();
      InputManager.m_released.Clear();
      if (InputManager.m_rebind == null)
      {
        foreach (KeyValuePair<string, Keys> keyboardInput in InputManager.m_keyboardInputs)
        {
          if (!InputManager.m_keyboard.IsKeyDown(keyboardInput.Value) && !InputManager.m_gamepad.IsButtonDown(InputManager.m_gamepadInputs[keyboardInput.Key]) && (k.IsKeyDown(keyboardInput.Value) || g.IsButtonDown(InputManager.m_gamepadInputs[keyboardInput.Key])))
            InputManager.m_pressed.Add(keyboardInput.Key);
          if ((InputManager.m_keyboard.IsKeyDown(keyboardInput.Value) || InputManager.m_gamepad.IsButtonDown(InputManager.m_gamepadInputs[keyboardInput.Key])) && !k.IsKeyDown(keyboardInput.Value) && !g.IsButtonDown(InputManager.m_gamepadInputs[keyboardInput.Key]))
            InputManager.m_released.Add(keyboardInput.Key);
        }
      }
      else
      {
        foreach (Keys pressedKey in k.GetPressedKeys())
        {
          bool flag = true;
          foreach (KeyValuePair<string, Keys> keyboardInput in InputManager.m_keyboardInputs)
          {
            if (!(keyboardInput.Key == InputManager.m_rebind) && keyboardInput.Value == pressedKey)
            {
              flag = false;
              break;
            }
          }
          if (flag && InputManager.m_keyboard.IsKeyUp(pressedKey))
          {
            InputManager.m_keyboardInputs[InputManager.m_rebind] = pressedKey;
            InputManager.m_rebind = (string) null;
            break;
          }
        }
      }
      InputManager.m_keyboardPrev = InputManager.m_keyboard;
      InputManager.m_gamepadPrev = InputManager.m_gamepad;
      InputManager.m_keyboard = k;
      InputManager.m_gamepad = g;
    }

    public static bool IsHeld(string input)
    {
      if (!InputManager.m_keyboardInputs.ContainsKey(input))
        return false;
      return InputManager.m_keyboard.IsKeyDown(InputManager.m_keyboardInputs[input]) || InputManager.m_gamepad.IsButtonDown(InputManager.m_gamepadInputs[input]);
    }

    public static bool IsHeld(Keys key) => InputManager.m_keyboard.IsKeyDown(key);

    public static bool IsPressed(string input) => InputManager.m_pressed.Contains(input);

    public static bool IsPressed(Keys key)
    {
      return InputManager.m_keyboard.IsKeyDown(key) && !InputManager.m_keyboardPrev.IsKeyDown(key);
    }

    public static bool IsReleased(string input) => InputManager.m_released.Contains(input);

    public static bool IsReleased(Keys key)
    {
      return !InputManager.m_keyboard.IsKeyDown(key) && InputManager.m_keyboardPrev.IsKeyDown(key);
    }

    public static List<string> GetInputList()
    {
      List<string> inputList = new List<string>();
      for (int index = 0; index < InputManager.m_inputNames.Count; ++index)
        inputList.Add(InputManager.m_inputNames[index]);
      return inputList;
    }

    public static List<Keys> GetKeysList()
    {
      List<Keys> keysList = new List<Keys>();
      for (int index = 0; index < InputManager.m_inputNames.Count; ++index)
        keysList.Add(InputManager.m_keyboardInputs[InputManager.m_inputNames[index]]);
      return keysList;
    }

    public static Keys GetInputKey(string input) => InputManager.m_keyboardInputs[input];

    public static void SetRebind(string input) => InputManager.m_rebind = input;

    public static bool IsRebinding() => InputManager.m_rebind != null;

    public static Dictionary<string, Keys> GetInputDictionary()
    {
      Dictionary<string, Keys> inputDictionary = new Dictionary<string, Keys>();
      foreach (KeyValuePair<string, Keys> keyboardInput in InputManager.m_keyboardInputs)
        inputDictionary[keyboardInput.Key] = keyboardInput.Value;
      return inputDictionary;
    }
  }
}
