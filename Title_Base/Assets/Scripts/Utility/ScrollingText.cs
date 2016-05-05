using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TEXT_STATUS
{
    TEXT_IN_PROGRESS,
    TEXT_COMPLETE,
}

public class ScrollingText : MonoBehaviour
{
    // the  current text on screen
    string CurText = "";

    // the text we want to say
    [TextArea]
    public string TargetText = "";

    // how long to wait before printing each character
    public float CharacterDelay = 0.04f;

    public float SentenceDelay = 0.5f;

    // the timer to determine when to print a new chracter
    float StringTimer;

    // our current index into the target string
    int curCharacter = 0;

    // the textmesh component
    TextMesh TMesh = null;

    // the starting delay value.
    float startingdelay = 0.0f;

    public TEXT_STATUS TextStatus = TEXT_STATUS.TEXT_IN_PROGRESS;

    public void ClearText()
    {
        curCharacter = 0;
        CurText = string.Empty;
        TMesh.text = string.Empty;
        TargetText = string.Empty;
        TextStatus = TEXT_STATUS.TEXT_COMPLETE;
    }

	void Start ()
    {
        startingdelay = CharacterDelay;
        TMesh = GetComponentInChildren<TextMesh>();
	}
	
    public void ChangeText(string NewText)
    {
        curCharacter = 0;
        CurText = string.Empty;
        TMesh.text = string.Empty;
        TargetText = NewText;
    }

    public void SkipToEnd()
    {
        curCharacter = TargetText.Length;
        CurText = TargetText;
        TMesh.text = TargetText;
        TextStatus = TEXT_STATUS.TEXT_COMPLETE;
    }

	// Update is called once per frame
	void Update ()
    {
	    if(curCharacter < TargetText.Length)
        {
            TextStatus = TEXT_STATUS.TEXT_IN_PROGRESS;

            StringTimer += Time.deltaTime;

            if (StringTimer > CharacterDelay)
            {
                string subString = TargetText.Substring(curCharacter, 1);

                if(subString == ".")
                {
                    CharacterDelay = SentenceDelay;
                }
                else
                {
                    CharacterDelay = startingdelay;
                }

                CurText = string.Concat(CurText, subString);
                ++curCharacter;
                StringTimer = 0.0f;
                TMesh.text = CurText;
            }

        }
        else
        {
            TextStatus = TEXT_STATUS.TEXT_COMPLETE;
        }
	}
}
