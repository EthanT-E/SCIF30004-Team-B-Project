using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

namespace Assets.Scripts
{

    public class Magnet_class : MonoBehaviour
    {
        public GameObject Magnet;
        
        public Vector3 MagnetPosition;
        public float magnetic_susceptibility;
        public Vector3 auxilary_field, dipole_moment;

        public Magnet_class(GameObject prefab, Vector3 start_pos, Vector3 iAux, float iSus = 1)
        {
            Magnet = Instantiate(prefab);
            Magnet.transform.position = start_pos;
            magnetic_susceptibility = iSus;

            auxilary_field = iAux;

        }

        private void Dipole_moment()
        {
            dipole_moment = auxilary_field * magnetic_susceptibility;
        }

        public void set_suscept(float Ichi)
        {
            magnetic_susceptibility = Ichi;
            Dipole_moment();
        }

        public void set_auxiliary(Vector3 iAux)
        {
            auxilary_field = iAux;
            Dipole_moment();
        }

        public void new_pos()
        {
             MagnetPosition = Magnet.transform.position;
        }

        public static void Generate_magnet(GameObject prefab, List<Magnet_class> Magnet_list, Vector3 start_pos, Vector3 iAux, float iSus = 1)
        {
            Magnet_class mag = new Magnet_class(prefab, start_pos, iAux, iSus);
            Magnet_list.Add(mag);
        }

        public static void Generate_magnet(GameObject prefab, List<Magnet_class> Magnet_list, Vector3 start_pos, float iSus = 1)
        {
            Vector3 DefaultAux = new Vector3(1, 5, 1);
            Magnet_class mag = new Magnet_class(prefab, start_pos, DefaultAux, iSus);
            Magnet_list.Add(mag);
        }

    }
}
