using System;
using System.Reflection;
using NUnit.Framework;

namespace Gx.CLI.Test
{
    [TestFixture]
    public class RecordSetParserTest
    {
        [Test]
        public void ParseTestExcludingOrigColumns ()
        {
            var a = Assembly.GetExecutingAssembly ();
            var records1307888 = a.GetManifestResourceStream ("Gedcomx.CLI.Test.recordset-1307888.xml");
            RecordSetTransformer.WriteCSV (records1307888, Console.Out, true);
        }

        [Test]
        public void ParseTestIncludingOrigColumns()
        {
            var a = Assembly.GetExecutingAssembly();
            var records1307888 = a.GetManifestResourceStream("Gedcomx.CLI.Test.recordset-1307888.xml");
            RecordSetTransformer.WriteCSV(records1307888, Console.Out, false);
        }
    }
}

