using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
    Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
    }


    void Update()
    {
        RaycastHit hit;
        if (!Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null)
            return;

        Mesh mesh = meshCollider.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];

        if (Input.GetMouseButtonDown(0)) {
            Mesh m = mesh;
            Vector3[] vert = vertices;
            vert[triangles[hit.triangleIndex * 3 + 0]] -= new Vector3(0, 1, 0);
            vert[triangles[hit.triangleIndex * 3 + 1]] -= new Vector3(0, 1, 0);
            vert[triangles[hit.triangleIndex * 3 + 2]] -= new Vector3(0, 1, 0);

            m.vertices = vert;

            meshCollider.gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        }

        Transform hitTransform = hit.collider.transform;
        p0 = hitTransform.TransformPoint(p0);
        p1 = hitTransform.TransformPoint(p1);
        p2 = hitTransform.TransformPoint(p2);
        Debug.DrawLine(p0, p1);
        Debug.DrawLine(p1, p2);
        Debug.DrawLine(p2, p0);

        
    }
}