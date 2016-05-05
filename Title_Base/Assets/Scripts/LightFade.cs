/****************************************************************************/
/*!
\file   LightFade.cs
\author Michael Van Zant
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;

public class LightFade : MonoBehaviour
{
    [SerializeField]
    private float StartValue;
    [SerializeField]
    private float EndValue;
    [SerializeField]
    private float LengthOfTime;

    private float ValueStep;

    ActionGroup Grp = new ActionGroup();

    // Use this for initialization
    void Start ()
    {
        gameObject.GetComponent<Light>().intensity = StartValue;

        var seq = ActionSystem.Action.Sequence(Grp, true);
        ActionSystem.Action.Property(seq, this.gameObject.GetComponent<Light>().GetProperty(o => o.intensity), EndValue, LengthOfTime, Ease.Linear);
    }
	
	// Update is called once per frame
	void Update ()
    {
        print(gameObject.GetComponent<Light>().intensity);

        Grp.Update(Time.smoothDeltaTime);
    }
}
