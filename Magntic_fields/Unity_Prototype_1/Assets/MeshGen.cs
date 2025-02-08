using UnityEngine;

public class MeshGen : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mesh = new Mesh(); 
        GetComponent<MeshFilter>().mesh = mesh;

        CreateArrow();
        UpdateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateArrow()
    {
        vertices = new Vector3[] {
        new Vector3 (0,0,0), new Vector3 (0,0,1), new Vector3 (1,0,0)
        };//sets starting coordinates of mesh vertices relative to each other

        triangles = new int[]
        {
            0,1,2
        };//sets clockwise order for triangle to be made (e.g 0 is the first element in vertices)
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();//lets lighting dircetion change the matarial. might remove
    }
}
