using UnityEngine;
using System.Collections;
using ActionSystem;

public class EditEmissiveOnEvent : EditOnEvent
{
    public Color Emmissive = new Color(0, 0, 0);
    public float Duration = 1.0f;
    public Curve EasingCurve = Ease.Linear;
    bool Running = false;
    [HideInInspector]
    public Vector4 StartingColor { get; set; }
    Renderer ObjectRenderer;
	// Use this for initialization
	void Start ()
    {
        ObjectRenderer = GetComponent<Renderer>();
        if (!ObjectRenderer)
        {
            throw new System.Exception("Object " + name + "Has not component 'Renderer'.");
        }
    }

    public override void OnEventFunc(EventData data)
    {

        
        StartingColor = ObjectRenderer.material.GetColor("_EmissionColor");
        
        var Seq = Action.Sequence(Actions);
        Action.Property<Vector4>(Seq, this.GetProperty( o => o.StartingColor), Emmissive, Duration, EasingCurve);
        SetRunning(true);
        Action.Call(Seq, SetRunning, false);
        EditChecks(Seq);
        
        
    }
    void SetRunning( bool val)
    {
        Running = val;
    }
    // Update is called once per frame
    void Update ()
    {
	    if(Running)
        {
            //Debug.Log(StartingColor);
            ObjectRenderer.material.SetColor("_EmissionColor", StartingColor);
        }
	}
}

//Standard Shader Properties
/* _Color
 _MainTex
 _Cutoff
 _Glossiness
 _Metallic
 _MetallicGlossMap
 _BumpScale
 _BumpMap
 _Parallax
 _ParallaxMap
 _OcclusionStrength
 _OcclusionMap
 _EmissionColor
 _EmissionMap
 _DetailMask
 _DetailAlbedoMap
 _DetailNormalMapScale
 _DetailNormalMap
 _UVSec
 _EmissionScaleUI
 _EmissionColorUI
 _Mode
 _SrcBlend
 _DstBlend
 _ZWrite
*/