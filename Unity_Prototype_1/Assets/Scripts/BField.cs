using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class BField : MonoBehaviour
{
    public GameObject magnetPrefab;
    public List<GameObject> Magnets = new List<GameObject>();
    List<Vector3> magnet_positions = new List<Vector3>();
    public GameObject arrowPrefab;
    public float min_radius_of_influence;
    public List<GameObject> Arrows = new List<GameObject>();
    public Vector3 field_size = new Vector3(3,3,2);
    public float arrow_gap = 0.15f;
    public float b_factor = 4;
    public float radius_of_influence = 0.4f;
    public float max_B_field_value = 0f;
    public Vector3 dipole_moment = new Vector3(1, 5, 1);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generate_magnets(); // generates the magnets
        generate_field(field_size,arrow_gap,Arrows); //generates the field of arrows
        min_radius_of_influence = arrow_gap/2;
        for (int i=0; i<Arrows.Count;i++) //iterates through all arrows
        {
            Vector3 arrow_position = Arrows[i].transform.position;
            bool active = false;
            for (int j = 0; j < magnet_positions.Count; j++)
            {
                float distance = Vector3.Distance(magnet_positions[j], arrow_position);
                if (distance < radius_of_influence && distance > min_radius_of_influence)
                {
                    active = true;
                    break;
                }
            }
            if (active)
            {
                Vector3 b_field = calculate_b_field(arrow_position);
                Arrows[i].transform.rotation = Quaternion.LookRotation(b_factor * b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range
                Arrows[i].SetActive(true);
                if(b_field.magnitude>max_B_field_value)
                    max_B_field_value = b_field.magnitude;
            }
            else
            {
                Arrows[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool has_moved = false;
        for (int i = 0; i < Magnets.Count; i++)
        {
            if (magnet_positions[i] != Magnets[i].transform.position)
            {
                has_moved = true;
                magnet_positions[i] = Magnets[i].transform.position;
            }
        }
        if(has_moved)
        {
            for (int i=0; i<Arrows.Count;i++)
            {
                Vector3 arrow_position = Arrows[i].transform.position;
                bool active = false;
                for (int j = 0; j < magnet_positions.Count; j++)
                {
                    float distance = Vector3.Distance(magnet_positions[j], arrow_position);
                    if (distance < radius_of_influence && distance > min_radius_of_influence)
                    {
                        active = true;
                        break;
                    }
                }
                if (active)
                {
                    Vector3 b_field = calculate_b_field(arrow_position);
                    Arrows[i].transform.rotation = Quaternion.LookRotation(b_factor * b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range 
                    Arrows[i].SetActive(true);
                    float colorscale = 60*(b_field.magnitude/max_B_field_value);
                    if(colorscale>1){
                        colorscale = 1f;
                    }
                    Arrows[i].GetComponent<MeshRenderer>().material.color = new Color(1,1-colorscale,0,colorscale);
                }
                else
                {
                    Arrows[i].SetActive(false);
                }
            } 
        }
    }

    Vector3 calculate_b_field(Vector3 arrow_pos)
    {
        Vector3 resultant_b_field = Vector3.zero;
        for (int i = 0; i < magnet_positions.Count; i++)
        {
            Vector3 vector_distance = new Vector3(arrow_pos.x - magnet_positions[i].x,
                                                  arrow_pos.y - magnet_positions[i].y,
                                                  arrow_pos.z - magnet_positions[i].z).normalized;
            resultant_b_field += 1e-7f * (3 * (Vector3.Dot(dipole_moment, vector_distance)) * vector_distance - dipole_moment)
                                            / Mathf.Pow(Vector3.Distance(magnet_positions[i], arrow_pos), 3);
        }
        return resultant_b_field;
    }

    void generate_magnets()
    {
        GameObject magnet = Instantiate(magnetPrefab);
        magnet.transform.position = new Vector3(-2,2,0);
        Magnets.Add(magnet);
        magnet_positions.Add(magnet.transform.position);

        GameObject magnet2 = Instantiate(magnetPrefab);
        magnet2.transform.position = new Vector3(-2,2,0);
        Magnets.Add(magnet2);
        magnet_positions.Add(magnet2.transform.position);
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
                    XRSimpleInteractable scanInteractor = arrow.GetComponent("XRSimpleInteractable") as XRSimpleInteractable;
                    scanInteractor.selectEntered.AddListener(scan);
                    Arrows.Add(arrow); //Adds arrow to list of arrows
                }
            }
        }
    }

    void scan(SelectEnterEventArgs args)
    {
        Vector3 b_field = calculate_b_field(args.interactableObject.transform.position);
        Debug.Log(b_field.x + ", " + b_field.y + ", " + b_field.z);
    }
}