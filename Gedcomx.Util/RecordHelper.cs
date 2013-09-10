using System;
using System.Collections.Generic;
using System.Data;
using Gx;
using Gx.Records;
using Gx.Conclusion;
using Gx.Types;
using Gx.Source;

namespace Gx.Util
{
    public static class RecordHelper
    {
        /// <summary>
        /// Enumerates the fields of a record. Note that this method assumes the record is not a census record
        /// (which is usually stitched together from a set of person subrecords) and therefore will return all 
        /// fields in the record. Consider using <see cref="RecordHelper.EnumerateCensusRecordFields"/> for
        /// census records.
        /// </summary>
        /// <returns>
        /// The fields.
        /// </returns>
        /// <param name='record'>
        /// The record.
        /// </param>
        public static List<Field> EnumerateFields (Gedcomx record)
        {
            return new FieldGatheringVisitor (record).Fields;
        }
        
        /// <summary>
        /// Enumerates the fields of a census record, aggregating them by the persons in the record.
        /// </summary>
        /// <returns>
        /// The census record fields.
        /// </returns>
        /// <param name='record'>
        /// The census record.
        /// </param>
        public static ICollection<List<Field>> EnumerateCensusRecordFields (Gedcomx record)
        {
            return new CensusFieldGatheringVisitor (record).FieldsByPerson.Values;
        }
        
        /// <summary>
        /// Builds a table of records for a given record set.
        /// </summary>
        /// <returns>
        /// The table of records.
        /// </returns>
        /// <param name='recordSet'>
        /// The record set.
        /// </param>
        public static DataTable BuildTableOfRecords (RecordSet recordSet)
        {
            DataTable table = new DataTable ();
            FieldValueTableBuildingVisitor values = new FieldValueTableBuildingVisitor( recordSet );
            foreach (string column in values.ColumnNames) {
                table.Columns.Add(column, typeof(string));
            }
            foreach (Dictionary<string, string> fieldSet in values.Rows) {
                foreach (KeyValuePair<string, string> entry in fieldSet) {
                    DataRow row = table.NewRow();
                    row[entry.Key] = entry.Value;
                    table.Rows.Add(row);
                }
            }
            return table;
        }
  
        /// <summary>
        /// Utility method used to determine whether the given record covers census data.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the record covers any census data.
        /// </returns>
        /// <param name='record'>
        /// The record.
        /// </param>
        public static bool IsCensusRecord (Gedcomx record)
        {
            String uri = record.DescriptionRef;
            if (uri != null) {
                int hashIndex = uri.IndexOf ('#');
                if (hashIndex >= 0) {
                    string id = uri.Substring (hashIndex);
                    if (record.SourceDescriptions != null) {
                        foreach (SourceDescription source in record.SourceDescriptions) {
                            if (id.Equals (source.Id)) {
                                if (source.Coverage != null) {
                                    foreach (Coverage coverage in source.Coverage) {
                                        if (coverage.KnownRecordType == RecordType.Census) {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            return false;
        }
        
        /// <summary>
        /// Visitor that gathers all the field values in a set of records by their ids.
        /// </summary>
        private class FieldValueTableBuildingVisitor : GedcomxModelVisitorBase
        {
            private HashSet<string> columnNames = new HashSet<string> ();
            private List<Dictionary<string, string>> rows = new List<Dictionary<string, string>> ();
            private Dictionary<string, string> currentRecord = null;
            private List<Dictionary<string, string>> subrecords = null;
            private bool parsingCensus = false;
            
            public FieldValueTableBuildingVisitor (RecordSet records)
            {
                VisitRecordSet (records);
            }
            
            new public void VisitGedcomx (Gedcomx gx)
            {
                if ( IsCensusRecord (gx)) {
                    this.parsingCensus = true;
                }
                
                this.currentRecord = new Dictionary<string, string>();
                this.subrecords = new List<Dictionary<string, string>>();
                
                base.VisitGedcomx( gx );
                
                if (this.subrecords.Count > 0) {
                    //if we have any subrecords, the field values list only contains the field values
                    //that are applicable to all subrecords. So iterate through each subrecord and add 
                    //the values of the parent record to them.
                    foreach (Dictionary<string, string> subrecord in this.subrecords) {
                        foreach (KeyValuePair<string, string> entry in this.currentRecord) {
                            subrecord[entry.Key] = entry.Value;
                        }
                        this.rows.Add(subrecord);
                    }
                }
                else {
                    //no subrecords; just add the record fields.
                    this.rows.Add(currentRecord);
                }
                
                this.parsingCensus = false;
                this.currentRecord = null;
                this.subrecords = null;
            }

            new public void VisitFieldValue (FieldValue fieldValue)
            {
                if (fieldValue.LabelId != null) {
                    this.columnNames.Add( fieldValue.LabelId );
                    this.currentRecord[ fieldValue.LabelId ] = fieldValue.Text;
                }
                else {
                    //todo: throw some error? log some warning?
                }
            }

            new public void VisitPerson (Gx.Conclusion.Person person)
            {
                Dictionary<string, string> recordFieldValues = this.currentRecord;
                if (parsingCensus) {
                    this.currentRecord = new Dictionary<string, string>();
                }
                
                base.VisitPerson( person );
                
                if (parsingCensus) {
                    //add the person as a subrecord...
                    this.subrecords.Add(this.currentRecord);
                    //...and put the record back.
                    this.currentRecord = recordFieldValues;
                }
            }
            
            public HashSet<string> ColumnNames {
                get {
                    return this.columnNames;
                }
            }

            public List<Dictionary<string, string>> Rows {
                get {
                    return this.rows;
                }
            }
            
        }
  
        /// <summary>
        /// Visitor that gathers all fields in a record.
        /// </summary>
        private class FieldGatheringVisitor : GedcomxModelVisitorBase
        {            
            private List<Field> fields = new List<Field> ();
            
            public FieldGatheringVisitor (Gedcomx record)
            {
                VisitGedcomx (record);
            }
            
            public List<Field> Fields {
                get {
                    return this.fields;
                }
            }
            
            new public void VisitField (Field field)
            {
                this.fields.Add (field);
            }
            
        }

        /// <summary>
        /// Visitor that gathers all fields in a census record.
        /// </summary>
        private class CensusFieldGatheringVisitor : GedcomxModelVisitorBase
        {            
        
            private Dictionary<Person, List<Field>> fieldsByPerson = new Dictionary<Person, List<Field>> ();
            private List<Field> commonFields = new List<Field> ();
            
            public CensusFieldGatheringVisitor (Gedcomx record)
            {
                VisitGedcomx (record);
                foreach (List<Field> fields in this.fieldsByPerson.Values) {
                    fields.AddRange (this.commonFields);
                }
            }
            
            public Dictionary<Person, List<Field>> FieldsByPerson {
                get {
                    return this.fieldsByPerson;
                }
            }

            new public void VisitField (Field field)
            {
                Person person = null;
                foreach (object item in this.contextStack) {
                    if (item is Person) {
                        person = item as Person;
                        break;
                    }
                }
                
                if (person == null) {
                    this.commonFields.Add (field);
                } else {
                    List<Field> personFields;
                    if (this.fieldsByPerson.ContainsKey (person)) {
                        personFields = this.fieldsByPerson [person];
                    } else {
                        personFields = new List<Field> ();
                        this.fieldsByPerson.Add (person, personFields);
                    }
                    
                    personFields.Add (field);
                }
            }
            
        }
    }
    
}

