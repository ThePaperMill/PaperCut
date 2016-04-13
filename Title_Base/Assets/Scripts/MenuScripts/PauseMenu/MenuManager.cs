/****************************************************************************/
/*!
\file   MenuManager.cs
\author Steven Gallwas
\brief  
    Manages the menu.  Should never have more than 1.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuInfo
{
  public MenuInfo()
  {
    Buttons = new List<MenuButton>();
  }

  public void AddButton(MenuButton button)
  {
    Buttons.Add(button);
    SortButtons();
  }

  void SortButtons()
  {
    // selection sort for the lazy \^_^/
    for (int i = 0; i < Buttons.Count; ++i)
    {
      int j = i;
      var current = Buttons[i];

      while ((j > 0) && (Buttons[j - 1].Priority > current.Priority))
      {
        Buttons[j] = Buttons[j - 1];
        j--;
      }

      Buttons[j] = current;
    }
  }

  public bool MenuActive = false;
  public List<MenuButton> Buttons;
}

public class MenuManager : EventHandler
{
  bool MoveUp         = false;
  bool MoveDown       = false;
  bool Activate       = false;
  bool StickTriggered = false;
  bool Escape         = false;

  bool OverlayActive = false;
  int CurrentMenu   = 0;
  int CurrentButton = 0;

  FMOD_StudioEventEmitter SoundEmitter = null;

  Vector3 ButtonOffset = new Vector3(0,-0.075f, -0.045f);

  Dictionary<int, MenuInfo> Menus = new Dictionary<int, MenuInfo>();

  GameObject Selector = null;

  public bool MainMenu = false;

    [HideInInspector]
    public int PrevMenu = -1;

  MenuManager()
  {
      
  }
  
  void OnDestroy()
  {
    EventSystem.GlobalHandler.Disconnect(Events.PauseGameEvent, OnPauseGameEvent);
    EventSystem.GlobalHandler.Disconnect(Events.ResumeGameEvent, OnResumeGameEvent);
    EventSystem.GlobalHandler.Disconnect(Events.InitiateQuitEvent, OnInitiateQuitEvent);
    EventSystem.GlobalHandler.Disconnect(Events.CancelQuitEvent, OnCancelQuitEvent);
    EventSystem.GlobalHandler.Disconnect(Events.OverlayActive, OnOverlayActive);
  }

  void OnPauseGameEvent(EventData data)
  { 
        ActivateMenu(0);
  }

  void OnResumeGameEvent(EventData data)
  {
    	DeactivateMenu(0);
  }

  	// Use this for initialization
  void Start () 
  {
      EventSystem.GlobalHandler.Connect(Events.PauseGameEvent, OnPauseGameEvent);
      EventSystem.GlobalHandler.Connect(Events.ResumeGameEvent, OnResumeGameEvent);
      EventSystem.GlobalHandler.Connect(Events.InitiateQuitEvent, OnInitiateQuitEvent);
      EventSystem.GlobalHandler.Connect(Events.CancelQuitEvent, OnCancelQuitEvent);
      EventSystem.GlobalHandler.Connect(Events.OverlayActive, OnOverlayActive);

      Selector = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("MenuSelector"));
      
      if(Selector)
      {
        SoundEmitter = Selector.GetComponent<FMOD_StudioEventEmitter>();
      }
    
      if(MainMenu)
      {
          ActivateMenu(0);
      }
  }

    void OnOverlayActive(EventData data)
    {
        OverlayActive = true;
    }

  void OnInitiateQuitEvent(EventData data)
  {
    EventSystem.GlobalHandler.DispatchEvent(Events.CancelOverlay);
    OverlayActive = false;

    DeactivateMenu(0);
    ActivateMenu(1);
  }

  void OnCancelQuitEvent(EventData data)
  {
    DeactivateMenu(1);
    ActivateMenu(0);
  }

	// Update is called once per frame
  void LateUpdate () 
  {
    UpdateInput();
    
    if(OverlayActive && (Escape || Activate))
    {
            EventSystem.GlobalHandler.DispatchEvent(Events.CancelOverlay);
            OverlayActive = false;
            return;
    }

    if (Menus.ContainsKey(CurrentMenu) == false || Menus[CurrentMenu].MenuActive == false)
    {
        if (Escape)
        {
            GamestateManager.GetSingleton.PauseGame();
        }

        else if (Selector)
        {
            Selector.SetActive(false);
        }

         return;
    }

    if(Menus[CurrentMenu].Buttons.Count != 0)
    {
        UpdateSelector();
      
        if(Activate)
        {
            Menus[CurrentMenu].Buttons[CurrentButton].Activate();
        }

        if(Escape)
        {
            if(CurrentMenu == 0 && !MainMenu)
            {
                GamestateManager.GetSingleton.ResumeGame();
                return;
            }
        }

      if (Selector)
      {
        Selector.SetActive(true);
      }
    }
    else
    {
      if (Selector)
      {
        Selector.SetActive(false);
      }
    }
  }

  void UpdateSelector()
  {
    var leftStickInfo = InputManager.GetSingleton.GetLeftStickValues();
    var rightStickInfo = InputManager.GetSingleton.GetRightStickValues();

    if (MoveDown || (StickTriggered && (leftStickInfo.YPos < 0.0f || rightStickInfo.YPos < 0.0f)))
    {
      ++CurrentButton;

      if(SoundEmitter && GlobalSoundInitializer.GetSingleton.FmodSoundInitialzied == true)
      {
        SoundEmitter.StartEvent();
      }

      if (CurrentButton == Menus[CurrentMenu].Buttons.Count)
      {
        CurrentButton = 0;
      }
    }

    else if (MoveUp || (StickTriggered && (leftStickInfo.YPos > 0.0f || rightStickInfo.YPos > 0.0f)))
    {
      --CurrentButton;

      if (SoundEmitter)
      {
        SoundEmitter.StartEvent();
      }

      if (CurrentButton < 0)
      {
        CurrentButton = Menus[CurrentMenu].Buttons.Count - 1;
      }
    }

    if (Selector)
    {
        Selector.transform.position = Menus[CurrentMenu].Buttons[CurrentButton].gameObject.transform.position + ButtonOffset;
    }
  }

  void UpdateInput()
  {
    MoveUp         = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_DPAD_UP)   || InputManager.GetSingleton.IsKeyTriggered(KeyCode.W) || InputManager.GetSingleton.IsKeyTriggered(KeyCode.UpArrow);
    MoveDown       = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_DPAD_DOWN) || InputManager.GetSingleton.IsKeyTriggered(KeyCode.S) || InputManager.GetSingleton.IsKeyTriggered(KeyCode.DownArrow); 
    Activate       = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_A)         || InputManager.GetSingleton.IsKeyTriggered(KeyCode.Space) || InputManager.GetSingleton.IsKeyTriggered(KeyCode.Return);
    StickTriggered = InputManager.GetSingleton.IsLeftStickTriggered()                             || InputManager.GetSingleton.IsRightStickTriggered();
    Escape         = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_START)     || InputManager.GetSingleton.IsKeyTriggered(KeyCode.Escape);
  }

  public void AddButton(int Menu, MenuButton button)
  {
    if(Menus.ContainsKey(Menu) == false)
    {
      Menus.Add(Menu,new MenuInfo());
    }

    Menus[Menu].AddButton(button);
  }

  void ActivateButton()
  {
    List<MenuButton> Buttons = Menus[CurrentMenu].Buttons;
    Buttons[CurrentButton].Activate();
  }

  public void ActivateMenu(int Menu)
  {
    if (Menus.ContainsKey(Menu))
    {
      //Menus[CurrentMenu].MenuActive = false;
      CurrentMenu = Menu;
      CurrentButton = 0;
      Menus[Menu].MenuActive = true;
      PrevMenu = Menu;

      List<MenuButton> Buttons = Menus[CurrentMenu].Buttons;

      if (Buttons.Count != 0)
      {
        UpdateSelector();
      }
    }
  }

  void DeactivateMenu(int Menu)
  {
    if (Menus.ContainsKey(Menu))
    {
      CurrentMenu = Menu;
      CurrentButton = 0;
      Menus[Menu].MenuActive = false;
    }
  }
}
