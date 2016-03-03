using UnityEngine;
using System.Collections;
using ActionSystem;

public class LevelIntro : MonoBehaviour
{
    GameObject LSettings = null;
    LevelInfo LInfo = null;
    TextMesh TMesh = null;
    ActionSequence Seq = new ActionSequence();
    Color StartColor = Color.clear;

    public float Alpha { get; set; }

// Use this for initialization
void Start()
    {
        LSettings = GameObject.FindGameObjectWithTag("LevelSettings");
        TMesh = GetComponent<TextMesh>();

        StartColor = TMesh.color;
        Alpha = 0;

        if (LSettings)
        {
            LInfo = LSettings.GetComponent<LevelInfo>();

            if (LInfo)
            {
                Action.Property(Seq, this.GetProperty(o => o.Alpha), 1.0f, 1.5f, Ease.Linear);
                Action.Property(Seq, this.GetProperty(o => o.Alpha), 0.0f, 1.5f, Ease.Linear);


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
        TMesh.color = new Vector4(TMesh.color.r, TMesh.color.g, TMesh.color.b, Alpha);
    }
}
