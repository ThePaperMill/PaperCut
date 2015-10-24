/*********************************
 * MeshCombine.cs
 * Troy
 * Created 10/17/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[ExecuteInEditMode()]
[AddComponentMenu("Mesh Combine")]
public class MeshCombine : MonoBehaviour
{
    public string combineName = "Combined Mesh";

    public bool addMeshCollider;
    public bool destroyAfterwards;
    public bool getShadows = true;
    public bool castShadow = true;
    public bool combineOnStart = true;

    /// Rendering with a triangle list is usually faster, but when there's very few triangles the list takes longer
    public bool generateTriangles = true;

    public int waitFrame;

    private void Start()
    {
        if (combineOnStart && waitFrame == 0)
        {
            CombineImmediately();
        }

        else
        {
            StartCoroutine(CombineLate());
        }
    }

    [ContextMenu("Start Combining")]
    public void BeginToCombine()
    {
        MeshCombine[] children = gameObject.GetComponentsInChildren<MeshCombine>();

        for (int loop = 0; loop < children.Length; ++loop)
        {
            if (this != children[loop])
            {
                children[loop].CombineImmediately();
            }
        }

        combineOnStart = enabled = false;
    }

    private IEnumerator CombineLate()
    {
        // Just combine them as fast/slow
        for (int loop = 0; loop < waitFrame; ++loop)
        {
            yield return 0;
        }

        CombineImmediately();
    }

    /// This option has a far longer preprocessing time at startup but leads to better runtime performance.
    [ContextMenu("Combine Immediately")]
    public void CombineImmediately()
    {
        MeshFilter[] filterList = GetComponentsInChildren<MeshFilter>();
        Matrix4x4 pos = transform.worldToLocalMatrix;
        Dictionary<Material, List<NewMeshInstantiate.MeshInstance>> materialToMesh = new Dictionary<Material, List<NewMeshInstantiate.MeshInstance>>();

        foreach (MeshFilter mfilter in filterList)
        {
            Renderer current = mfilter.GetComponent<Renderer>();

            // Don't use inactive meshes
            if (current == null || !current.enabled)
            {
                continue;
            }

            NewMeshInstantiate.MeshInstance currInst = new NewMeshInstantiate.MeshInstance{};
            currInst.mesh = mfilter.sharedMesh;
            currInst.transform = pos * mfilter.transform.localToWorldMatrix;

            // Don't use inactive meshes
            if (currInst.mesh == null)
            {
                continue;
            }

            Material[] currmaterial = current.sharedMaterials;

            // The mesh is active & valid, let's merge it into the parent
            for (int loop = 0; loop < currmaterial.Length; ++loop)
            {
                currInst.subMeshIndex = Math.Min(loop, currInst.mesh.subMeshCount - 1);
                List<NewMeshInstantiate.MeshInstance> objects;

                if (!materialToMesh.TryGetValue(currmaterial[loop], out objects))
                {
                    objects = new List<NewMeshInstantiate.MeshInstance>();
                    materialToMesh.Add(currmaterial[loop], objects);
                }

                objects.Add(currInst);
            }

            // Trash removal logic
            if (destroyAfterwards && combineOnStart && Application.isPlaying)
            {
                Destroy(current.gameObject);
            }

            else if (destroyAfterwards)
            {
                DestroyImmediate(current.gameObject);
            }

            else
            {
                current.enabled = false;
            }
        }

        int meshIndex = 0;

        // Assign the new mesh instances
        foreach (KeyValuePair<Material, List<NewMeshInstantiate.MeshInstance>> de in materialToMesh)
        {
            foreach (NewMeshInstantiate.MeshInstance currInst in de.Value)
            {
                currInst.targetIndex = meshIndex;
            }

            ++meshIndex;
        }

        // Get its components
        if (!GetComponent<MeshFilter>())
        {
            gameObject.AddComponent<MeshFilter>();
        }

        MeshFilter filter = GetComponent<MeshFilter>();

        if (!GetComponent<MeshRenderer>())
        {
            gameObject.AddComponent<MeshRenderer>();
        }

        Renderer renderer = GetComponent<MeshRenderer>();

        // Apply the changes in-game
        Mesh final = NewMeshInstantiate.CombinePrep(materialToMesh.SelectMany(kvp => kvp.Value), generateTriangles);
        final.name = combineName;

        if (Application.isPlaying)
        {
            filter.mesh = final;
        }

        else
        {
            filter.sharedMesh = final;
        }

        renderer.materials = materialToMesh.Keys.ToArray();
        renderer.enabled = true;

        // Got through the optional steps
        if (addMeshCollider)
        {
            gameObject.AddComponent<MeshCollider>();
        }

        renderer.castShadows = castShadow;
        renderer.receiveShadows = getShadows;
    }
}