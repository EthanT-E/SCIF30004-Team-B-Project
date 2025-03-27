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
    public GameObject Scanner;

    float MAX_B = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generate_field(field_size, arrow_gap, Arrows); //generates the field of arrows
        min_radius_of_influence = arrow_gap / 2;
        for (int i = 0; i < magnets.Count; i++)
        {
            magnets[i].new_pos();
            magnets[i] = radius(magnets[i],arrow_gap,20);//initialize the radius of influence and maximum B field value
        }

        for (int i = 0; i < Arrows.Count; i++) //iterates through all arrows
        {
            Vector3 arrow_position = Arrows[i].transform.position;
            bool active = false;
            for (int j = 0; j < magnets.Count; j++)
            {
                float distance = Vector3.Distance(magnets[j].MagnetPosition, arrow_position);
                if ((distance < magnets[j].Radius_of_influence) && (distance > min_radius_of_influence))//check whther the arrow is within the radius of influence
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
        bool change = false; //checks for any change to UI or magnets
        if (magnets.Count ==0)
        {
            change = true;
        }
        for (int i = 0; i < magnets.Count; i++)
        {
            if (magnets[i].MagnetPosition != magnets[i].Magnet.transform.position)
            {
                change = true;
                magnets[i].new_pos();
            }
            if (magnets[i].UI_value_change)
            {
                change = true;
            }
            if (magnets[i].MagnetRotation != magnets[i].Magnet.transform.rotation)
            {
                change = true;
                magnets[i].new_rot();
                magnets[i].update_dipole();
            }
        }
        if (change) //updates bfield and arrows if there has been a change
        {
            for (int i = 0; i < Arrows.Count; i++)
            {
                Vector3 arrow_position = Arrows[i].transform.position;
                bool active = false;
                for (int j = 0; j < magnets.Count; j++)
                {
                    magnets[j] = radius(magnets[j],arrow_gap,20);//updating the radius of influence
                    if (magnets[j].max_B_field_value>MAX_B){
                        MAX_B = magnets[j].max_B_field_value;//finding the maximum B field value amoung all magnets
                    }

                    float distance = Vector3.Distance(magnets[j].MagnetPosition, arrow_position);
                    if ((distance < magnets[j].Radius_of_influence) && (distance > min_radius_of_influence))//check whther the arrow is within the radius of influence
                    {
                        active = true;
                        break;
                    }
                }
                if (active)
                {
                    Vector3 b_field = calculate_b_field(arrow_position);
                    Arrows[i].transform.rotation = Quaternion.LookRotation(b_field); // gets arrow to point in b direction. increase the coeffeient also increases the effective range 
                    Arrows[i].SetActive(true); //Enable arrow rendering and interactability
                    
                    float colorscale = b_field.magnitude / MAX_B;
                    if (colorscale > 1)//set colorscale to 1 if it is larger than 1 (it can get bigger than 1 if it is closer than the selected close point)
                    {
                        colorscale = 1f;
                    }
                    Arrows[i].GetComponent<MeshRenderer>().material.color = new Color(1, 1 - colorscale, 0, colorscale);//change the color of magnet depends on the colorscale
                }
                else
                {
                    Arrows[i].SetActive(false); //Disable arrow rendering and interactability
                }
            }
            // Calculating force and torque exerted on all magnets
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
                torque_on_i = Calculate_magnetic_torque(magnets[i], calculate_b_field_except_self(magnets[i].MagnetPosition, i));

                // ith magnet experiences torque and force.
                magnets[i].influence_force(force_on_i); 
                magnets[i].influence_torque(torque_on_i);
            }
        }
    }


    /**
    /* Calculates Magnetic Torque of magnet.
    /* Is simply the cross product between magnet own dipole and resultant magnetic field.
    /* @param[in] magnet - Magnet_class - Magnet_class - Magnet to apply torque to
    /* @param[in] b_field - Magnet_class - Resultant B-Field to cross product with
    /* @return Vector3 cross product vector between magnet magnetic dipole and res B field
    */
    Vector3 Calculate_magnetic_torque(Magnet_class magnet, Vector3 b_field)
    {
        return Vector3.Cross(magnet.dipole_moment, b_field);
    }

    /**
    /* Calculates force vector between two magnets
    /* Is dependent on magnet properties encased in Magnet_class
    /* @param[in] magnet1 - Magnet_class - magnet to calculate force with
    /* @param[in] magnet2 - Magnet_class - magnet to calculate force with
    /* @return - Vector3 Force vector between two magnets
    */
    Vector3 Calculate_force_vector(Magnet_class magnet1, Magnet_class magnet2)
    {
        Vector3 dist = magnet1.MagnetPosition - magnet2.MagnetPosition; // distance between two magnets
        Vector3 dist_norm = dist.normalized; // calculates unit vector

        // store dipole moments
        Vector3 dipole_moment1 = magnet1.dipole_moment;
        Vector3 dipole_moment2 = magnet2.dipole_moment;

        // compute dot products for all quantities needed
        float dip1_dot_dip2 = Vector3.Dot(dipole_moment1, dipole_moment2);
        float distn_dot_dip1 = Vector3.Dot(dist_norm, dipole_moment1);
        float distn_dot_dip2 = Vector3.Dot(dist_norm, dipole_moment2);

        // Compute force between two magnets using big equation.
        Vector3 force = (3 * 1 * dipole_moment1.magnitude * dipole_moment2.magnitude / (4 * Mathf.PI * Mathf.Pow(dist.magnitude, 4))) *
            ((dist_norm * dip1_dot_dip2) + (dipole_moment1 * distn_dot_dip1) + (dipole_moment2 * distn_dot_dip2) - (5 * dist_norm * distn_dot_dip1 * distn_dot_dip2));

        return force;
    }

    /**
    /* Calculates resultant magnetic field at position
    /* @param[in] arrow_pos - Vector3 - position vector to calculate at
    */
    public Vector3 calculate_b_field(Vector3 arrow_pos)
    {
        Vector3 resultant_b_field = Vector3.zero;
        for (int i = 0; i < magnets.Count; i++)
        {
            Vector3 vector_distance = new Vector3(arrow_pos.x - magnets[i].MagnetPosition.x,
                                                  arrow_pos.y - magnets[i].MagnetPosition.y,
                                                  arrow_pos.z - magnets[i].MagnetPosition.z).normalized;
            //equation:        ~(μ₀/4π)·(                                         m·r                                )×r - m 
            //                                                                  / |r|³
            resultant_b_field += 1e-7f * (3 * (Vector3.Dot(magnets[i].dipole_moment, vector_distance)) * vector_distance - magnets[i].dipole_moment)
                                            / Mathf.Pow(Vector3.Distance(magnets[i].MagnetPosition, arrow_pos), 3);
        }

        return resultant_b_field;
    }

    /**
    /* Calculates resultant magnetic field exerted on magnet dipile
    /* Only needed to calculate magnetic torque
    /* @param[in] mag_pos - Vector3 - position vector of magnet
    /* @param[in] ignore_index - int - index of magnet within the magnet 
    /* @param[in] Vector3 of resultant magnetic field surrounding excluded magnet
    */
    Vector3 calculate_b_field_except_self(Vector3 mag_pos, int ignore_index)
    {
        Vector3 resultant_b_field = Vector3.zero;
        for (int i = 0; i < magnets.Count; i++)
        {
            if (i != ignore_index)
            {
                Vector3 vector_distance = new Vector3(mag_pos.x - magnets[i].MagnetPosition.x,
                                                    mag_pos.y - magnets[i].MagnetPosition.y,
                                                    mag_pos.z - magnets[i].MagnetPosition.z).normalized;
                resultant_b_field += 1e-7f * (3 * (Vector3.Dot(magnets[i].dipole_moment, vector_distance)) * vector_distance - magnets[i].dipole_moment)
                                                / Mathf.Pow(Vector3.Distance(magnets[i].MagnetPosition, mag_pos), 3);
            }
        }

        return resultant_b_field;

    }

    float calculate_costheta(Vector3 r,Vector3 m)//function for calculating the cos(theta)
    {
        float costheta = Vector3.Dot(r,m)/(r.magnitude*m.magnitude);
        return costheta;
    }
    float calculate_b_field_magnitude(float r,float costheta,float m)//function for calculating the b field value for single magnet using the cos(theta)
    {
        return (1e-7f*m*(Mathf.Sqrt(1+3*Mathf.Pow(costheta,2))))/ Mathf.Pow(r,3);
    }
    float calculate_r_magnitude(float B,float costheta,float m)//funciton for calculating the corresponding distance at certain value of B field
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
                    // XRSimpleInteractable scanInteractor = Scanner.GetComponent("XRSimpleInteractable") as XRSimpleInteractable;
                    // scanInteractor.selectEntered.AddListener(scan);
                    Arrows.Add(arrow); //Adds arrow to list of arrows
                }
            }
        }
    }
    
    Magnet_class radius(Magnet_class magnets, float arrow_gap, int radius_factor)//finding the maximum B-field value and using it to find a radius of influence for each magnet
    {
            Vector3 close_point = magnets.closest_arrow(arrow_gap);//get the position of a point that is close to the magnet
            float costheta_init = calculate_costheta(close_point,magnets.dipole_moment);//calculating cos(theta) for calculation
            magnets.max_B_field_value = calculate_b_field_magnitude(close_point.magnitude,costheta_init,magnets.dipole_moment.magnitude);//finding the maximum B-field value
            float B_field_factor = magnets.max_B_field_value/Mathf.Pow(radius_factor,3);
            
            

            float R1 = calculate_r_magnitude(B_field_factor,costheta_init,magnets.dipole_moment.magnitude)*1.5f;//calculate the radius of influence
            magnets.Radius_of_influence = R1;//change the radius of influence for each magnet
            return magnets;
    }

    /**
    /* Scans magnetic field at arrow
    /* Currently unused previous implementation of the scanner
       see generate_field for how to set the arrows to use this function when selected by the near-far interactor
    /* @param[in] args - SelectEnterEventArgs - argument handled/provided by Unity's event system, ignore at the programmers' end
    */
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
