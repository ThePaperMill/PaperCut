/*********************************
 * InstantiateMeshControllerOld.cs
 * Troy
 * Created 10/2/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using System.Collections;

public class InstantiateMeshControllerOld : MonoBehaviour {

	public Mesh mesh;
    public Material material;

	Vector3[] baseVertices;
    bool firstFrame = true;

	// Use this for initialization
	void Start ()
	{ 
		float scale = 20.0f;
		bool recalculateNormals = false;

		if (baseVertices == null)
		{
			baseVertices = mesh.vertices;
		}

		Vector3[] vertices = baseVertices;

		for (var i = 0; i < baseVertices.Length; i++)
		{
			Vector3 vertex = baseVertices[i];
			vertex.x = vertex.x * scale;
			vertex.y = vertex.y * scale;
			vertex.z = vertex.z * scale;

			vertices[i] = vertex;
		}

		mesh.vertices = vertices;
			
		if (recalculateNormals)
		{   
			mesh.RecalculateNormals();
		}
		mesh.RecalculateBounds();

		Graphics.DrawMesh(mesh, new Vector3(4, 4, 0), Quaternion.identity, material, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
        // Don't call in samew frame as Start() does
        if (firstFrame)
        {
            firstFrame = false;
        }
        else
        {
            Graphics.DrawMesh(mesh, new Vector3(4, 4, 0), Quaternion.identity, material, 0);
        }
	}
}
