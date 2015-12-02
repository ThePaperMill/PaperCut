/****************************************************************************/
/*!
\file   GamestateManager.cs
\author Steven Gallwas
\brief  
       The gamestate manager to handle pause and cutscenes
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;
using System.IO;

public enum GAME_STATE
{
  GS_PAUSE,
  GS_CINEMATIC,
  GS_GAME,
}

public class GamestateManager : Singleton<GamestateManager>
{
    public GAME_STATE CurState = GAME_STATE.GS_GAME;
    public bool IsPaused = false;
    public bool AllowQuit = false;

    //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    //private static extern int GetWindowThreadProcessId(HandleRef handle, out int processId);

    //private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    //private static extern bool EnumWindows(EnumWindowsProc callback, IntPtr extraData);

    //bool bUnityHandleSet = false;
    //HandleRef unityWindowHandle;


    //public bool EnumWindowsCallBack(IntPtr hWnd, IntPtr lParam)
    //{
    //    int procid;
    //    int returnVal = GetWindowThreadProcessId(new HandleRef(this, hWnd), out procid);

    //    int currentPID = System.Diagnostics.Process.GetCurrentProcess().Id;

    //    HandleRef handle = new HandleRef(this, System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);

    //    if (procid == currentPID)
    //    {
    //        unityWindowHandle = new HandleRef(this, hWnd);
    //        bUnityHandleSet = true;
    //        return false;
    //    }

    //    return true;
    //}


    // Use this for initialization
    void Start ()
    {

    }

    public void Initialize()
    {

    }

    void OnLevelWasLoaded()
    {
        ResumeGame();
    }

    void OnApplicationQuit()
    {
        if (AllowQuit == false)
        {
            Application.CancelQuit();

            if (Application.loadedLevelName == "ScrollingCredits" || Application.loadedLevelName == "SplashScreens")
            {
                return;
            }
            else
            {
                PauseGame();
                EventSystem.GlobalHandler.DispatchEvent(Events.InitiateQuitEvent);
            }
        }           
    }

    public void PauseGame()
    {
        if (IsPaused)
            return;

        EventSystem.GlobalHandler.DispatchEvent(Events.PauseGameEvent, null);
        IsPaused = true;
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        EventSystem.GlobalHandler.DispatchEvent(Events.ResumeGameEvent, null);
        IsPaused = false;
        Time.timeScale = 1.0f;
    }


	// Update is called once per frame
    void Update ()
    {

    }
}
