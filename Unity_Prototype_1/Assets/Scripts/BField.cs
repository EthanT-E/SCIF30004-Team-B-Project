using UnityEngine;
using System.Collections.Generic;

public class BField : MonoBehaviour
{
    public GameObject magnet;
    public GameObject point;
    Vector3 magnet_position;
    Vector3 point_position;
    Vector3 resultant_b_field;
    public List<GameObject> magnets;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        magnet_position = magnet.transform.position;
        point_position = point.transform.position;
        Vector3 b_field = calculate_b_field(magnet_position, point_position);
        Vector3 resultant_b_field = new Vector3(0,0,0);
        Debug.Log(b_field.x + ", " + b_field.y + ", " + b_field.z);

        point.transform.rotation = Quaternion.LookRotation(1000*b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range
        magnets.Add(magnet);
    }

    // Update is called once per frame
    void Update()
    {
        if(magnet_position != magnet.transform.position || point_position != point.transform.position) // Eventually will add statement for new magnet added
        {
            resultant_b_field = new Vector3(0,0,0);
            foreach (var magnet in magnets)
            {
                magnet_position = magnet.transform.position;
                point_position = point.transform.position;
                Vector3 b_field = calculate_b_field(magnet_position, point_position);
                resultant_b_field += b_field;
            }
            Debug.Log(resultant_b_field.x + ", " + resultant_b_field.y + ", " + resultant_b_field.z);
            point.transform.rotation = Quaternion.LookRotation(1000*resultant_b_field);// gets arrow to point in b direction. increase the coeffeient also increases the effective range
        }
              
    }

    Vector3 calculate_b_field(Vector3 magnet_pos, Vector3 point_pos)
    {
        Vector3.Dot(magnet_pos, point_pos);
        Vector3 dipole_moment = new Vector3(1, 2, 3);
        Vector3 vector_distance = new Vector3(point_pos.x - magnet_pos.x,
                                              point_pos.y - magnet_pos.y,
                                              point_pos.z - magnet_pos.z).normalized;
        float distance = Vector3.Distance(magnet.transform.position, point.transform.position);
        return((float)1e-7 * (3 * (Vector3.Dot(dipole_moment, vector_distance)) * vector_distance - dipole_moment)
                                                                                            / Mathf.Pow(distance, 3));
    }
}