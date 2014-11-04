using System;
using System.Collections.Generic;
using System.Data;
using Gx.Records;
using Gx.Conclusion;
using Gx.Types;
using Gx.Source;

namespace Gx.Util
{
    /// <summary>
    /// Provides management helpers in working with a collection of records.
    /// </summary>
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
        public static List<Field> EnumerateFields(Gedcomx record)
        {
            return new FieldGatheringVisitor(record).Fields;
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
        public static ICollection<List<Field>> EnumerateCensusRecordFields(Gedcomx record)
        {
            return new CensusFieldGatheringVisitor(record).FieldsByPerson.Values;
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
        public static DataTable BuildTableOfRecords(RecordSet recordSet)
        {
            var table = new DataTable();
            var values = new FieldValueTableBuildingVisitor(recordSet);
            foreach (string column in values.ColumnNames)
            {
                table.Columns.Add(column, typeof(string));
            }
            foreach (Dictionary<string, string> fieldSet in values.Rows)
            {
                DataRow row = table.NewRow();
                foreach (KeyValuePair<string, string> entry in fieldSet)
                {
                    row[entry.Key] = string.Format("\"{0}\"", entry.Value.Replace("\"", "\\\""));
                }
                table.Rows.Add(row);
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
        public static bool IsCensusRecord(Gedcomx record)
        {
            String uri = record.DescriptionRef;
            if (uri != null)
            {
                int hashIndex = uri.IndexOf('#');
                if (hashIndex >= 0)
                {
                    string id = uri.Substring(hashIndex);
                    if (record.SourceDescriptions != null)
                    {
                        foreach (SourceDescription source in record.SourceDescriptions)
                        {
                            if (id.Equals(source.Id))
                            {
                                if (source.Coverage != null)
                                {
                                    foreach (Coverage coverage in source.Coverage)
                                    {
                                        if (coverage.KnownRecordType == RecordType.Census)
                                        {
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
            private readonly HashSet<string> _columnNames = new HashSet<string>();
            private readonly List<Dictionary<string, string>> _rows = new List<Dictionary<string, string>>();
            private Dictionary<string, string> _currentRecord;
            private List<Dictionary<string, string>> _subrecords;
            private bool _parsingCensus;

            public FieldValueTableBuildingVisitor(RecordSet records)
            {
                VisitRecordSet(records);
            }

            public override void VisitGedcomx(Gedcomx gx)
            {
                if (IsCensusRecord(gx))
                {
                    _parsingCensus = true;
                }

                _currentRecord = new Dictionary<string, string>();
                _subrecords = new List<Dictionary<string, string>>();

                base.VisitGedcomx(gx);

                if (_subrecords.Count > 0)
                {
                    //if we have any subrecords, the field values list only contains the field values
                    //that are applicable to all subrecords. So iterate through each subrecord and add 
                    //the values of the parent record to them.
                    foreach (Dictionary<string, string> subrecord in _subrecords)
                    {
                        foreach (KeyValuePair<string, string> entry in _currentRecord)
                        {
                            subrecord[entry.Key] = entry.Value;
                        }
                        _rows.Add(subrecord);
                    }
                }
                else
                {
                    //no subrecords; just add the record fields.
                    _rows.Add(_currentRecord);
                }

                _parsingCensus = false;
                _currentRecord = null;
                _subrecords = null;
            }

            public override void VisitFieldValue(FieldValue fieldValue)
            {
                if (fieldValue.LabelId != null)
                {
                    _columnNames.Add(fieldValue.LabelId);
                    _currentRecord[fieldValue.LabelId] = fieldValue.Text;
                }
                else
                {
                    //todo: throw some error? log some warning?
                }
            }

            public override void VisitPerson(Person person)
            {
                Dictionary<string, string> recordFieldValues = _currentRecord;
                if (_parsingCensus)
                {
                    _currentRecord = new Dictionary<string, string>();
                }

                base.VisitPerson(person);

                if (_parsingCensus)
                {
                    //add the person as a subrecord...
                    _subrecords.Add(_currentRecord);
                    //...and put the record back.
                    _currentRecord = recordFieldValues;
                }
            }

            public override void VisitSourceDescription(SourceDescription sourceDescription)
            {
                if ((sourceDescription.KnownResourceType == ResourceType.DigitalArtifact) && (sourceDescription.About != null))
                {
                    _columnNames.Add("IMAGE_URI");
                    _currentRecord["IMAGE_URI"] = sourceDescription.About;
                }

                base.VisitSourceDescription(sourceDescription);
            }

            public IEnumerable<string> ColumnNames
            {
                get
                {
                    return _columnNames;
                }
            }

            public IEnumerable<Dictionary<string, string>> Rows
            {
                get
                {
                    return _rows;
                }
            }

        }

        /// <summary>
        /// Visitor that gathers all fields in a record.
        /// </summary>
        private sealed class FieldGatheringVisitor : GedcomxModelVisitorBase
        {
            private readonly List<Field> _fields = new List<Field>();

            public FieldGatheringVisitor(Gedcomx record)
            {
                VisitGedcomx(record);
            }

            public List<Field> Fields
            {
                get
                {
                    return _fields;
                }
            }

            public override void VisitField(Field field)
            {
                _fields.Add(field);
            }

        }

        /// <summary>
        /// Visitor that gathers all fields in a census record.
        /// </summary>
        private sealed class CensusFieldGatheringVisitor : GedcomxModelVisitorBase
        {

            private readonly Dictionary<Person, List<Field>> _fieldsByPerson = new Dictionary<Person, List<Field>>();
            private readonly List<Field> _commonFields = new List<Field>();

            public CensusFieldGatheringVisitor(Gedcomx record)
            {
                VisitGedcomx(record);
                foreach (List<Field> fields in _fieldsByPerson.Values)
                {
                    fields.AddRange(_commonFields);
                }
            }

            public Dictionary<Person, List<Field>> FieldsByPerson
            {
                get
                {
                    return _fieldsByPerson;
                }
            }

            public override void VisitField(Field field)
            {
                Person person = null;
                foreach (object item in contextStack)
                {
                    if (item is Person)
                    {
                        person = item as Person;
                        break;
                    }
                }

                if (person == null)
                {
                    _commonFields.Add(field);
                }
                else
                {
                    List<Field> personFields;
                    if (_fieldsByPerson.ContainsKey(person))
                    {
                        personFields = _fieldsByPerson[person];
                    }
                    else
                    {
                        personFields = new List<Field>();
                        _fieldsByPerson.Add(person, personFields);
                    }

                    personFields.Add(field);
                }
            }

        }
    }

}

