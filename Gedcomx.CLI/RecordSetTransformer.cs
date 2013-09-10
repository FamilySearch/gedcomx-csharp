using System;
using System.IO;
using Gx.Records;
using System.Xml.Serialization;
using Gx.Util;
using System.Data;

namespace Gx.CLI
{
 
    /// <summary>
    /// Utilities for transforming streams of record sets to other formats.
    /// </summary>
    public static class RecordSetTransformer
    {
        /// <summary>
        /// Reads a record set file and outputs a CSV.
        /// </summary>
        /// <param name='inFile'>
        /// The name of a file containing a record set.
        /// </param>
        /// <param name='outWriter'>
        /// Where to write the CSV.
        /// </param>
        public static void WriteCSV (String inFile, TextWriter outWriter)
        {
            WriteCSV (new FileStream (inFile, FileMode.Open), outWriter);
        }
     
        /// <summary>
        /// Reads a record set file and outputs a CSV.
        /// </summary>
        /// <param name='inStream'>
        /// The stream for the record set.
        /// </param>
        /// <param name='outWriter'>
        /// Where to write the CSV.
        /// </param>
        public static void WriteCSV (Stream inStream, TextWriter outWriter)
        {
            XmlSerializer serializer = new XmlSerializer (typeof(RecordSet));
            RecordSet records = (RecordSet)serializer.Deserialize (inStream);
            DataTable table = RecordHelper.BuildTableOfRecords (records);
            int columnCount = table.Columns.Count;
            foreach (DataColumn column in table.Columns) {
                outWriter.Write (column.ColumnName);
                if (--columnCount > 0) {
                    outWriter.Write (",");
                }
            }
            outWriter.WriteLine ();
            foreach (DataRow row in table.Rows) {
                columnCount = table.Columns.Count;
                foreach (DataColumn column in table.Columns) {
                    outWriter.Write (row [column]);
                    if (--columnCount > 0) {
                        outWriter.Write (",");
                    }
                }
            }
            
        }
    }
}

