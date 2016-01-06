/*********************************
 * MainMenuLogic.cs
 * Troy
 * Created 9/4/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuLogic : MonoBehaviour {
	
	public Canvas quitMenu;
	public Canvas mainMenu;
	public Canvas creditMenu;
	public Canvas instructMenu;

	public Button startOption;
	public Button instructOption;
	public Button creditOption;
	public Button quitOption;
	// Add more public Buttons for other options!


	// Initialization
	void Start ()
	{
		quitMenu = quitMenu.GetComponent<Canvas> ();
		mainMenu = mainMenu.GetComponent<Canvas> ();
		creditMenu = creditMenu.GetComponent<Canvas> ();
		instructMenu = instructMenu.GetComponent<Canvas> ();

		startOption = startOption.GetComponent<Button> ();
		quitOption = instructOption.GetComponent<Button> ();
		startOption = creditOption.GetComponent<Button> ();
		quitOption = quitOption.GetComponent<Button> ();

		mainMenu.enabled = true;
		quitMenu.enabled = false;
		creditMenu.enabled = false;
		instructMenu.enabled = false;
	}

	public void ExitPressed()
	{
		quitMenu.enabled = true;
		creditMenu.enabled = false;
		instructMenu.enabled = false;

		startOption.enabled = false;
		instructOption.enabled = false;
		creditOption.enabled = false;
		quitOption.enabled = false;
	}

	public void NonePressed()
	{
		mainMenu.enabled = true;
		quitMenu.enabled = false;
		creditMenu.enabled = false;
		instructMenu.enabled = false;

		startOption.enabled = true;
		instructOption.enabled = true;
		creditOption.enabled = true;
		quitOption.enabled = true;
	}

	public void CreditPressed()
	{
		mainMenu.enabled = false;
		quitMenu.enabled = false;
		instructMenu.enabled = false;
		creditMenu.enabled = true;
		
		startOption.enabled = false;
		instructOption.enabled = false;
		creditOption.enabled = false;
		quitOption.enabled = false;
	}
	
	public void InstructPressed()
	{
		mainMenu.enabled = false;
		quitMenu.enabled = false;
		instructMenu.enabled = true;
		creditMenu.enabled = false;
		
		startOption.enabled = false;
		instructOption.enabled = false;
		creditOption.enabled = false;
		quitOption.enabled = false;
	}
	
	public void StartGame()
	{
		SceneManager.LoadScene("Player_House");
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
