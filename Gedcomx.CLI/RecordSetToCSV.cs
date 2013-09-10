using System;
using NDesk.Options;

namespace Gx.CLI
{
	class RecordSetToCSV
	{
		public static void Main (string[] args)
		{
			string file = null;
			
			OptionSet options = new OptionSet() {
				{ "f|file=", o =>  file = o }
			};
			options.Parse( args );
			
			if (file == null) {
				ShowHelp(options);
				return;
			}
			
            RecordSetTransformer.WriteCSV( file, System.Console.Out );
		}
		
		static void ShowHelp(OptionSet options) 
		{
			Console.WriteLine("Options:");
			options.WriteOptionDescriptions(Console.Out);
		}
	}
}
