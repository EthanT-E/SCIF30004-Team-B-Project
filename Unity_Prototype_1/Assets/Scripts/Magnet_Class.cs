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

        public Magnet_class(GameObject iMagnet, List<GameObject> mag_list, List<Vector3> mag_pos,Vector3 start_pos)
        {
            Magnet = iMagnet;
            Magnet.transform.position = start_pos;
            mag_list.Add(Magnet);
            mag_pos.Add(Magnet.transform.position);
            magnetic_susceptibility = 1;
            auxilary_field = new Vector3(1, 5, 1);
        }

        public Magnet_class(GameObject iMagnet, Vector3 start_pos)
        {
            Magnet = iMagnet;
            Magnet.transform.position = start_pos;
            magnetic_susceptibility = 1;
            auxilary_field = new Vector3(1, 5, 1);
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



    }
}
