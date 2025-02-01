namespace C__Prcatice
{
    internal class Vector3D
    {
        private double[] xyz = [0, 0, 0];

        public Vector3D()
        {
        }

        public Vector3D(double iX, double iY, double iZ)
        {
            XYZ[0] = iX;
            XYZ[1] = iY;
            XYZ[2] = iZ;
        }

        public double[] XYZ { get { return xyz; } set { xyz = value; } }

        public double Magnitude()
        {
            double magnitude = (xyz[0] * xyz[0]) + (xyz[1] * xyz[1]) + (xyz[2] * xyz[2]);
            magnitude = Math.Sqrt(magnitude);
            return magnitude;
        }

        public static double Dot(Vector3D a, Vector3D b)
        {
            double dot = (a.XYZ[0] * b.XYZ[0]) + (a.XYZ[1] * b.XYZ[1]) + (a.XYZ[2] * b.XYZ[2]);
            return dot;
        }

        public static Vector3D UnitVector(Vector3D a)
        {
            Vector3D unit = a / a.Magnitude();
            return unit;
        }

        public static Vector3D operator *(Vector3D a, double b)
        {
            Vector3D Product = new Vector3D((a.XYZ[0] * b), (a.XYZ[1] * b), (a.XYZ[2] * b));
            return Product;
        }

        public static Vector3D operator /(Vector3D a, double b)
        {
            Vector3D Product = new Vector3D((a.XYZ[0] / b), (a.XYZ[1] / b), (a.XYZ[2] / b));
            return Product;
        }

        public static Vector3D operator -(Vector3D a, Vector3D b)
        {
            Vector3D sum = new Vector3D((a.XYZ[0] - b.XYZ[0]), (a.XYZ[1] - b.XYZ[1]), (a.XYZ[2] - b.XYZ[2]));

            return sum;
        }

        public static Vector3D operator +(Vector3D a, Vector3D b)
        {
            Vector3D sum = new Vector3D((a.XYZ[0] + b.XYZ[0]), (a.XYZ[1] + b.XYZ[1]), (a.XYZ[2] + b.XYZ[2]));

            return sum;
        }

        public override string ToString()
        {
            return "(" + XYZ[0] + ", " + XYZ[1] + ", " + XYZ[2] + ")";
        }
    }
}
