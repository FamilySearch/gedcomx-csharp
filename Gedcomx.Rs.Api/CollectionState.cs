using Gedcomx.Model;
using Gx.Common;
using Gx.Conclusion;
using Gx.Links;
using Gx.Records;
using Gx.Rs.Api.Util;
using Gx.Source;
using Gx.Types;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tavis.UriTemplates;
using RestSharp.Extensions;
using Gedcomx.Support;

namespace Gx.Rs.Api
{

    public class CollectionState : GedcomxApplicationState<Gedcomx>
    {

        public CollectionState(Uri uri)
            : this(uri, new StateFactory())
        {
        }

        private CollectionState(Uri uri, StateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClient(uri), stateFactory)
        {
        }

        private CollectionState(Uri uri, IFilterableRestClient client, StateFactory stateFactory)
            : this(new RestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(uri, Method.GET), client, stateFactory)
        {
        }

        private CollectionState(IRestRequest request, IFilterableRestClient client, StateFactory stateFactory)
            : this(request, client.Handle(request), client, null, stateFactory)
        {
        }

        protected internal CollectionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState<Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new CollectionState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        protected override SupportsLinks MainDataElement
        {
            get
            {
                return Entity == null ? null : Entity.Collections == null ? null : Entity.Collections.FirstOrDefault();
            }
        }

        public Collection Collection
        {
            get
            {
                return Entity == null ? null : Entity.Collections == null ? null : Entity.Collections.FirstOrDefault();
            }
        }

        public CollectionState Update(Collection collection, params StateTransitionOption[] options)
        {
            return (CollectionState)Post(new Gedcomx().AddCollection(collection), options);
        }

        public RecordsState ReadRecords(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.RECORDS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewRecordsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RecordState AddRecord(Gedcomx record, params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.RECORDS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException(String.Format("Collection at {0} doesn't support adding records.", GetUri()));
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(record).Build(link.Href, Method.POST);
            return this.stateFactory.NewRecordState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonsState ReadPersons(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.PERSONS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPersonsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState AddPerson(Person person, params StateTransitionOption[] options)
        {
            Gedcomx entity = new Gedcomx();
            entity.AddPerson(person);
            return AddPerson(entity, options);
        }

        public PersonState AddPerson(Gedcomx entity, params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.PERSONS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException(String.Format("Collection at {0} doesn't support adding persons.", GetUri()));
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(link.Href, Method.POST);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState ReadPersonForCurrentUser(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.CURRENT_USER_PERSON);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonState ReadPerson(Uri personUri, params StateTransitionOption[] options)
        {
            if (personUri == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(personUri, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PersonSearchResultsState SearchForPersons(GedcomxPersonSearchQueryBuilder query, params StateTransitionOption[] options)
        {
            return SearchForPersons(query.Build(), options);
        }

        public PersonSearchResultsState SearchForPersons(String query, params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.PERSON_SEARCH);
            if (link == null || link.Template == null)
            {
                return null;
            }
            String template = link.Template;

            String uri = new UriTemplate(template).AddParameter("q", query).Resolve();
            IRestRequest request = CreateAuthenticatedFeedRequest().Build(uri, Method.GET);
            return this.stateFactory.NewPersonSearchResultsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public PlaceSearchResultsState SearchForPlaces(GedcomxPlaceSearchQueryBuilder query, params StateTransitionOption[] options)
        {
            return SearchForPlaces(query.Build(), options);
        }

        public PlaceSearchResultsState SearchForPlaces(String query, params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.PLACE_SEARCH);
            if (link == null || link.Template == null)
            {
                return null;
            }
            String template = link.Template;

            String uri = new UriTemplate(template).AddParameter("q", query).Resolve();
            IRestRequest request = CreateAuthenticatedFeedRequest().Build(uri, Method.GET);
            return this.stateFactory.NewPlaceSearchResultsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipsState ReadRelationships(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.RELATIONSHIPS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewRelationshipsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public RelationshipState AddSpouseRelationship(PersonState person1, PersonState person2, params StateTransitionOption[] options)
        {
            return AddSpouseRelationship(person1, person2, null, options);
        }

        public RelationshipState AddSpouseRelationship(PersonState person1, PersonState person2, Fact fact, params StateTransitionOption[] options)
        {
            Relationship relationship = new Relationship();
            relationship.Person1 = new ResourceReference(person1.GetSelfUri());
            relationship.Person2 = new ResourceReference(person2.GetSelfUri());
            relationship.KnownType = RelationshipType.Couple;
            relationship.AddFact(fact);
            return AddRelationship(relationship, options);
        }

        public RelationshipState AddParentChildRelationship(PersonState parent, PersonState child, params StateTransitionOption[] options)
        {
            return AddParentChildRelationship(parent, child, null, options);
        }

        public RelationshipState AddParentChildRelationship(PersonState parent, PersonState child, Fact fact, params StateTransitionOption[] options)
        {
            Relationship relationship = new Relationship();
            relationship.Person1 = new ResourceReference(parent.GetSelfUri());
            relationship.Person2 = new ResourceReference(child.GetSelfUri());
            relationship.KnownType = RelationshipType.ParentChild;
            relationship.AddFact(fact);
            return AddRelationship(relationship, options);
        }

        public virtual RelationshipState AddRelationship(Relationship relationship, params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.RELATIONSHIPS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException(String.Format("Collection at {0} doesn't support adding relationships.", GetUri()));
            }

            Gedcomx entity = new Gedcomx();
            entity.AddRelationship(relationship);
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(link.Href, Method.POST);
            return this.stateFactory.NewRelationshipState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public virtual RelationshipsState AddRelationships(List<Relationship> relationships, params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.RELATIONSHIPS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException(String.Format("Collection at {0} doesn't support adding relationships.", GetUri()));
            }

            Gedcomx entity = new Gedcomx();
            entity.Relationships = relationships;
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(link.Href, Method.POST);
            return this.stateFactory.NewRelationshipsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public SourceDescriptionState AddArtifact(DataSource artifact, params StateTransitionOption[] options)
        {
            return AddArtifact(null, artifact, options);
        }

        public SourceDescriptionState AddArtifact(SourceDescription description, DataSource artifact, params StateTransitionOption[] options)
        {
            return AddArtifact(this, description, artifact, options);
        }

        internal static SourceDescriptionState AddArtifact(GedcomxApplicationState<Gedcomx> state, SourceDescription description, DataSource artifact, params StateTransitionOption[] options)
        {
            Link link = state.GetLink(Rel.ARTIFACTS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException(String.Format("Resource at {0} doesn't support adding artifacts.", state.GetUri()));
            }

            String mediaType = artifact.ContentType;
            IRestRequest request = state.CreateAuthenticatedGedcomxRequest()
                .ContentType(MediaTypes.MULTIPART_FORM_DATA_TYPE)
                .Build(link.Href, Method.POST);

            if (description != null)
            {
                if (description.Titles != null)
                {
                    foreach (TextValue value in description.Titles)
                    {
                        request.AddFile("title", Encoding.UTF8.GetBytes(value.Value), null, MediaTypes.TEXT_PLAIN_TYPE);
                    }
                }
                if (description.Descriptions != null)
                {
                    foreach (TextValue value in description.Descriptions)
                    {
                        request.AddFile("description", Encoding.UTF8.GetBytes(value.Value), null, MediaTypes.TEXT_PLAIN_TYPE);
                    }
                }
                if (description.Citations != null)
                {
                    foreach (SourceCitation citation in description.Citations)
                    {
                        request.AddFile("citation", Encoding.UTF8.GetBytes(citation.Value), null, MediaTypes.TEXT_PLAIN_TYPE);
                    }
                }
                if (description.MediaType != null)
                {
                    mediaType = description.MediaType;
                }
            }

            if (mediaType == null)
            {
                mediaType = MediaTypes.APPLICATION_OCTET_STREAM;//default to octet stream?
            }

            Byte[] inputBytes = GetBytes(artifact.InputStream);
            if (artifact.Name != null)
            {
                request.Files.Add(new FileParameter()
                {
                    Name = "artifact",
                    FileName = artifact.Name,
                    ContentType = artifact.ContentType,
                    Writer = new Action<Stream>(s =>
                    {
                        using (var ms = new MemoryStream(inputBytes))
                        using (var reader = new StreamReader(ms))
                        {
                            reader.BaseStream.CopyTo(s);
                        }
                    })
                });
            }

            return state.stateFactory.NewSourceDescriptionState(request, state.Invoke(request, options), state.Client, state.CurrentAccessToken);
        }

        public SourceDescriptionsState ReadSourceDescriptions(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.SOURCE_DESCRIPTIONS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewSourceDescriptionsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public SourceDescriptionState AddSourceDescription(SourceDescription source, params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.SOURCE_DESCRIPTIONS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException(String.Format("Collection at {0} doesn't support adding source descriptions.", GetUri()));
            }

            Gedcomx entity = new Gedcomx();
            entity.AddSourceDescription(source);
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(link.Href, Method.POST);
            return this.stateFactory.NewSourceDescriptionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public CollectionState ReadCollection(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.COLLECTION);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewCollectionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public CollectionsState ReadSubcollections(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.SUBCOLLECTIONS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewCollectionsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public CollectionState AddCollection(Collection collection, params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.SUBCOLLECTIONS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException(String.Format("Collection at {0} doesn't support adding subcollections.", GetUri()));
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(new Gedcomx().AddCollection(collection)).Build(link.Href, Method.POST);
            return this.stateFactory.NewCollectionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public SourceDescriptionsState ReadResourcesOfCurrentUser(params StateTransitionOption[] options)
        {
            Link link = this.GetLink(Rel.CURRENT_USER_RESOURCES);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewSourceDescriptionsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        public List<SourceDescription> GetSourceDescriptions()
        {
            return this.Entity == null ? null : this.Entity.SourceDescriptions;
        }

        private static Byte[] GetBytes(Stream stream)
        {
            Byte[] result = null;

            if (stream != null && stream.CanRead)
            {
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                }

                result = stream.ReadAsBytes();
            }

            return result;
        }
    }
}
