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

  int CurrentMenu   = 0;
  int CurrentButton = 0;

  Vector3 ButtonOffset = new Vector3(-1,0,0);

  Dictionary<int, MenuInfo> Menus = new Dictionary<int, MenuInfo>();

  GameObject Selector = null;
  MeshRenderer SelectorRenderer;

  MenuManager()
  {
  }
  
  void OnDestroy()
  {
    EventSystem.GlobalHandler.Connect(Events.PauseGameEvent, OnPauseGameEvent);
    EventSystem.GlobalHandler.Connect(Events.ResumeGameEvent, OnResumeGameEvent);
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

    Selector = Instantiate(Resources.Load("MenuSelector")) as GameObject;
    SelectorRenderer = Selector.GetComponent<MeshRenderer>();
    SelectorRenderer.enabled = false;
	}

	// Update is called once per frame
	void Update () 
  {
    if(Menus.ContainsKey(CurrentMenu) == false || Menus[CurrentMenu].MenuActive == false)
    {
      if (Selector)
      {
        Selector.SetActive(false);
      }
      return;
    }

    List<MenuButton> Buttons = Menus[CurrentMenu].Buttons;

    if(Buttons.Count != 0)
    {
      UpdateInput();
      UpdateSelector();
      
      if(Activate)
      {
        Buttons[CurrentButton].Activate();
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
    List<MenuButton> Buttons = Menus[CurrentMenu].Buttons;

    if (MoveDown || (StickTriggered && leftStickInfo.YPos < 0.0f))
    {
      ++CurrentButton;

      if (CurrentButton == Buttons.Count)
      {
        CurrentButton = 0;
      }
    }

    else if (MoveUp || (StickTriggered && leftStickInfo.YPos > 0.0f))
    {
      --CurrentButton;

      if (CurrentButton < 0)
      {
        CurrentButton = Buttons.Count - 1;
      }
    }

    if(Selector)
      Selector.transform.position = Buttons[CurrentButton].gameObject.transform.position + ButtonOffset;
  }

  void UpdateInput()
  {
    MoveUp         = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_DPAD_UP);
    MoveDown       = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_DPAD_DOWN);
    Activate       = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_A) || InputManager.GetSingleton.IsKeyTriggered(KeyCode.Space);
    StickTriggered = InputManager.GetSingleton.IsLeftStickTriggered();
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

  void ActivateMenu(int Menu)
  {
    if (Menus.ContainsKey(Menu))
    {
      CurrentMenu = Menu;
      CurrentButton = 0;
      Menus[Menu].MenuActive = true;

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
