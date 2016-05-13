using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CycleMaterials : MonoBehaviour {

    public List<Material> MaterialsArray = new List<Material>(); //Array of materials/textures
    public List<int> IntArray = new List<int>(); //

    public int GlobalI = 0;

    public Material Mat1 = null;
    public Material Mat2 = null;
    public Material Mat3 = null;
    public Material Mat4 = null;

    public int CycleTimer = 0;

    // Use this for initialization
    void Start () {
        //CycleTimer = 6;
        MaterialsArray.Add(Mat1);
        MaterialsArray.Add(Mat2);
        MaterialsArray.Add(Mat3);
        MaterialsArray.Add(Mat4);

        //IntArray.
    }
	
	// Update is called once per frame
	void Update () {
        //print("MY CURRENT TEXTURE IS THE FIRST");
        ++CycleTimer;
        if(CycleTimer >= 5)
        {
            CycleTimer = 0;
            NextMaterial();
        }
	}

    void NextMaterial()
    {/*
        if (this.GetComponent<MeshRenderer>().sharedMaterial.name == "OeFrame1")
        {
            //print("MY CURRENT TEXTURE IS THE FIRST");
            this.GetComponent<MeshRenderer>().sharedMaterial = Mat2;
        }
        else if (this.GetComponent<MeshRenderer>().sharedMaterial.name == "OeFrame2")
        {
            this.GetComponent<MeshRenderer>().sharedMaterial = Mat3;
        }
        else if (this.GetComponent<MeshRenderer>().sharedMaterial.name == "OeFrame3")
        {
            this.GetComponent<MeshRenderer>().sharedMaterial = Mat4;
        }
        else if (this.GetComponent<MeshRenderer>().sharedMaterial.name == "OeFrame3")
        {
            this.GetComponent<MeshRenderer>().sharedMaterial = Mat4;
        }*/

        if (GlobalI == 0)
        {
            //print("MY CURRENT TEXTURE IS THE FIRST");
            this.GetComponent<MeshRenderer>().sharedMaterial = Mat2;
            ++GlobalI; //Now has 2nd texture/material; globalI
        }
        else if (GlobalI == 1)
        {
            this.GetComponent<MeshRenderer>().sharedMaterial = Mat3;
            ++GlobalI;
        }
        else if (GlobalI == 2)
        {
            this.GetComponent<MeshRenderer>().sharedMaterial = Mat4;
            ++GlobalI;
        }
        else if (GlobalI == 3)
        {
            this.GetComponent<MeshRenderer>().sharedMaterial = Mat1;
            GlobalI = 0;
        }
    }
    //How to access the current gameobject's materials
}
