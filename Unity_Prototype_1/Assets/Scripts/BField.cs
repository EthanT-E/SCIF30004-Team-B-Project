using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Linq;
using System.Runtime.CompilerServices;

public class BField : MonoBehaviour
{
    Vector3 magnet_position;
    Vector3 arrow_position;
    public GameObject magnet;
    public GameObject arrowPrefab;
    public List<GameObject> Arrows = new List<GameObject>();
    public Vector3 field_size = new Vector3(2, 2, 2);
    public float arrow_gap = 0.5f;
    public float b_factor = 1000000;

    public Vector3 resultant_b_field;
    public List<GameObject> magnets;
    int magnets_amount;
    List<Vector3> old_positions = new List<Vector3>();
    List<Vector3> new_positions = new List<Vector3>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generate_field(field_size, arrow_gap, Arrows); //generates the field of arrows
        Vector3 resultant_b_field = Vector3.zero;
        magnets.Add (magnet);
        magnets_amount = magnets.Count;
        old_positions.Add(magnet.transform.position);
        for (int i = 0; i < Arrows.Count; i++) //iterates through all arrows //equal to line 37-44
        {
            Arrows[i].SetActive(true);
            arrow_position = Arrows[i].transform.position;
            for (int j = 0; j < magnets.Count; j++)
                {
                    magnet_position = magnets[j].transform.position;
                    Vector3 b_field = calculate_b_field(magnet_position, arrow_position);
                    resultant_b_field += b_field;
                }
            Debug.Log(resultant_b_field.x + ", " + resultant_b_field.y + ", " + resultant_b_field.z);
            Arrows[i].transform.rotation = Quaternion.LookRotation(b_factor * resultant_b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        new_positions.Clear();
        for (int i = 0; i < magnets.Count; i++)
        {
            new_positions.Add(magnets[i].transform.position);
        }
        //
        if (old_positions.SequenceEqual(new_positions) == false)
        {
            Debug.Log(old_positions.Equals(new_positions));
            magnets_amount = magnets.Count;
            old_positions = new_positions;
            for (int i = 0; i < Arrows.Count; i++)
            {
                resultant_b_field = Vector3.zero;
                arrow_position = Arrows[i].transform.position;
                for (int j = 0; j < magnets.Count; j++)
                {
                    Debug.Log(magnets[j]);
                    magnet_position = magnets[j].transform.position;
                    Vector3 b_field = calculate_b_field(magnet_position, arrow_position);
                    resultant_b_field += b_field;
                    Debug.Log($"Magnet {j} -> Arrow {i}: B-Field = {resultant_b_field.x + ", " + resultant_b_field.y + ", " + resultant_b_field.z}");
                    
                }
                //Debug.Log("Arrow " + i +" " + resultant_b_field.x + ", " + resultant_b_field.y + ", " + resultant_b_field.z);
                Arrows[i].transform.rotation = Quaternion.LookRotation(b_factor * resultant_b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range 
            }
        }


    }

    Vector3 calculate_b_field(Vector3 magnet_pos, Vector3 arrow_pos)
    {
        Vector3.Dot(magnet_pos, arrow_pos);
        Vector3 dipole_moment = new Vector3(1, 2, 3);
        Vector3 vector_distance = new Vector3(arrow_pos.x - magnet_pos.x,
                                              arrow_pos.y - magnet_pos.y,
                                              arrow_pos.z - magnet_pos.z).normalized;
        float distance = Vector3.Distance(magnet_pos, arrow_pos);
        return ((float)1e-7 * (3 * (Vector3.Dot(dipole_moment, vector_distance)) * vector_distance - dipole_moment)
                                                                                            / Mathf.Pow(distance, 3));
    }

    void generate_field(Vector3 field_size, float arrow_gap, List<GameObject> Arrows)
    {
        // iterates over each axes to form a grid of points
        for (float x = -field_size.x; x <= field_size.x; x += arrow_gap)
        {
            for (float y = -field_size.y; y <= field_size.y; y += arrow_gap)
            {
                for (float z = -field_size.z; z <= field_size.z; z += arrow_gap)
                {
                    GameObject arrow = Instantiate(arrowPrefab, transform); //creates arrow
                    arrow.transform.position = new Vector3(x, y, z); //gives arrow coordinates corresponding to grid point
                    
                    Arrows.Add(arrow); //Adds arrow to list of arrows
                }
            }
        }
    }
}