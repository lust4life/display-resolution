using System;

namespace cds
{
    class Program
    {
        static void Main(string[] args)
        {
            var res = Helper.ChangeDisplaySettings(1920, 1080, Helper.Flags.CDS_TEST);
            System.Console.WriteLine(res);
        }
    }
}
