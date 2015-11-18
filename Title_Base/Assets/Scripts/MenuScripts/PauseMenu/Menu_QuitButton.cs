using UnityEngine;
using System.Collections;

public class Menu_QuitButton : MenuButton
{
  public override void Activate()
  {
    Application.Quit();
  }

	// Update is called once per frame
	void Update () 
  {
	
	}
}
