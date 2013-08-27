using System;
using NDesk.Options;

namespace Gx.CLI
{
	class ParseRecordSet
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
			
			RecordSetParser parser = new RecordSetParser();
			parser.Parse(file);
		}
		
		static void ShowHelp(OptionSet options) 
		{
			Console.WriteLine("Options:");
			options.WriteOptionDescriptions(Console.Out);
		}
	}
}
