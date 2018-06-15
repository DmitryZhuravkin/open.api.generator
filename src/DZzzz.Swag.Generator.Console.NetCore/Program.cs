using DZzzz.Swag.Specification.Version12;

namespace DZzzz.Swag.Generator.Console.NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");

            Version12SpecificationProvider provider = new Version12SpecificationProvider();




            System.Console.ReadKey(true);
        }
    }
}
