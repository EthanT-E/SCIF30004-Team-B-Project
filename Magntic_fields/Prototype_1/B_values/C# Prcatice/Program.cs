

namespace C__Prcatice
{
    internal class Program
    {

        static Vector3D BCalc(Magnet magnet, Vector3D arrowPosition)
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
            Magnet magnet = new(0.1,magPosition,auxField);

            Vector3D arrorPosition = new Vector3D(1, 0, 0);

            Console.WriteLine("B value "+BCalc(magnet, arrorPosition));

        }

    }
}
