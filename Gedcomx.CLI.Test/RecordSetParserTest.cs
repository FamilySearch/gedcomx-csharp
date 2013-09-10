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
            Assembly a = Assembly.GetExecutingAssembly ();
            Stream records1307888 = a.GetManifestResourceStream ("Gedcomx.CLI.Test.recordset-1307888.xml");
            RecordSetTransformer.WriteCSV (records1307888, System.Console.Out);
        }
    }
}

