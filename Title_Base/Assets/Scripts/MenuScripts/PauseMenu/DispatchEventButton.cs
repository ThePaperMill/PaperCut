using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DispatchEventButton : MenuButton
{
    public List<string> DispatchEvents = new List<string>();

    public bool ChangeMenu = false;
    public int NewMenu = 0;


    void Start () 
    {
	
    }
	

    void Update () 
    {
	
    }

    public override void Activate()
    {
        foreach (var Event in DispatchEvents)
        {
            EventSystem.GlobalHandler.DispatchEvent(Event);
        }

        if(ChangeMenu)
        {
           var test =  GameObject.FindGameObjectWithTag("LevelSettings");
            if(test)
            {
                test.GetComponent<MenuManager>().ActivateMenu(NewMenu);
            }
        }
    }
}
