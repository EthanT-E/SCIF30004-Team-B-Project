using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using Assets.Scripts;
using TMPro;
using Unity.VisualScripting;

public class BField : MonoBehaviour
{
    public GameObject magnetPrefab;
    public List<Magnet_class> magnets = new List<Magnet_class>();
    public GameObject arrowPrefab;
    public float min_radius_of_influence;
    public List<GameObject> Arrows = new List<GameObject>();
    public Vector3 field_size = new Vector3(3, 3, 2);
    public float arrow_gap = 0.15f;
    public float radius_of_influence = 0.4f;
    public float max_B_field_value = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Magnet_class.Generate_magnet(magnetPrefab, magnets, new Vector3(-2, 1, 0), new Vector3(5,0,0), 1); // generates the magnets
        Magnet_class.Generate_magnet(magnetPrefab, magnets, new Vector3(-2.2f, 1, 0), new Vector3(-5,0,0), 1);

        generate_field(field_size, arrow_gap, Arrows); //generates the field of arrows
        min_radius_of_influence = arrow_gap / 2;
        for (int i = 0; i < Arrows.Count; i++) //iterates through all arrows
        {
            Vector3 arrow_position = Arrows[i].transform.position;
            bool active = false;
            for (int j = 0; j < magnets.Count; j++)
            {
                float distance = Vector3.Distance(magnets[j].MagnetPosition, arrow_position);
                if (distance < radius_of_influence && distance > min_radius_of_influence)
                {
                    active = true;
                    break;
                }
            }
            if (active)
            {
                Vector3 b_field = calculate_b_field(arrow_position);

                Arrows[i].transform.rotation = Quaternion.LookRotation(b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range
                Arrows[i].SetActive(true);
                if (b_field.magnitude > max_B_field_value)
                {
                    max_B_field_value = b_field.magnitude;
                }
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
        bool change = false;
        for (int i = 0; i < magnets.Count; i++)
        {
            if (magnets[i].MagnetPosition != magnets[i].Magnet.transform.position)
            {
                change = true;
                magnets[i].new_pos();
            }
            if(magnets[i].UI_value_change)
            {
                change = true;
            }
            if (magnets[i].MagnetRotation!=magnets[i].Magnet.transform.rotation)
            {
                change = true;
                magnets[i].new_rot();
                magnets[i].update_dipole();
            }
        }
        if (change)
        {
            for (int i = 0; i < Arrows.Count; i++)
            {
                Vector3 arrow_position = Arrows[i].transform.position;
                bool active = false;
                for (int j = 0; j < magnets.Count; j++)
                {
                    float distance = Vector3.Distance(magnets[j].MagnetPosition, arrow_position);
                    if (distance < radius_of_influence && distance > min_radius_of_influence)
                    {
                        active = true;
                        break;
                    }


                }
                if (active)
                {
                    Vector3 b_field = calculate_b_field(arrow_position);
                    Arrows[i].transform.rotation = Quaternion.LookRotation(b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range 
                    Arrows[i].SetActive(true);

                    float colorscale = 60 * (b_field.magnitude / max_B_field_value);
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
            Vector3 force_on_i = Vector3.zero;
            Vector3 torque_on_i = Vector3.zero;
            for (int i = 0; i < magnets.Count; i++)
            {
                force_on_i = Vector3.zero;
                torque_on_i = Vector3.zero;
                for (int j = 0; j < magnets.Count; j++)
                {
                    if (i != j)
                        force_on_i += Calculate_force_vector(magnets[i], magnets[j]);
                        
                }
                torque_on_i = Calculate_magnetic_torque(magnets[i], force_on_i);
                Debug.Log($"torque on magnet {i} - {torque_on_i.x},{torque_on_i.y},{torque_on_i.z}");
                
                magnets[i].influence_force(force_on_i);
                magnets[i].influence_torque(torque_on_i);
                
            }
        }
    }
    

   Vector3 Calculate_magnetic_torque(Magnet_class magnet, Vector3 b_field)
    {
        return Vector3.Cross(magnet.dipole_moment, b_field);
    }

   Vector3 Calculate_force_vector(Magnet_class magnet1, Magnet_class magnet2)
    {
        Vector3 dist = magnet1.MagnetPosition - magnet2.MagnetPosition;
        Vector3 dist_norm = dist.normalized;

        Vector3 dipole_moment1 = magnet1.dipole_moment;
        Vector3 dipole_moment2 = magnet2.dipole_moment;

        float dip1_dot_dip2 = Vector3.Dot(dipole_moment1, dipole_moment2);
        float distn_dot_dip1 = Vector3.Dot(dist_norm, dipole_moment1);
        float distn_dot_dip2 = Vector3.Dot(dist_norm, dipole_moment2);

        Vector3 force = (3 * 1 * dipole_moment1.magnitude * dipole_moment2.magnitude / (4 * Mathf.PI * Mathf.Pow(dist.magnitude, 4))) *
            ((dist_norm * dip1_dot_dip2) + (dipole_moment1 * distn_dot_dip1) + (dipole_moment2 * distn_dot_dip2) - (5 * dist_norm * distn_dot_dip1 * distn_dot_dip2));

        return force;
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
        args.interactorObject.transform.parent.Find("ScannerUI").Find("BFieldVal").GetComponent<TMP_Text>().text = string.Format("{0}\n{1}\n{2}", b_field.x, b_field.y, b_field.z);
        args.interactorObject.transform.parent.Find("ScannerUI").Find("BFieldMag").GetComponent<TMP_Text>().text = string.Format("Magnitude: {0}", Vector3.Magnitude(b_field));
        Vector3 direction = args.interactableObject.transform.eulerAngles;
        args.interactorObject.transform.parent.Find("ScannerUI").Find("BFieldAngle").GetComponent<TMP_Text>().text = string.Format("{0:000}�\n{1:000}�\n{2:000}�", 
                                                                                                    Mathf.Round(direction.x), Mathf.Round(direction.y), Mathf.Round(direction.z));
    }
}