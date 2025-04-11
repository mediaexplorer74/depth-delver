
// Type: LD57.InputManager
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Assembly location: C:\Users\Admin\Desktop\RE\DepthDelver\LD57.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
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

    // Touch-related fields
    private static TouchCollection m_touchCollection = new TouchCollection();
    private static List<TouchLocation> m_touchesBegan = new List<TouchLocation>();
    private static List<TouchLocation> m_touchesEnded = new List<TouchLocation>();
    private static List<TouchLocation> m_touchesMoved = new List<TouchLocation>();

    // Swipe detection fields
    private static Vector2 m_swipeStart = Vector2.Zero;
    private static Vector2 m_swipeEnd = Vector2.Zero;
    private static DateTime m_swipeStartTime;
    private static bool m_isSwipeDetected = false;

    // Swipe thresholds
    private const float MinSwipeDistance = 50f; // Minimum distance in pixels
    private const float MaxSwipeDuration = 20f;//0.5f; // Maximum duration in seconds


    public static void SetInput(string input, Keys key, Buttons button)
    {
      if (!InputManager.m_keyboardInputs.ContainsKey(input))
        InputManager.m_inputNames.Add(input);
      InputManager.m_keyboardInputs[input] = key;
      InputManager.m_gamepadInputs[input] = button;
    }

    public static void UpdateInput(KeyboardState k, GamePadState g, TouchCollection t)
    {
      InputManager.m_pressed.Clear();
      InputManager.m_released.Clear();

      m_touchesBegan.Clear();
      m_touchesEnded.Clear();
      m_touchesMoved.Clear();
      m_isSwipeDetected = false;

      // Update touch input
      m_touchCollection = t;
      foreach (TouchLocation touch in t)
      {
         switch (touch.State)
         {
             case TouchLocationState.Pressed:
                  m_touchesBegan.Add(touch);
                  m_swipeStart = touch.Position;
                  m_swipeStartTime = DateTime.Now;
                //Experimental
                //m_swipeEnd = touch.Position;
                //DetectSwipe();
                break;

             case TouchLocationState.Released:
                  m_touchesEnded.Add(touch);
                  m_swipeEnd = touch.Position;
                  DetectSwipe();
             break;

             case TouchLocationState.Moved:
                  m_touchesMoved.Add(touch);
                  m_swipeEnd = touch.Position;
                  DetectSwipe();
             break;
         }
      }

      if (InputManager.m_rebind == null)
      {
        foreach (KeyValuePair<string, Keys> keyboardInput in InputManager.m_keyboardInputs)
        {
          if (!InputManager.m_keyboard.IsKeyDown(keyboardInput.Value) 
              && !InputManager.m_gamepad.IsButtonDown(InputManager.m_gamepadInputs[keyboardInput.Key]) 
              && (k.IsKeyDown(keyboardInput.Value) 
              || g.IsButtonDown(InputManager.m_gamepadInputs[keyboardInput.Key])))
            InputManager.m_pressed.Add(keyboardInput.Key);

          if ((InputManager.m_keyboard.IsKeyDown(keyboardInput.Value) 
           || InputManager.m_gamepad.IsButtonDown(InputManager.m_gamepadInputs[keyboardInput.Key]))
           && !k.IsKeyDown(keyboardInput.Value) && 
           !g.IsButtonDown(InputManager.m_gamepadInputs[keyboardInput.Key]))
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
      return InputManager.m_keyboard.IsKeyDown(InputManager.m_keyboardInputs[input]) 
         || InputManager.m_gamepad.IsButtonDown(InputManager.m_gamepadInputs[input]);
    }

    /*
    public static bool IsHeld(Keys key)
    {
        return InputManager.m_keyboard.IsKeyDown(key);
    }
    */

    public static bool IsPressed(string input)
    {
        return InputManager.m_pressed.Contains(input);
    }

    public static bool IsPressed(Keys key)
    {
      return InputManager.m_keyboard.IsKeyDown(key) && !InputManager.m_keyboardPrev.IsKeyDown(key);
    }

        /*
        public static bool IsReleased(string input)
        {
            return InputManager.m_released.Contains(input);
        }

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

        public static Keys GetInputKey(string input)
        {
            return InputManager.m_keyboardInputs[input];
        }

        public static void SetRebind(string input)
        {
            InputManager.m_rebind = input;
        }

        public static bool IsRebinding()
        {
            return InputManager.m_rebind != null;
        }

        public static Dictionary<string, Keys> GetInputDictionary()
        {
          Dictionary<string, Keys> inputDictionary = new Dictionary<string, Keys>();
          foreach (KeyValuePair<string, Keys> keyboardInput in InputManager.m_keyboardInputs)
            inputDictionary[keyboardInput.Key] = keyboardInput.Value;
          return inputDictionary;
        }*/



        private static void DetectSwipe()
        {
            // Calculate swipe distance and duration
            float distance = Vector2.Distance(m_swipeStart, m_swipeEnd);
            TimeSpan duration = DateTime.Now - m_swipeStartTime;

            if (distance >= MinSwipeDistance /*&& duration.TotalSeconds <= MaxSwipeDuration*/)
            {
                m_isSwipeDetected = true;
            }
        }

        public static bool IsSwipeDetected()
        {
            return m_isSwipeDetected;
        }

        public static Vector2 GetSwipeDirection()
        {
            //if (!m_isSwipeDetected) return Vector2.Zero;

            Vector2 direction = m_swipeEnd - m_swipeStart;
            direction.Normalize();
            return direction;
        }

        public static bool IsSwipeLeft()
        {
            //if (!m_isSwipeDetected) return false;

            Vector2 direction = GetSwipeDirection();
            return direction.X < -0.5f && Math.Abs(direction.Y) < 0.5f;
        }

        public static bool IsSwipeRight()
        {
            //if (!m_isSwipeDetected) return false;

            Vector2 direction = GetSwipeDirection();
            return direction.X > 0.5f && Math.Abs(direction.Y) < 0.5f;
        }

        public static bool IsSwipeUp()
        {
            //if (!m_isSwipeDetected) return false;

            Vector2 direction = GetSwipeDirection();
            return direction.Y < -0.5f && Math.Abs(direction.X) < 0.5f;
        }

        public static bool IsSwipeDown()
        {
            //if (!m_isSwipeDetected) return false;

            Vector2 direction = GetSwipeDirection();
            return direction.Y > 0.5f && Math.Abs(direction.X) < 0.5f;
        }

        internal static bool IsDoubleSwipeUp()
        {
            Vector2 direction = GetSwipeDirection();
            var result = direction.Y < -0.5f && Math.Abs(direction.X) < 0.5f;
            if (result)
            {
                if (m_touchCollection.Count < 2)
                    result = false;
            }
            return result;
        }
    }
}

