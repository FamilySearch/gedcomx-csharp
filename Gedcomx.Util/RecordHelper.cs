using System;
using System.Collections.Generic;
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
                            if (id.Equals(source.Id)) {
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

