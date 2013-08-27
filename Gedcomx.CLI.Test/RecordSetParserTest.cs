using System;
using System.Reflection;
using System.IO;
using NUnit.Framework;
using Gx.CLI;

namespace Gx.CLI.Test
{
	[TestFixture]
	public class RecordSetParserTest
	{
		[Test]
		public void ParseTest ()
		{
			RecordSetParser parser = new RecordSetParser();
			Assembly a = Assembly.GetExecutingAssembly();
			Stream records1307888 = a.GetManifestResourceStream("Gedcomx.CLI.Test.recordset-1307888.xml");
			int count = parser.Parse(records1307888);
			Assert.AreEqual(10, count);
		}
	}
}

