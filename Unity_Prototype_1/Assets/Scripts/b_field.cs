using UnityEngine;
using System.Collections.Generic;

public class b_field : MonoBehaviour
{
    public GameObject magnet;
    // public GameObject[] all_points;
    Vector3 magnet_position;
    Vector3 point_position;
    public GameObject arrowPrefab;
    public List<GameObject> Arrows = new List<GameObject>();
    public Vector3 field_size = new Vector3(1,1,1);
    public float arrow_gap = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (float x = -field_size.x; x <= field_size.x; x+= arrow_gap)
        {
            for (float y = -field_size.y; y <= field_size.y; y+= arrow_gap)
            {
                for (float z = -field_size.z; z <= field_size.z; z+= arrow_gap)
                {
                    GameObject arrow = Instantiate(arrowPrefab,transform);
                    arrow.transform.position = new Vector3(x,y,z);
                    Arrows.Add(arrow);
                }
            }
        }
        // all_points = new GameObject[magnet.transform.childCount];
        magnet_position = magnet.transform.position;
        // for (int i=0; i<all_points.Length;i++)
        // {
        //     all_points[i] = magnet.transform.GetChild(i).gameObject;
        //     point_position = all_points[i].transform.localPosition;
        //     Vector3 b_field = calculate_b_field(magnet_position, point_position,all_points[i]);
        //     Debug.Log(b_field.x + ", " + b_field.y + ", " + b_field.z);
        //     all_points[i].transform.rotation = Quaternion.LookRotation(1000*b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range
        // }
        for (int i=0; i<Arrows.Count;i++)
        {
            point_position = Arrows[i].transform.position;
            Vector3 b_field = calculate_b_field(magnet_position, point_position,Arrows[i]);
            Debug.Log(b_field.x + ", " + b_field.y + ", " + b_field.z);
            Arrows[i].transform.rotation = Quaternion.LookRotation(1000*b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range
        }
    }

    // Update is called once per frame
    void Update()
    {
        // all_points = new GameObject[magnet.transform.childCount];
        magnet_position = magnet.transform.position;
        // for (int i=0; i<all_points.Length;i++)
        // {
        //     all_points[i] = magnet.transform.GetChild(i).gameObject;
        //     if(magnet_position != magnet.transform.position)
        //     {
        //     all_points[i] = magnet.transform.GetChild(i).gameObject;
        //     point_position = all_points[i].transform.localPosition;
        //     Vector3 b_field = calculate_b_field(magnet_position, point_position,all_points[i]);
        //     Debug.Log(b_field.x + ", " + b_field.y + ", " + b_field.z);
        //     all_points[i].transform.rotation = Quaternion.LookRotation(b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range
        //     }
        // }
        for (int i=0; i<Arrows.Count;i++)
        {
            // if(magnet_position != magnet.transform.position)
            // {
            point_position = Arrows[i].transform.position;
            Vector3 b_field = calculate_b_field(magnet_position, point_position,Arrows[i]);
            Debug.Log(b_field.x + ", " + b_field.y + ", " + b_field.z);
            Arrows[i].transform.rotation = Quaternion.LookRotation(1000*b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range
            // }   
        }
    }

    Vector3 calculate_b_field(Vector3 magnet_pos, Vector3 point_pos, GameObject point)
    {
        Vector3.Dot(magnet_pos, point_pos);
        Vector3 dipole_moment = new Vector3(1, 2, 3);
        Vector3 vector_distance = new Vector3(point_pos.x - magnet_pos.x,
                                              point_pos.y - magnet_pos.y,
                                              point_pos.z - magnet_pos.z).normalized;
        float distance = Vector3.Distance(magnet.transform.position, point.transform.position);
        return( (float)1e-7*(3 * (Vector3.Dot(dipole_moment, vector_distance)) * vector_distance - dipole_moment)
                                                                                            / Mathf.Pow(distance, 3));
    }
}