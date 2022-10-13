using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
//Attach to slope.
public class TriangularPrism : MonoBehaviour
{
    [SerializeField] private Material _mat;
    private const float root3 = 1.732051f;

    // Use this for initialization
    void Start()
    {
        var mesh = new Mesh();

        Vector3[] positions = new Vector3[] 
        {
            // top
            new Vector3 (1f, 0f, 0f),
            new Vector3 (1f, 0f, -1f),
            new Vector3 (1f, 1f, 0f),
            // bottom
            new Vector3 (0f, 0f, 0f),
            new Vector3 (0f, 0f, -1f),
            new Vector3 (0f, 1f, 0f),
        };

        int[] vertIndices = new int[]
        {
            0, 1, 2,
            3, 5, 4,

            1, 4, 5,
            1, 5, 2,
            5, 3, 2,
            2, 3, 0,
            3, 4, 0,
            0, 4, 1
        };

    Vector3[] vertices = new Vector3[vertIndices.Length];
    for (int i = 0; i < vertIndices.Length; i++)
    {
        vertices[i] = positions[vertIndices[i]];
    }
    mesh.vertices = vertices;

    int[] triangles = new int[mesh.vertices.Length];
    for (int i = 0; i < mesh.vertices.Length; i++)
    {
        triangles[i] = i;
    }
    mesh.triangles = triangles;

    Vector2[] uvSources = new Vector2[]{};
    Vector2[] uvs = new Vector2[uvSources.Length];
    for (int i = 0; i < uvSources.Length; i++)
    {
        uvs[i] = uvSources[i];
    }
    mesh.uv = uvs;

    mesh.RecalculateNormals();

    var filter = GetComponent<MeshFilter>();
    filter.sharedMesh = mesh;

    var renderer = GetComponent<MeshRenderer>();
    renderer.material = _mat;


    gameObject.AddComponent<MeshCollider>();
}
}