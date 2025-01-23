using UnityEngine;
using System.Diagnostics;

public class CustomQuad : MonoBehaviour
{
    public float width = 1f;  // Desired width
    public float height = 1f; // Desired height
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            Mesh mesh = meshFilter.mesh;

            Vector3[] vertices = mesh.vertices;
            vertices[0] = new Vector3(-width / 2, -height / 2, 0); // Bottom-left
            vertices[1] = new Vector3(width / 2, -height / 2, 0);  // Bottom-right
            vertices[2] = new Vector3(-width / 2, height / 2, 0);  // Top-left
            vertices[3] = new Vector3(width / 2, height / 2, 0);   // Top-right

            mesh.vertices = vertices;
            mesh.RecalculateBounds();
        }
    }
}
