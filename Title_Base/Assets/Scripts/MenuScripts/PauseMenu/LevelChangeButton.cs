using UnityEngine;
using System.Collections;

public class LevelChangeButton : MenuButton
{
  public string Level = "";

  public override void Activate()
  {
    Application.LoadLevel(Level);
  }
	
	// Update is called once per frame
	void Update () 
  {
	
	}
}
