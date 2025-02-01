namespace C__Prcatice
{
    public class Magnet
    {
        private double chi;
        private Vector3D pos = new Vector3D();
        private Vector3D aux = new Vector3D();
        private Vector3D magnitization = new Vector3D();

        public double Chi { get { return chi; } set { chi = value; } }
        public Vector3D Aux { get { return aux; } set { aux = value; } }
        public Vector3D Pos { get { return pos; } set { pos = value; } }
        public Vector3D Magnitization { get { return magnitization; } set { magnitization = value; } }


        public Magnet(double iChi, Vector3D iPos, Vector3D iAux)
        {
            Aux = iAux;
            Chi = iChi;
            Pos = iPos;
            Magnitization = iAux * iChi;
        }

    }
}