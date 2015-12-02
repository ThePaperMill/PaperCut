/****************************************************************************/
/*!
\file   TileMasterScript
\author Ian Aemmer
\brief  
    Master of tiles.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class TileMasterScript : MonoBehaviour {

    public GameObject PreFab;

    public float HeighthDistance = 0.05f;
    public float WidthDistance = 0.05f;
    public float DepthDistance = 0.05f;

    int Height = 1;
    int Depth = 1;
    int Width = 1;

    // Use this for initialization
    void Start ()
    {
       //print("I need to alter the editSystem for Removal. I need to alter the edit System");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Width * Depth * Height > 1000)
        {
            print("Your object is WAAAY too big. Fix that. ~TileMasterScript");
            return;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            print("Up Key was pressed");
            //For each object along the width,
            for(int i = 0; i < Width; ++i)
            {
                for (int j = 0; j < Height; ++j)
                {
                    //I need to create a Peice in the Z direction.
                    GameObject newblock = Instantiate(PreFab, gameObject.transform.localPosition, Quaternion.identity) as GameObject;
                    //Makes the GameObject "newblock" the parent of the GameObject "player".
                    newblock.transform.parent = gameObject.transform;
                    newblock.transform.Translate(i * WidthDistance, j*HeighthDistance, Depth * DepthDistance);
                }
            }
            Depth += 1;
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            print("Right Key was pressed");
            //For each object along the depth,
            for (int i = 0; i < Depth; ++i)
            {
                //Foreach object along the height,
                for(int j = 0; j < Height; ++j)
                {
                    //I need to create a Peice in the Z direction.
                    GameObject newblock = Instantiate(PreFab, gameObject.transform.localPosition, Quaternion.identity) as GameObject;
                    //Makes the GameObject "newblock" the parent of the GameObject "player".
                    newblock.transform.parent = gameObject.transform;
                    newblock.transform.Translate(Width*WidthDistance, j*HeighthDistance, i*DepthDistance);
                }
            }
            Width += 1;
        }

        else if (Input.GetKeyDown(KeyCode.I))
        {
            print("Up Key was pressed");
            //For each object along the width,
            for (int i = 0; i < Depth; ++i)
            {
                for(int j = 0; j < Width; ++j)
                {
                    //I need to create a Peice in the Z direction.
                    GameObject newblock = Instantiate(PreFab, gameObject.transform.localPosition, Quaternion.identity) as GameObject;
                    //Makes the GameObject "newblock" the parent of the GameObject "player".
                    newblock.transform.parent = gameObject.transform;
                    newblock.transform.Translate(j * WidthDistance, Height * HeighthDistance, i * DepthDistance);
                }
            }
            Height += 1;
        }
    }
}