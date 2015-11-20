/*********************************
 * NewMeshInstantiate.cs
 * Troy
 * Created 10/16/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class NewMeshInstantiate : MonoBehaviour
{

    private List<Color> colourList = new List<Color>();
    private List<Vector3> meshNormals = new List<Vector3>();
    private Dictionary<int, List<int>> subMeshList = new Dictionary<int, List<int>>();
    private Dictionary<int, List<int>> triangleList = new Dictionary<int, List<int>>();

    private List<Vector2> uv = new List<Vector2>();
    //private List<Vector2> uv2 = new List<Vector2>();
    private List<Vector3> vertexList = new List<Vector3>();
    private List<Vector4> tangentList = new List<Vector4>();
    private bool isTriangles;

    // I guess this is how classes within classes works?
    #region Nested type: MeshInstance

    public class MeshInstance
    {
        public Mesh mesh;
        public int subMeshIndex;
        public int targetIndex;
        public Matrix4x4 transform;
    }

    #endregion

    public void NewMeshInst(bool trianglesOrList)
    {
        // Are we using just triangles or were they assorted into a List?
        isTriangles = trianglesOrList;
    }
    
    public void VertexStart(int vertices)
    {
        // Determine how many vertices CombineMeshes would've missed
        int toPlug = vertices - (vertexList.Capacity - vertexList.Count);

        // Plug them in manually
        colourList.Capacity += toPlug;
        meshNormals.Capacity += toPlug;
        uv.Capacity += toPlug;
        //uv2.Capacity += toPlug;
        vertexList.Capacity += toPlug;
        tangentList.Capacity += toPlug;
    }

    public void TriangleStart(int targetIndex, int indices)
    {
        // Don't add triangles if we're not using a triangle list!
        if (!triangleList.ContainsKey(targetIndex))
        {
            triangleList.Add(targetIndex, new List<int>());
        }

        // Determine how many triangles CombineMeshes would've missed and plug them in
        int toPlug = indices - (triangleList[targetIndex].Capacity - triangleList[targetIndex].Count);
        triangleList[targetIndex].Capacity += toPlug;
    }

    public void AddMeshStart(int targetIndex, IEnumerable<int> lengthList)
    {
        // Don't add meshs if we're not using a triangle list!
        if (!subMeshList.ContainsKey(targetIndex))
        {
            subMeshList.Add(targetIndex, new List<int>());
        }

        // This is how much space we're using now.  This could very well change!
        int requiredCapacity = subMeshList[targetIndex].Count;

        foreach (int srcLength in lengthList)
        {
            int adjLength = srcLength;

            // Determine the length of the meshes we're adding in memory size and add them
            if (requiredCapacity > 0)
            {
                if ((requiredCapacity & 1) == 1)
                {
                    adjLength += 3;
                }
                else
                {
                    adjLength += 2;
                }
            }

            requiredCapacity += adjLength;
        }

        // Don't undercut how much space we need in our subMeshList if additions were made
        if (subMeshList[targetIndex].Capacity < requiredCapacity)
        {
            subMeshList[targetIndex].Capacity = requiredCapacity;
        }
    }

    public void AddSingleMesh(MeshInstance instance)
    {
        // The number of vertices is needed to add the mesh.  This is a little faster than calling Count multiple times
        int vertexCount = vertexList.Count;

        VertexStart(instance.mesh.vertexCount);

        // Get the mesh's info
        uv.AddRange(instance.mesh.uv);
        //uv2.AddRange(instance.mesh.uv2);
        colourList.AddRange(instance.mesh.colors);

        vertexList.AddRange(instance.mesh.vertices.Select(verts => instance.transform.MultiplyPoint(verts)));
        meshNormals.AddRange(instance.mesh.normals.Select(norms => instance.transform.inverse.transpose.MultiplyVector(norms).normalized));
        tangentList.AddRange(instance.mesh.tangents.Select(tans => // More than a function call!  See within the breackets below
        {
            Vector3 pos = new Vector3(tans.x, tans.y, tans.z);
            pos = instance.transform.inverse.transpose.MultiplyVector(pos).normalized;
            return new Vector4(pos.x, pos.y, pos.z, tans.w);
        }));

        // If there's no triangle list, then we're relying more on our subMesh list
        if (isTriangles)
        {
            int[] input = instance.mesh.GetTriangles(instance.subMeshIndex);
            AddMeshStart(instance.targetIndex, new[] { input.Length });
            List<int> output = subMeshList[instance.targetIndex];

            // Add mesh if one is there
            if (output.Count != 0)
            {
                output.Add(output[output.Count - 1]);
                output.Add(input[0] + vertexCount);

                // Some meshes take up a little extra space
                if ((output.Count & 1) == 1)
                {
                    output.Add(input[0] + vertexCount);
                }
            }

            // Adjust mesh vertices
            output.AddRange(input.Select(verts => verts + vertexCount));
        }
        else // Just use the triangle list
        {
            int[] trianglesToAdd = instance.mesh.GetTriangles(instance.subMeshIndex);
            TriangleStart(instance.targetIndex, trianglesToAdd.Length);
            triangleList[instance.targetIndex].AddRange(trianglesToAdd.Select(tris => tris + vertexCount));
        }
    }

    public void AddMultipleMeshes(IEnumerable<MeshInstance> instances)
    {
        // We can't just simply call AddSingleMesh multiple times.  We have to take into account that there could be a mix of differing vertexLists & triangleLists
        instances = instances.Where(instance => instance.mesh);
        VertexStart(instances.Sum(instance => instance.mesh.vertexCount));

        foreach (IGrouping<int, MeshInstance> subMeshIterator in instances.GroupBy(instance => instance.targetIndex))
        {
            if (isTriangles)
            {
                AddMeshStart(subMeshIterator.Key, subMeshIterator.Select(instance => instance.mesh.GetTriangles(instance.subMeshIndex).Length));
            }
            else
            {
                TriangleStart(subMeshIterator.Key, subMeshIterator.Sum(instance => instance.mesh.GetTriangles(instance.subMeshIndex).Length));
            }
        }

        foreach (MeshInstance instance in instances)
        {
            AddSingleMesh(instance);
        }
    }

    public Mesh CreateCombine()
    {
        // Define the mesh we'll end up making
        Mesh mesh = new Mesh
        {
            name = "Combine",
            vertices = vertexList.ToArray(),
            normals = meshNormals.ToArray(),
            colors = colourList.ToArray(),
            uv = uv.ToArray(),
            //uv2 = uv2.ToArray(),
            tangents = tangentList.ToArray(),
            subMeshCount = (isTriangles) ?subMeshList.Count : triangleList.Count
        };

        if (isTriangles)
        {
            foreach (var subMeshIterator in subMeshList)
                mesh.SetTriangles(subMeshIterator.Value.ToArray(), subMeshIterator.Key);
        }
        else
        {
            foreach (var subMeshIterator in triangleList)
                mesh.SetTriangles(subMeshIterator.Value.ToArray(), subMeshIterator.Key);
        }

        return mesh;
    }

    public static Mesh CombinePrep(GameObject toAttach, IEnumerable<MeshInstance> instances, bool trianglesOrList)
    {
        // This will create a prepared class object for combining once the user is ready
        NewMeshInstantiate processor = toAttach.AddComponent<NewMeshInstantiate>() as NewMeshInstantiate;
        processor.NewMeshInst(trianglesOrList);
        processor.AddMultipleMeshes(instances);
        return processor.CreateCombine();
    }
}