using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.AR;
using Unity.VisualScripting;

namespace Assets.Scripts
{

    public class MagnetClass : MonoBehaviour
    {
        public GameObject Magnet;
        public Vector3 magnet_position;
        public Quaternion magnet_rotation;
        public float magnetic_susceptibility;
        public Vector3 auxilary_field;
        public Vector3 dipole_moment;
        public bool UI_value_change = false;
        public bool holding=false;
        public Vector3 max_B_field;
        public float max_B_field_value;
        public float radius_of_influence;
        public MagnetClass(GameObject prefab, Vector3 start_pos, Vector3 iAux, float iSus = 1)
        {
            Magnet = Instantiate(prefab);
            Magnet.transform.position = start_pos;
            magnetic_susceptibility = iSus;
            auxilary_field = iAux;
            dipole_moment = Dipole_moment();
            
            Magnet.GetComponent<MagnetClass>().magnet_position = Magnet.transform.position;
            Magnet.GetComponent<MagnetClass>().magnetic_susceptibility = iSus;

            Magnet.GetComponent<MagnetClass>().auxilary_field = iAux;

            Magnet.GetComponent<MagnetClass>().dipole_moment = Dipole_moment();
            Debug.Log($"DIpole: {Magnet.GetComponent<MagnetClass>().dipole_moment.x},{Magnet.GetComponent<MagnetClass>().dipole_moment.y},{Magnet.GetComponent<MagnetClass>().dipole_moment.z}");
        }

        private Vector3 Dipole_moment()
        {
            return auxilary_field * magnetic_susceptibility;
        }

        public void set_suscept(float Ichi)
        {
            magnetic_susceptibility = Ichi;
            update_dipole();
        }

        public void set_auxiliary(Vector3 iAux)
        {
            auxilary_field = iAux;
            update_dipole();
        }

        public void new_pos()
        {
            magnet_position = Magnet.transform.position;
            Magnet.GetComponent<MagnetClass>().magnet_position = Magnet.transform.position;
        }

        public void new_rot()
        {
            magnet_rotation = Magnet.transform.rotation;
        }
        
        public void update_dipole() //updates dipole for magnet rotation
        {
            dipole_moment = Dipole_moment();
            dipole_moment =  magnet_rotation*dipole_moment;
        }

        /**
        /* Applies force onto itself
        /* @param[in] force - Vector3 - Force vector to exert onto self
        */
        public void influence_force(Vector3 force)
        {
            // Magnetic force was typiclly too strong. Introduced damping terms to prevent extreme chaos
            float scale_factor = 1.0f - Mathf.Exp(-force.magnitude / 1E10f);
            float adjust_magnitude = Mathf.Pow(force.magnitude, 0.65f);

            // Applies force onto self
            Magnet.GetComponent<Rigidbody>().AddForce(force.normalized * scale_factor * adjust_magnitude, ForceMode.Acceleration);
        }

        /**
        /* Applies torque onto itself
        /* @param[in] torque - Vector3 - Torque vector to exert onto self
        */
        public void influence_torque(Vector3 torque)
        {
            Magnet.GetComponent<Rigidbody>().AddTorque(torque.normalized * Mathf.Clamp(torque.magnitude, 0.0f, 1E5f), ForceMode.Acceleration);
        }

        public static void Generate_magnet(GameObject prefab, List<MagnetClass> Magnet_list, Vector3 start_pos, Vector3 iAux, float iSus = 1)
        {
            MagnetClass mag = new MagnetClass(prefab, start_pos, iAux, iSus);
            Magnet_list.Add(mag);
        }

        public static void Generate_magnet(GameObject prefab, List<MagnetClass> Magnet_list, Vector3 start_pos, float iSus = 1)
        {
            Vector3 DefaultAux = new Vector3(1, 0, 0);
            MagnetClass mag = new MagnetClass(prefab, start_pos, DefaultAux, iSus);
            Magnet_list.Add(mag);
        }


        /**
        /* Sends instance of itself to reciever component for later processing of MagneticVibration.cs
        /* @param[in] args - SelectEnterEventArgs - argument for GameObject that has grabbed magnet
        */
        public void Send_Self(SelectEnterEventArgs args)
        {
            if (args.interactorObject is NearFarInteractor controller) // Checks to make sure controller has grabbed self
            {
                RecieveAndSendToSystem reciever = controller.GetComponent<RecieveAndSendToSystem>(); // Store reciever component of controller
                Debug.Log($"DIpoe: {this.GetComponent<MagnetClass>().dipole_moment.x},{this.GetComponent<MagnetClass>().dipole_moment.y},{this.GetComponent<MagnetClass>().dipole_moment.z}");
                reciever.get_magnet(this.GetComponent<MagnetClass>()); // RecieveAndSendToSystem object Gets MagnetClass instance of self 
                ChangeTag holding = Magnet.GetComponent<ChangeTag>();
                holding.not_hovering();
            }
        }

        public Vector3 closest_arrow(float arrow_gap)
        {
        Vector3 closest_arrow = new Vector3(arrow_gap,
                                        arrow_gap,
                                        arrow_gap);
        return magnet_rotation*closest_arrow;
        }

        public Vector3 calculate_r(Vector3 position)
        {
        Vector3 r = new Vector3(Magnet.transform.position.x - position.x,
                                        Magnet.transform.position.y - position.y,
                                        Magnet.transform.position.z - position.z);
        return r;
        }

        /**
        /* Sends instance nullreciever component for later processing of MagneticVibration.cs
        /* @param[in] args - SelectEnterEventArgs - argument for GameObject that has dropped magnet
        */
        public void unsend_Self(SelectExitEventArgs args)
        {
            if (args.interactorObject is NearFarInteractor controller)
            {
                RecieveAndSendToSystem reciever = controller.GetComponent<RecieveAndSendToSystem>(); // Store reciever component of controller
                reciever.get_magnet(null); // RecieveAndSendToSystem object gets null
                ChangeTag holding = Magnet.GetComponent<ChangeTag>();
                holding.not_hovering();
            }
        }

    }
}
