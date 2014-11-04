using System;
using Gx;
using Gx.Agent;
using Gx.Common;
using Gx.Conclusion;
using Gx.Links;
using Gx.Records;
using Gx.Source;
using Gx.Types;
using System.Collections.Generic;

namespace Gx.Util
{
    /// <summary>
    /// A dictionary for looking up GEDCOM X data elements by their local id (i.e. "fragment identifier").
    /// </summary>
    public class GedcomxIdDictionary : GedcomxModelVisitorBase
    {
        private readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();

        /// <summary>
        /// Initializes an id dictionary for the specified data set.
        /// </summary>
        /// <param name='gx'>
        /// The data set.
        /// </param>
        public GedcomxIdDictionary(Gedcomx gx)
        {
            VisitGedcomx(gx);
        }

        /// <summary>
        /// Initializes an id dictionary for the specified record set.
        /// </summary>
        /// <param name='rs'>
        /// The record set.
        /// </param>
        public GedcomxIdDictionary(RecordSet rs)
        {
            VisitRecordSet(rs);
        }

        /// <summary>
        /// Gets or sets the dictionary with an element of the specified id. If the specified key is not 
        /// found, a get operation throws a KeyNotFoundException, and a set operation creates a new element 
        /// with the specified key.
        /// </summary>
        /// <param name='key'>
        /// The id.
        /// </param>
        public object this[string key]
        {
            get
            {
                return dictionary[key];
            }

            set
            {
                dictionary[key] = value;
            }
        }

        /// <summary>
        /// Whether the dictionary contains the specified key.
        /// </summary>
        /// <returns>
        /// Whether the dictionary contains the specified key.
        /// </returns>
        /// <param name='key'>
        /// The key to test.
        /// </param>
        public bool ContainsKey(string key)
        {
            return dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Gets the value associated with the specified id.
        /// </summary>
        /// <returns>
        /// The data of the specified id.
        /// </returns>
        /// <param name='key'>
        /// The id of the value to get.
        /// </param>
        /// <param name='value'>
        /// When this method returns, contains the value associated with the specified key, if the key is found; 
        /// otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.
        /// </param>
        public bool TryGetValue(string key, out object value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Resolve the value referenced by the specified URI. If the object is not resolved, a 
        /// KeyNotFoundException is thrown.
        /// </summary>
        /// <param name='uri'>
        /// The URI.
        /// </param>
        public object Resolve(string uri)
        {
            int fragmentIndex = uri.IndexOf('#');
            if (fragmentIndex >= 0)
            {
                return this[uri.Substring(fragmentIndex)];
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Attempts to resolve the value referenced by the specified URI.
        /// </summary>
        /// <param name='uri'>
        /// The URI.
        /// </param>
        /// <param name='value'>
        /// When this method returns, contains the resolved value, if it is resolved; 
        /// otherwise, the default value for the type of the value parameter. 
        /// This parameter is passed uninitialized.
        /// </param>
        public bool TryResolveValue(string uri, out object value)
        {
            value = null;
            int fragmentIndex = uri.IndexOf('#');
            if (fragmentIndex >= 0)
            {
                return TryGetValue(uri.Substring(fragmentIndex), out value);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Visits the specified record set.
        /// </summary>
        /// <param name="rs">The record set to visit.</param>
        public override void VisitRecordSet(RecordSet rs)
        {
            if (rs.Id != null)
            {
                this.dictionary.Add(rs.Id, rs);
            }

            base.VisitRecordSet(rs);
        }

        /// <summary>
        /// Visits the specified <see cref="Gx.Gedcomx"/> entity.
        /// </summary>
        /// <param name="gx">The <see cref="Gx.Gedcomx"/> entity to visit.</param>
        public override void VisitGedcomx(Gx.Gedcomx gx)
        {
            if (gx.Id != null)
            {
                this.dictionary.Add(gx.Id, gx);
            }
            base.VisitGedcomx(gx);
        }

        /// <summary>
        /// Visits the document.
        /// </summary>
        /// <param name="document">The document to visit.</param>
        public override void VisitDocument(Gx.Conclusion.Document document)
        {
            if (document.Id != null)
            {
                this.dictionary.Add(document.Id, document);
            }
            base.VisitDocument(document);
        }

        /// <summary>
        /// Visits the place description.
        /// </summary>
        /// <param name="place">The place description to visit.</param>
        public override void VisitPlaceDescription(Gx.Conclusion.PlaceDescription place)
        {
            if (place.Id != null)
            {
                this.dictionary.Add(place.Id, place);
            }
            base.VisitPlaceDescription(place);
        }

        /// <summary>
        /// Visits the event.
        /// </summary>
        /// <param name="e">The event to visit.</param>
        public override void VisitEvent(Gx.Conclusion.Event e)
        {
            if (e.Id != null)
            {
                this.dictionary.Add(e.Id, e);
            }
            base.VisitEvent(e);
        }

        /// <summary>
        /// Visits the event role.
        /// </summary>
        /// <param name="role">The event role to visit.</param>
        public override void VisitEventRole(Gx.Conclusion.EventRole role)
        {
            if (role.Id != null)
            {
                this.dictionary.Add(role.Id, role);
            }
            base.VisitEventRole(role);
        }

        /// <summary>
        /// Visits the agent.
        /// </summary>
        /// <param name="agent">The agent to visit.</param>
        public override void VisitAgent(Gx.Agent.Agent agent)
        {
            if (agent.Id != null)
            {
                this.dictionary.Add(agent.Id, agent);
            }
            base.VisitAgent(agent);
        }

        /// <summary>
        /// Visits the source description.
        /// </summary>
        /// <param name="sourceDescription">The source description to visit.</param>
        public override void VisitSourceDescription(Gx.Source.SourceDescription sourceDescription)
        {
            if (sourceDescription.Id != null)
            {
                this.dictionary.Add(sourceDescription.Id, sourceDescription);
            }
            base.VisitSourceDescription(sourceDescription);
        }

        /// <summary>
        /// Visits the source citation.
        /// </summary>
        /// <param name="citation">The source citation to visit.</param>
        public override void VisitSourceCitation(Gx.Source.SourceCitation citation)
        {
            if (citation.Id != null)
            {
                this.dictionary.Add(citation.Id, citation);
            }
            base.VisitSourceCitation(citation);
        }

        /// <summary>
        /// Visits the collection.
        /// </summary>
        /// <param name="collection">The collection to visit.</param>
        public override void VisitCollection(Collection collection)
        {
            if (collection.Id != null)
            {
                this.dictionary.Add(collection.Id, collection);
            }
            base.VisitCollection(collection);
        }

        /// <summary>
        /// Visits the facet.
        /// </summary>
        /// <param name="facet">The facet to visit.</param>
        public override void VisitFacet(Facet facet)
        {
            if (facet.Id != null)
            {
                this.dictionary.Add(facet.Id, facet);
            }
            base.VisitFacet(facet);
        }

        /// <summary>
        /// Visits the record descriptor.
        /// </summary>
        /// <param name="recordDescriptor">The record descriptor to visit.</param>
        public override void VisitRecordDescriptor(RecordDescriptor recordDescriptor)
        {
            if (recordDescriptor.Id != null)
            {
                this.dictionary.Add(recordDescriptor.Id, recordDescriptor);
            }
            base.VisitRecordDescriptor(recordDescriptor);
        }

        /// <summary>
        /// Visits the field.
        /// </summary>
        /// <param name="field">The field to visit.</param>
        public override void VisitField(Field field)
        {
            if (field.Id != null)
            {
                this.dictionary.Add(field.Id, field);
            }
            base.VisitField(field);
        }

        /// <summary>
        /// Visits the field value.
        /// </summary>
        /// <param name="fieldValue">The field value to visit.</param>
        public override void VisitFieldValue(FieldValue fieldValue)
        {
            if (fieldValue.Id != null)
            {
                this.dictionary.Add(fieldValue.Id, fieldValue);
            }
            base.VisitFieldValue(fieldValue);
        }

        /// <summary>
        /// Visits the relationship.
        /// </summary>
        /// <param name="relationship">The relationship to visit.</param>
        public override void VisitRelationship(Gx.Conclusion.Relationship relationship)
        {
            if (relationship.Id != null)
            {
                this.dictionary.Add(relationship.Id, relationship);
            }
            base.VisitRelationship(relationship);
        }

        /// <summary>
        /// Visits the person.
        /// </summary>
        /// <param name="person">The person to visit.</param>
        public override void VisitPerson(Gx.Conclusion.Person person)
        {
            if (person.Id != null)
            {
                this.dictionary.Add(person.Id, person);
            }
            base.VisitPerson(person);
        }

        /// <summary>
        /// Visits the fact.
        /// </summary>
        /// <param name="fact">The fact to visit.</param>
        public override void VisitFact(Gx.Conclusion.Fact fact)
        {
            if (fact.Id != null)
            {
                this.dictionary.Add(fact.Id, fact);
            }
            base.VisitFact(fact);
        }

        /// <summary>
        /// Visits the place reference.
        /// </summary>
        /// <param name="place">The place reference to visit.</param>
        public override void VisitPlaceReference(Gx.Conclusion.PlaceReference place)
        {
            if (place.Id != null)
            {
                this.dictionary.Add(place.Id, place);
            }
            base.VisitPlaceReference(place);
        }

        /// <summary>
        /// Visits the date.
        /// </summary>
        /// <param name="date">The date to visit.</param>
        public override void VisitDate(Gx.Conclusion.DateInfo date)
        {
            if (date.Id != null)
            {
                this.dictionary.Add(date.Id, date);
            }
            base.VisitDate(date);
        }

        /// <summary>
        /// Visits the name.
        /// </summary>
        /// <param name="name">The name to visit.</param>
        public override void VisitName(Gx.Conclusion.Name name)
        {
            if (name.Id != null)
            {
                this.dictionary.Add(name.Id, name);
            }
            base.VisitName(name);
        }

        /// <summary>
        /// Visits the name form.
        /// </summary>
        /// <param name="form">The name form to visit.</param>
        public override void VisitNameForm(Gx.Conclusion.NameForm form)
        {
            if (form.Id != null)
            {
                this.dictionary.Add(form.Id, form);
            }
            base.VisitNameForm(form);
        }

        /// <summary>
        /// Visits the name part.
        /// </summary>
        /// <param name="part">The name part to visit.</param>
        public override void VisitNamePart(Gx.Conclusion.NamePart part)
        {
            if (part.Id != null)
            {
                this.dictionary.Add(part.Id, part);
            }
            base.VisitNamePart(part);
        }

        /// <summary>
        /// Visits the gender.
        /// </summary>
        /// <param name="gender">The gender to visit.</param>
        public override void VisitGender(Gx.Conclusion.Gender gender)
        {
            if (gender.Id != null)
            {
                this.dictionary.Add(gender.Id, gender);
            }
            base.VisitGender(gender);
        }

        /// <summary>
        /// Visits the source reference.
        /// </summary>
        /// <param name="sourceReference">The source reference to visit.</param>
        public override void VisitSourceReference(Gx.Source.SourceReference sourceReference)
        {
            if (sourceReference.Id != null)
            {
                this.dictionary.Add(sourceReference.Id, sourceReference);
            }
            base.VisitSourceReference(sourceReference);
        }

        /// <summary>
        /// Visits the note.
        /// </summary>
        /// <param name="note">The note to visit.</param>
        public override void VisitNote(Gx.Common.Note note)
        {
            if (note.Id != null)
            {
                this.dictionary.Add(note.Id, note);
            }
            base.VisitNote(note);
        }
    }
}

