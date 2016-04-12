/****************************************************************************/
/*!
\file  LevelIntro.cs
\author Steven Gallwas 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;

public class LevelIntro : MonoBehaviour
{
    GameObject LSettings = null;
    LevelInfo LInfo = null;
    TextMesh TMesh = null;
    ActionSequence Seq = new ActionSequence();
    ActionSequence Seq2 = new ActionSequence();
    public bool UseUnderline = false;
    GameObject UnderLineChild = null;

    public float Alpha { get; set; }

    // Use this for initialization
    void Start()
    {
        LSettings = GameObject.FindGameObjectWithTag("LevelSettings");
        TMesh = GetComponent<TextMesh>();

        var childtemp = transform.FindChild("Underline");

        if (childtemp)
        {
            UnderLineChild = childtemp.gameObject;
        }

        if (UnderLineChild)
        {
            UnderLineChild.GetComponent<MeshRenderer>().material.color = TMesh.color;
            UnderLineChild.transform.localScale = new Vector3(0, UnderLineChild.transform.localScale.y, UnderLineChild.transform.localScale.z);
        }

        //StartColor = TMesh.color;
        Alpha = 0;
        if (LSettings)
        {
            LInfo = LSettings.GetComponent<LevelInfo>();

            if (LInfo)
            {
                Action.Property(Seq, this.GetProperty(o => o.Alpha), 1.0f, 1.5f, Ease.Linear);
                Action.Delay(Seq, 0.5f);
                Action.Property(Seq, this.GetProperty(o => o.Alpha), 0.0f, 1.5f, Ease.Linear);

                if (UnderLineChild)
                {
                    Action.Delay(Seq2, 1.5f);

                    //var length = TMesh.GetComponent<MeshRenderer>().bounds.extents.x;

                    //print(length);

                    Action.Property(Seq2, UnderLineChild.transform.GetProperty(o => o.localScale), new Vector3(100.0f, UnderLineChild.transform.localScale.y, UnderLineChild.transform.localScale.y), 0.85f, Ease.Linear);
                }

                if (LInfo.IsFinalLevel == false)
                    TMesh.text = LInfo.LevelName;
                else
                    TMesh.text = "Final Level" + " - " + LInfo.LevelName;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        Seq.Update(Time.deltaTime);

        if(UseUnderline)
            Seq2.Update(Time.deltaTime);

        TMesh.color = new Vector4(TMesh.color.r, TMesh.color.g, TMesh.color.b, Alpha);

        if (UnderLineChild)
        {
            UnderLineChild.GetComponent<Renderer>().material.color = new Vector4(TMesh.color.r, TMesh.color.g, TMesh.color.b, Alpha);
        }
    }
}
