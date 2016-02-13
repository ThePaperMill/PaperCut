using UnityEngine;
using System.Collections;

public class TestMaterialChanger : MonoBehaviour {

    bool Canrun = false;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        //print(gameObject.name);

        //Animation myanim = gameObject.GetComponent<Animation>();
        //print(myanim["Take_001"].speed);
        Animator anim = transform.parent.GetComponent<Animator>();
        //print(anim);
        if(anim != null)
        {
            //anim.Play(0);
        }
        
        if(Canrun != true)
        {
            return;
        }

        Renderer myrender = gameObject.GetComponent<Renderer>();
        //print(myrender.material.GetFloat("_SliceAmount"));
        //myrender.material
        myrender.material.SetFloat("_SliceAmount", myrender.material.GetFloat("_SliceAmount") + 0.005f * Time.deltaTime);
        if(myrender.material.GetFloat("_BurnSize") <= 2.8)
        {
            myrender.material.SetFloat("_BurnSize", myrender.material.GetFloat("_BurnSize") + 0.05f * Time.deltaTime);

        }
        else
        {
            myrender.material.SetFloat("_SliceAmount", myrender.material.GetFloat("_SliceAmount") + 0.05f * Time.deltaTime);

        }
    }
    public void TurnOn()
    {
        Canrun = true;
        //print("working");
    }
}
