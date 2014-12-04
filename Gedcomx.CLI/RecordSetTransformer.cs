using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// <param name="statusWriter">
        /// Where to write the status of the procedure
        /// </param>
        /// <param name="excludeOrigColumns">
        /// Should the output include the original values as well as the standarized ones
        /// </param>
        public static void WriteCSV(String inFile, TextWriter outWriter, TextWriter statusWriter, bool excludeOrigColumns)
        {
            try
            {
                WriteCSV(new FileStream(inFile, FileMode.Open), outWriter, excludeOrigColumns);
                statusWriter.WriteLine("Finished");
            }
            catch (Exception exception)
            {
                statusWriter.Write(exception.Message);
            }
            
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
        /// <param name="excludeOrigColumns">
        /// Should the output include the original values as well as the standarized ones
        /// </param>
        public static void WriteCSV (Stream inStream, TextWriter outWriter, bool excludeOrigColumns)
        {
            var serializer = new XmlSerializer (typeof(RecordSet));
            var records = (RecordSet)serializer.Deserialize (inStream);
            var table = RecordHelper.BuildTableOfRecords (records);

            if (excludeOrigColumns)
            {
                var collectionFieldsToRemove = new List<DataColumn>();
                foreach (var collectionField in table.Columns.Cast<DataColumn>().Where(x => x.ColumnName.EndsWith("_ORIG")))
                {
                    if (
                        table.Columns.Cast<DataColumn>().Any(
                            x =>
                            x.ColumnName ==
                            collectionField.ColumnName.Substring(0, collectionField.ColumnName.Length - 5) + "_STD" 
                                || x.ColumnName == collectionField.ColumnName.Substring(0, collectionField.ColumnName.Length - 5)))
                    {
                        collectionFieldsToRemove.Add(collectionField);
                    }
                }

                foreach (var dataColumn in collectionFieldsToRemove)
                {
                    table.Columns.Remove(dataColumn);    
                }
            }

            var columnCount = table.Columns.Count;
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
                outWriter.WriteLine();
            }

            // Ensure all the buffered data is written before finishing
            outWriter.Flush();
        }
    }
}

