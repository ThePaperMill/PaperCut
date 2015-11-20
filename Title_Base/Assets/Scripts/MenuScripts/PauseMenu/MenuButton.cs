using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour 
{
  public int MenuNumber = 0;
  public int Priority = 0;
  GameObject LevelSettings = null;
  MenuManager Mmngr = null;

	// Use this for initialization
  void Awake () 
  {
    LevelSettings = GameObject.FindGameObjectWithTag("LevelSettings");

    if(LevelSettings)
    {
      Mmngr = LevelSettings.GetComponent<MenuManager>();

      if(Mmngr)
      {
        Mmngr.AddButton(MenuNumber, this);
      }
    }
	}
	
  public virtual void Activate()
  {
    print("button pressed" + Priority);
  }

	// Update is called once per frame
	void Update () 
  {
	
	}
}
