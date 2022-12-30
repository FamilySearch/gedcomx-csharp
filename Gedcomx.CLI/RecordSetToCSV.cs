using System;

using CommandLine;

class Options
{
    [Option('f', "file", Required = true, HelpText = "Input files to be processed.")]
    public string File { get; set; }
}

namespace Gx.CLI
{
    class RecordSetToCSV
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
              .WithParsed(RunOptions);

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        static void RunOptions(Options opts)
        {
            RecordSetTransformer.WriteCSV(opts.File, Console.Out, Console.Out, false);
        }
    }
}
