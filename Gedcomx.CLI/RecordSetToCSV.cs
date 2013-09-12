using System;
using NDesk.Options;

namespace Gx.CLI
{
    class RecordSetToCSV
    {
        public static void Main (string[] args)
        {
            string file = null;
         
            var options = new OptionSet {
              { "f|file=", o => file = o }
            };
            options.Parse (args);
         
            if (file == null) {
                ShowHelp (options);
                return;
            }
         
            RecordSetTransformer.WriteCSV (file, Console.Out, Console.Out);

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
     
        static void ShowHelp (OptionSet options)
        {
            Console.WriteLine ("Options:");
            options.WriteOptionDescriptions (Console.Out);

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
