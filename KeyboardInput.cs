// Decompiled with JetBrains decompiler
// Type: StardewValley.KeyboardInput
// Assembly: Stardew Valley, Version=1.2.6400.27469, Culture=neutral, PublicKeyToken=null
// MVID: 77B7094A-F6F0-4ACC-91F4-E335E2733EDB
// Assembly location: D:\SteamLibrary\steamapps\common\Stardew Valley\Stardew Valley.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.InteropServices;

namespace StardewValley
{
  public static class KeyboardInput
  {
    private static bool initialized;
    private static IntPtr prevWndProc;
    private static KeyboardInput.WndProc hookProcDelegate;
    private static IntPtr hIMC;
    private const int GWL_WNDPROC = -4;
    private const int WM_KEYDOWN = 256;
    private const int WM_KEYUP = 257;
    private const int WM_CHAR = 258;
    private const int WM_IME_SETCONTEXT = 641;
    private const int WM_INPUTLANGCHANGE = 81;
    private const int WM_GETDLGCODE = 135;
    private const int WM_IME_COMPOSITION = 271;
    private const int DLGC_WANTALLKEYS = 4;

    public static event CharEnteredHandler CharEntered;

    public static event KeyEventHandler KeyDown;

    public static event KeyEventHandler KeyUp;

    [DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr ImmGetContext(IntPtr hWnd);

    [DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    public static void Initialize(GameWindow window)
    {
      if (KeyboardInput.initialized)
        throw new InvalidOperationException("TextInput.Initialize can only be called once!");
      KeyboardInput.hookProcDelegate = new KeyboardInput.WndProc(KeyboardInput.HookProc);
      KeyboardInput.prevWndProc = (IntPtr) KeyboardInput.SetWindowLong(window.Handle, -4, (int) Marshal.GetFunctionPointerForDelegate((Delegate) KeyboardInput.hookProcDelegate));
      KeyboardInput.hIMC = KeyboardInput.ImmGetContext(window.Handle);
      KeyboardInput.initialized = true;
    }

    private static IntPtr HookProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
      IntPtr num = KeyboardInput.CallWindowProc(KeyboardInput.prevWndProc, hWnd, msg, wParam, lParam);
      if (msg <= 135U)
      {
        if ((int) msg != 81)
        {
          if ((int) msg == 135)
            num = (IntPtr) (num.ToInt32() | 4);
        }
        else
        {
          KeyboardInput.ImmAssociateContext(hWnd, KeyboardInput.hIMC);
          num = (IntPtr) 1;
        }
      }
      else
      {
        switch (msg)
        {
          case 256:
            // ISSUE: reference to a compiler-generated field
            if (KeyboardInput.KeyDown != null)
            {
              // ISSUE: reference to a compiler-generated field
              KeyboardInput.KeyDown((object) null, new KeyEventArgs((Keys) (int) wParam));
              break;
            }
            break;
          case 257:
            // ISSUE: reference to a compiler-generated field
            if (KeyboardInput.KeyUp != null)
            {
              // ISSUE: reference to a compiler-generated field
              KeyboardInput.KeyUp((object) null, new KeyEventArgs((Keys) (int) wParam));
              break;
            }
            break;
          case 258:
            // ISSUE: reference to a compiler-generated field
            if (KeyboardInput.CharEntered != null)
            {
              // ISSUE: reference to a compiler-generated field
              KeyboardInput.CharEntered((object) null, new CharacterEventArgs((char) (int) wParam, lParam.ToInt32()));
              break;
            }
            break;
          case 641:
            if (wParam.ToInt32() == 1)
            {
              KeyboardInput.ImmAssociateContext(hWnd, KeyboardInput.hIMC);
              break;
            }
            break;
        }
      }
      return num;
    }

    private delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
  }
}
