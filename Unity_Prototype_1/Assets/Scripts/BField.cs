using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using Assets.Scripts;

public class BField : MonoBehaviour
{
    public GameObject magnetPrefab;
    public List<Magnet_class> magnets = new List<Magnet_class>();
    public GameObject arrowPrefab;
    public float min_radius_of_influence;
    public List<GameObject> Arrows = new List<GameObject>();
    public Vector3 field_size = new Vector3(3, 3, 2);
    public float arrow_gap = 0.13f;
    public float b_factor = 4;
    public float radius_of_influence = 0.4f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Magnet_class.Generate_magnet(magnetPrefab, magnets, new Vector3(-2, 1, 0), new Vector3(0, -20000, 0), 1.3E-3f); // generates the magnets
        Magnet_class.Generate_magnet(magnetPrefab, magnets, new Vector3(-2.2f, 1, 0), new Vector3(0, 6969, 0), 1.3E-3f);
        generate_field(field_size, arrow_gap, Arrows); //generates the field of arrows
        min_radius_of_influence = arrow_gap / 2;
        for (int i = 0; i < magnets.Count; i++)
        {
            magnets[i].new_pos();
            Vector3 close_point = magnets[i].closest_arrow(arrow_gap);
            float costheta_init = calculate_costheta(close_point,magnets[i].dipole_moment);
            magnets[i].max_B_field_value = calculate_b_field_magnitude(close_point.magnitude,costheta_init,magnets[i].dipole_moment.magnitude);
            // Debug.Log("close arrow: "+magnets[i].closest_arrow(arrow_gap)+"magnet: "+magnets[i].Magnet.transform.position);
            float B_field_factor = magnets[i].max_B_field_value/Mathf.Pow(30,3);

            

            float R1 = calculate_r_magnitude(B_field_factor,costheta_init,magnets[i].dipole_moment.magnitude)*2f;
            Debug.Log("max b field: "+magnets[i].max_B_field_value+" close point value:"+close_point+" R1: "+R1);
            magnets[i].Radius_of_influence = R1;
        }

        for (int i = 0; i < Arrows.Count; i++) //iterates through all arrows
        {
            Vector3 arrow_position = Arrows[i].transform.position;
            bool active = false;
            for (int j = 0; j < magnets.Count; j++)
            {
                float distance = Vector3.Distance(magnets[j].MagnetPosition, arrow_position);
                if (distance < magnets[j].Radius_of_influence && distance > min_radius_of_influence)
                {
                    active = true;
                    break;
                }
            }
            if (active)
            {
                Vector3 b_field = calculate_b_field(arrow_position);
                Vector3 r = magnets[0].calculate_r(arrow_position);
                float costheta = calculate_costheta(r,magnets[0].dipole_moment);
                Debug.Log("comparison="+b_field.magnitude+" & "+calculate_b_field_magnitude(r.magnitude,costheta,magnets[0].dipole_moment.magnitude)+
                " r value: "+ r);
                // Debug.Log(b_field);
                Arrows[i].transform.rotation = Quaternion.LookRotation(b_factor * b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range
                Arrows[i].SetActive(true);

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
        for (int i = 0; i < magnets.Count; i++)
        {
            if (magnets[i].MagnetPosition != magnets[i].Magnet.transform.position)
            {
                has_moved = true;
                magnets[i].new_pos();
            }
        }
        if (has_moved)
        {
            for (int i = 0; i < Arrows.Count; i++)
            {
                Vector3 arrow_position = Arrows[i].transform.position;
                bool active = false;
                for (int j = 0; j < magnets.Count; j++)
                {
                    float distance = Vector3.Distance(magnets[j].MagnetPosition, arrow_position);
                    if (distance < magnets[j].Radius_of_influence && distance > min_radius_of_influence)
                    {
                        active = true;
                        break;
                    }


                }
                if (active)
                {
                    Vector3 b_field = calculate_b_field(arrow_position);
                    Vector3 r = magnets[0].calculate_r(arrow_position);
                    float cost = calculate_costheta(r,magnets[0].dipole_moment);
                    float b_field_value = calculate_b_field_magnitude(r.magnitude,cost,magnets[0].dipole_moment.magnitude);
                    
                    Arrows[i].transform.rotation = Quaternion.LookRotation(b_factor * b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range 
                    Arrows[i].SetActive(true);
                    
                    float colorscale = b_field.magnitude / magnets[0].max_B_field_value;
                    // Debug.Log("magnitude_1: "+b_field_value+"   magnitude_2: "+b_field.magnitude+"  max_b_field"+magnets[0].max_B_field_value);
                    if (colorscale > 1)
                    {
                        colorscale = 1f;
                    }
                    Arrows[i].GetComponent<MeshRenderer>().material.color = new Color(1, 1 - colorscale, 0, colorscale);
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
        for (int i = 0; i < magnets.Count; i++)
        {
            Vector3 vector_distance = new Vector3(arrow_pos.x - magnets[i].MagnetPosition.x,
                                                  arrow_pos.y - magnets[i].MagnetPosition.y,
                                                  arrow_pos.z - magnets[i].MagnetPosition.z).normalized;
            resultant_b_field += 1e-7f * (3 * (Vector3.Dot(magnets[i].dipole_moment, vector_distance)) * vector_distance - magnets[i].dipole_moment)
                                            / Mathf.Pow(Vector3.Distance(magnets[i].MagnetPosition, arrow_pos), 3);

        }

        return resultant_b_field;

    }

    float calculate_costheta(Vector3 r,Vector3 m)
    {
        float costheta = Vector3.Dot(r,m)/(r.magnitude*m.magnitude);
        return costheta;
    }
    float calculate_b_field_magnitude(float r,float costheta,float m)
    {
        return (1e-7f*m*(Mathf.Sqrt(1+3*Mathf.Pow(costheta,2))))/ Mathf.Pow(r,3);
    }
    float calculate_r_magnitude(float B,float costheta,float m)
    {
        return Mathf.Pow(1e-7f*m*Mathf.Sqrt(1+3*Mathf.Pow(costheta,2)/B),1f/3f);
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