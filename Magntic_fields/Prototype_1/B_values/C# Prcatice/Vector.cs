namespace C__Prcatice
{
    public class Vector3D
    {
        /*
         This class is meant to store a 3-dimensional vector of doubles and then be able to do vector maths on those objects.
         */
        private double[] xyz = [0, 0, 0];

        public Vector3D() { }//Allows creation of vector with default values of zero.

        public Vector3D(double iX, double iY, double iZ)
        {
            /*
            Description:
                Intitializes the objcet with the x,y,z values for the vector.
            
            Parameters:
                double iX:
                    The value of the top vaule of a vector in vector form.
                double iY:
                    The value of the middle value of a vector in column form.
                double iZ:
                    The value of the bottom value of a vector in column form.

             */
            XYZ[0] = iX;
            XYZ[1] = iY;
            XYZ[2] = iZ;
        }

        public double[] XYZ { get { return xyz; } set { xyz = value; } }

        public double Magnitude()
        {
            /*
            Description:
                Calculates the magnitude of a vector via the 3D Pythagorean theorem.
            Parameters:
                None
            Returns:
                double magnitude:
                    The calculated magnitude of the object.
             */
            double magnitude = (xyz[0] * xyz[0]) + (xyz[1] * xyz[1]) + (xyz[2] * xyz[2]);
            magnitude = Math.Sqrt(magnitude);
            return magnitude;
        }

        public static double Dot(Vector3D a, Vector3D b)
        {
            /*
            Description:
                Calculates the dot product of two Vector3D objects
            Parameters:
                Vector3D a, b:
                    Order of the parameters does not matter.
            Returns:
                double dot:
                    The dot product of the two vectors.
             */
            double dot = (a.XYZ[0] * b.XYZ[0]) + (a.XYZ[1] * b.XYZ[1]) + (a.XYZ[2] * b.XYZ[2]);
            return dot;
        }

        public static Vector3D UnitVector(Vector3D a)
        {
            /*
            Description:
                Calculates the unit vector (magnitude of 1) of a vector without editing input vector. 
            Parameters:
                Vector3D a:
                    A vector3D which intends to havew a unit vector taken and remain unedited.
            Returns:
                Vectror3D unit:
                    The unit vector of a.
             */
            Vector3D unit = a / a.Magnitude();
            return unit;
        }

        //below are operatore overloads to make the vector class easyer to use for scalar mutiplication/ division and vector addition/subtraction. 
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
