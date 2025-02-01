

namespace C__Prcatice
{
    internal class Program
    {

        static Vector3D BCalc(Magnet magnet, Vector3D arrowPosition)
            /*
            Description:
                Caluculates the magnet field line vector at a given position of the arrow.

            Parameters:
                Magnet magnet:
                    The magnet that is having its magnetic field line calculated.
                Vector3D arrowPostion:
                    Postion of space that the magnetic field line calculated for.
                    In future this may be a Arriw object but for now it is a Vector3D (likely a parent class)
            Returns:
                Vector3D B:
                    The magnetic field line vector from that magnet in the arrows postion in space.
             */
        {
            Vector3D r = magnet.Pos - arrowPosition;
            Console.WriteLine("r is " +  r);

            Vector3D rUnit = Vector3D.UnitVector(r);
            Console.WriteLine("unit r is " + rUnit);

            double dot = Vector3D.Dot(rUnit, magnet.Magnitization);
            Console.WriteLine("Magnitization " + magnet.Magnitization);
            Console.WriteLine("Dot product of mag and unit r " + dot);
            dot = dot * 3;

            Vector3D nominator = rUnit*dot;
            nominator = nominator-magnet.Magnitization;
            Console.WriteLine("Nominator "+nominator);

            double denominator = Math.Pow(r.Magnitude(), 3);
            Console.WriteLine("Denominator "+denominator);

            Vector3D B = nominator/denominator;
            B = B * 1e-7;//mu 0 = 4pi*1e-7 and in eq mu 0 /(4*pi) 
            
            return B;
        }


        static void Main(string[] args)
        {
            Vector3D auxField = new  Vector3D(1, 1, 1);
            Vector3D magPosition = new Vector3D(0, 0, 0);
            Magnet magnet = new Magnet(0.1,magPosition,auxField);

            Vector3D arrorPosition = new Vector3D(1, 0, 0);

            Console.WriteLine("B value "+BCalc(magnet, arrorPosition));

        }

    }
}
