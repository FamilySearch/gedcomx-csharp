using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Gedcomx.Model;
using Gedcomx.Support;

using Gx.Common;
using Gx.Conclusion;
using Gx.Records;
using Gx.Rs.Api.Util;
using Gx.Source;
using Gx.Types;

using RestSharp;
using RestSharp.Extensions;

using Tavis.UriTemplates;

namespace Gx.Rs.Api
{

    /// <summary>
    /// The CollectionState is a collection of resources and exposes management of those resources.
    /// </summary>
    public class CollectionState : GedcomxApplicationState<Gedcomx>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionState"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target collection resides.</param>
        public CollectionState(Uri uri)
            : this(uri, new StateFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionState"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target collection resides.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private CollectionState(Uri uri, StateFactory stateFactory)
            : this(uri, stateFactory.LoadDefaultClient(uri), stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionState"/> class.
        /// </summary>
        /// <param name="uri">The URI where the target collection resides.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private CollectionState(Uri uri, IFilterableRestClient client, StateFactory stateFactory)
            : this(new RedirectableRestRequest().Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE).Build(uri, Method.GET), client, stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this CollectionState.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        private CollectionState(IRestRequest request, IFilterableRestClient client, StateFactory stateFactory)
            : this(request, client.Handle(request), client, null, stateFactory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that was used to instantiate the REST API response.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal CollectionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, string accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        /// <summary>
        /// Clones the current state instance.
        /// </summary>
        /// <param name="request">The REST API request used to create this state instance.</param>
        /// <param name="response">The REST API response used to create this state instance.</param>
        /// <param name="client">The REST API client used to create this state instance.</param>
        /// <returns>A cloned instance of the current state instance.</returns>
        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new CollectionState(request, response, client, CurrentAccessToken, stateFactory);
        }

        /// <summary>
        /// Gets the main data element represented by this state instance. In this overridden implementation, it is the first collection from <see cref="Gedcomx.Collections"/>.
        /// </summary>
        /// <value>
        /// The main data element represented by this state instance. In this overridden implementation, it is the first collection from <see cref="Gedcomx.Collections"/>.
        /// </value>
        protected override ISupportsLinks MainDataElement
        {
            get
            {
                return Entity == null ? null : Entity.AnyCollections() ? Entity.Collections.FirstOrDefault() : null;
            }
        }

        /// <summary>
        /// Gets the first collection from <see cref="Gedcomx.Collections"/>.
        /// </summary>
        /// <value>
        /// The first collection from <see cref="Gedcomx.Collections"/>.
        /// </value>
        public Collection Collection
        {
            get
            {
                return Entity == null ? null : Entity.AnyCollections() ? Entity.Collections.FirstOrDefault() : null;
            }
        }

        /// <summary>
        /// Updates the specified collection.
        /// </summary>
        /// <param name="collection">The collection to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="Gx.Rs.Api.CollectionState"/> instance containing the REST API response.
        /// </returns>
        public CollectionState Update(Collection collection, params IStateTransitionOption[] options)
        {
            return (CollectionState)Post(new Gedcomx().SetCollection(collection), options);
        }

        /// <summary>
        /// Reads records from this collection.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="Gx.Rs.Api.RecordsState"/> instance containing the REST API response.
        /// </returns>
        public RecordsState ReadRecords(params IStateTransitionOption[] options)
        {
            var link = GetLink(Rel.RECORDS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            var request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return stateFactory.NewRecordsState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Adds a record to this collection.
        /// </summary>
        /// <param name="record">The record to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="Gx.Rs.Api.CollectionState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public RecordState AddRecord(Gedcomx record, params IStateTransitionOption[] options)
        {
            var link = GetLink(Rel.RECORDS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException(string.Format("Collection at {0} doesn't support adding records.", GetUri()));
            }

            var request = CreateAuthenticatedGedcomxRequest().SetEntity(record).Build(link.Href, Method.POST);
            return stateFactory.NewRecordState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Creates a persons collection from the current collection.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// If a link exists for this resource, it returns a <see cref="PersonsState"/> instance containing the REST API response; otherwise, it returns null.
        /// </returns>
        public PersonsState ReadPersons(params IStateTransitionOption[] options)
        {
            var link = GetLink(Rel.PERSONS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            var request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return stateFactory.NewPersonsState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Adds a person to the current collection.
        /// </summary>
        /// <param name="person">The person to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddPerson(Person person, params IStateTransitionOption[] options)
        {
            var entity = new Gedcomx();
            entity.SetPerson(person);
            return AddPerson(entity, options);
        }

        /// <summary>
        /// Adds a person to the current collection.
        /// </summary>
        /// <param name="entity">The <see cref="Gedcomx"/> containing the person to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public PersonState AddPerson(Gedcomx entity, params IStateTransitionOption[] options)
        {
            var link = GetLink(Rel.PERSONS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException(string.Format("Collection at {0} doesn't support adding persons.", GetUri()));
            }

            var request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(link.Href, Method.POST);
            return stateFactory.NewPersonState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Reads the person record for the current user.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadPersonForCurrentUser(params IStateTransitionOption[] options)
        {
            var link = GetLink(Rel.CURRENT_USER_PERSON);
            if (link == null || link.Href == null)
            {
                return null;
            }

            var request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return stateFactory.NewPersonState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Reads the person specified by the URI.
        /// </summary>
        /// <param name="personUri">The person URI (e.g., https://api-integ.familysearch.org/platform/tree/persons/PPPP-PPP). </param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState ReadPerson(Uri personUri, params IStateTransitionOption[] options)
        {
            if (personUri == null)
            {
                return null;
            }

            var request = CreateAuthenticatedGedcomxRequest().Build(personUri, Method.GET);
            return stateFactory.NewPersonState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Searches for persons based off the specified query.
        /// </summary>
        /// <param name="query">The query to use for searching.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonSearchResultsState"/> instance containing the REST API response.
        /// </returns>
        public PersonSearchResultsState SearchForPersons(GedcomxPersonSearchQueryBuilder query, params IStateTransitionOption[] options)
        {
            return SearchForPersons(query.Build(), options);
        }

        /// <summary>
        /// Searches for persons based off the specified query.
        /// </summary>
        /// <param name="query">The query to use for searching.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonSearchResultsState"/> instance containing the REST API response.
        /// </returns>
        /// <remarks>String query format and additional information can be reviewed at https://www.familysearch.org/developers/docs/api/tree/Person_Search_resource. </remarks>
        public PersonSearchResultsState SearchForPersons(string query, params IStateTransitionOption[] options)
        {
            var link = GetLink(Rel.PERSON_SEARCH);
            if (link == null || link.Template == null)
            {
                return null;
            }
            var template = link.Template;

            var uri = new UriTemplate(template).AddParameter("q", query).Resolve();
            var request = CreateAuthenticatedFeedRequest().Build(uri, Method.GET);
            return stateFactory.NewPersonSearchResultsState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Searches for places based off the specified query.
        /// </summary>
        /// <param name="query">The query to use for searching.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PlaceSearchResultsState"/> instance containing the REST API response.
        /// </returns>
        public PlaceSearchResultsState SearchForPlaces(GedcomxPlaceSearchQueryBuilder query, params IStateTransitionOption[] options)
        {
            return SearchForPlaces(query.Build(), options);
        }

        /// <summary>
        /// Searches for places based off the specified query.
        /// </summary>
        /// <param name="query">The query to use for searching.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PlaceSearchResultsState"/> instance containing the REST API response.
        /// </returns>
        /// <remarks>String query format and additional information can be reviewed at https://www.familysearch.org/developers/docs/api/places/Places_Search_resource. </remarks>
        public PlaceSearchResultsState SearchForPlaces(string query, params IStateTransitionOption[] options)
        {
            var link = GetLink(Rel.PLACE_SEARCH);
            if (link == null || link.Template == null)
            {
                return null;
            }
            var template = link.Template;

            var uri = new UriTemplate(template).AddParameter("q", query).Resolve();
            var request = CreateAuthenticatedFeedRequest().Build(uri, Method.GET);
            return stateFactory.NewPlaceSearchResultsState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Reads relationships from the current collection.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipsState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipsState ReadRelationships(params IStateTransitionOption[] options)
        {
            var link = GetLink(Rel.RELATIONSHIPS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            var request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return stateFactory.NewRelationshipsState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Adds a spouse relationship between the two persons.
        /// </summary>
        /// <param name="person1">The first person to which a relationship will be added (e.g., a husband).</param>
        /// <param name="person2">The second person to which a relationship will be added (e.g., a wife).</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddSpouseRelationship(PersonState person1, PersonState person2, params IStateTransitionOption[] options)
        {
            return AddSpouseRelationship(person1, person2, null, options);
        }

        /// <summary>
        /// Adds a spouse relationship between the two persons and applies the specified fact.
        /// </summary>
        /// <param name="person1">The first person to which a relationship will be added (e.g., a husband).</param>
        /// <param name="person2">The second person to which a relationship will be added (e.g., a wife).</param>
        /// <param name="fact">The fact to apply when the relationship is created.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddSpouseRelationship(PersonState person1, PersonState person2, Fact fact, params IStateTransitionOption[] options)
        {
            var relationship = new Relationship
            {
                Person1 = new ResourceReference(person1.GetSelfUri()),
                Person2 = new ResourceReference(person2.GetSelfUri()),
                KnownType = RelationshipType.Couple
            };
            relationship.SetFact(fact);
            return AddRelationship(relationship, options);
        }

        /// <summary>
        /// Adds a parent child relationship between the two persons.
        /// </summary>
        /// <param name="parent">The parent person state.</param>
        /// <param name="child">The child person state.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddParentChildRelationship(PersonState parent, PersonState child, params IStateTransitionOption[] options)
        {
            return AddParentChildRelationship(parent, child, null, options);
        }

        /// <summary>
        /// Adds a parent child relationship between the two persons and applies the specified fact.
        /// </summary>
        /// <param name="parent">The parent person state.</param>
        /// <param name="child">The child person state.</param>
        /// <param name="fact">The fact to apply when the relationship is created.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState"/> instance containing the REST API response.
        /// </returns>
        public RelationshipState AddParentChildRelationship(PersonState parent, PersonState child, Fact fact, params IStateTransitionOption[] options)
        {
            var relationship = new Relationship
            {
                Person1 = new ResourceReference(parent.GetSelfUri()),
                Person2 = new ResourceReference(child.GetSelfUri()),
                KnownType = RelationshipType.ParentChild
            };
            relationship.SetFact(fact);
            return AddRelationship(relationship, options);
        }

        /// <summary>
        /// Adds relationship to the collection.
        /// </summary>
        /// <param name="relationship">The relationship to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipState" /> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public virtual RelationshipState AddRelationship(Relationship relationship, params IStateTransitionOption[] options)
        {
            var link = GetLink(Rel.RELATIONSHIPS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException(string.Format("Collection at {0} doesn't support adding relationships.", GetUri()));
            }

            var entity = new Gedcomx();
            entity.SetRelationship(relationship);
            var request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(link.Href, Method.POST);
            return stateFactory.NewRelationshipState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Adds the array of relationships to the collection.
        /// </summary>
        /// <param name="relationships">The relationships to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RelationshipsState" /> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public virtual RelationshipsState AddRelationships(List<Relationship> relationships, params IStateTransitionOption[] options)
        {
            var link = GetLink(Rel.RELATIONSHIPS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException(string.Format("Collection at {0} doesn't support adding relationships.", GetUri()));
            }

            var entity = new Gedcomx
            {
                Relationships = relationships
            };
            var request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(link.Href, Method.POST);
            return stateFactory.NewRelationshipsState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Adds an artifact to the collection.
        /// </summary>
        /// <param name="artifact">The artifact to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionState"/> instance containing the REST API response.
        /// </returns>
        public SourceDescriptionState AddArtifact(IDataSource artifact, params IStateTransitionOption[] options)
        {
            return AddArtifact(null, artifact, options);
        }

        /// <summary>
        /// Adds an artifact to the collection using the specified <see cref="SourceDescription"/>.
        /// </summary>
        /// <param name="description">The <see cref="SourceDescription"/> to apply to the artifact.</param>
        /// <param name="artifact">The artifact to add to the collection.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionState"/> instance containing the REST API response.
        /// </returns>
        public SourceDescriptionState AddArtifact(SourceDescription description, IDataSource artifact, params IStateTransitionOption[] options)
        {
            return AddArtifact(this, description, artifact, options);
        }

        /// <summary>
        /// Adds an artifact to the specified state, using the specified <see cref="SourceDescription"/>.
        /// </summary>
        /// <param name="state">The state instance to which the artifact will be added.</param>
        /// <param name="description">The <see cref="SourceDescription"/> to apply to the artifact.</param>
        /// <param name="artifact">The artifact to add to the state.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionState" /> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        internal static SourceDescriptionState AddArtifact(GedcomxApplicationState<Gedcomx> state, SourceDescription description, IDataSource artifact, params IStateTransitionOption[] options)
        {
            var link = state.GetLink(Rel.ARTIFACTS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException(string.Format("Resource at {0} doesn't support adding artifacts.", state.GetUri()));
            }

            var mediaType = artifact.ContentType;
            var request = state.CreateAuthenticatedGedcomxRequest()
                .ContentType(MediaTypes.MULTIPART_FORM_DATA_TYPE)
                .Build(link.Href, Method.POST);

            if (description != null)
            {
                if (description.Titles != null)
                {
                    foreach (var value in description.Titles)
                    {
                        request.AddFile("title", Encoding.UTF8.GetBytes(value.Value), null, MediaTypes.TEXT_PLAIN_TYPE);
                    }
                }
                if (description.Descriptions != null)
                {
                    foreach (var value in description.Descriptions)
                    {
                        request.AddFile("description", Encoding.UTF8.GetBytes(value.Value), null, MediaTypes.TEXT_PLAIN_TYPE);
                    }
                }
                if (description.AnyCitations())
                {
                    foreach (var citation in description.Citations)
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

            var inputBytes = GetBytes(artifact.InputStream);
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

        /// <summary>
        /// Reads the source descriptions for the current collection.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionsState"/> instance containing the REST API response.
        /// </returns>
        public SourceDescriptionsState ReadSourceDescriptions(params IStateTransitionOption[] options)
        {
            var link = GetLink(Rel.SOURCE_DESCRIPTIONS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            var request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return stateFactory.NewSourceDescriptionsState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Adds a <see cref="SourceDescription"/> to the current collection.
        /// </summary>
        /// <param name="source">The source description to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionState" /> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public SourceDescriptionState AddSourceDescription(SourceDescription source, params IStateTransitionOption[] options)
        {
            var link = GetLink(Rel.SOURCE_DESCRIPTIONS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException(string.Format("Collection at {0} doesn't support adding source descriptions.", GetUri()));
            }

            var entity = new Gedcomx();
            entity.SetSourceDescription(source);
            var request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(link.Href, Method.POST);
            return stateFactory.NewSourceDescriptionState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Reads the collection specified by this state instance.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="CollectionState"/> instance containing the REST API response.
        /// </returns>
        public CollectionState ReadCollection(params IStateTransitionOption[] options)
        {
            var link = GetLink(Rel.COLLECTION);
            if (link == null || link.Href == null)
            {
                return null;
            }

            var request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return stateFactory.NewCollectionState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Reads the subcollections specified by this state instance.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="CollectionsState"/> instance containing the REST API response.
        /// </returns>
        public CollectionsState ReadSubcollections(params IStateTransitionOption[] options)
        {
            var link = GetLink(Rel.SUBCOLLECTIONS);
            if (link == null || link.Href == null)
            {
                return null;
            }

            var request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return stateFactory.NewCollectionsState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Adds a collection to the subcollection resource specified by this state instance.
        /// </summary>
        /// <param name="collection">The collection to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="CollectionState" /> instance containing the REST API response.
        /// </returns>
        /// <exception cref="Gx.Rs.Api.GedcomxApplicationException">Thrown if this collection does not have a link to the resource.</exception>
        public CollectionState AddCollection(Collection collection, params IStateTransitionOption[] options)
        {
            var link = GetLink(Rel.SUBCOLLECTIONS);
            if (link == null || link.Href == null)
            {
                throw new GedcomxApplicationException(string.Format("Collection at {0} doesn't support adding subcollections.", GetUri()));
            }

            var request = CreateAuthenticatedGedcomxRequest().SetEntity(new Gedcomx().SetCollection(collection)).Build(link.Href, Method.POST);
            return stateFactory.NewCollectionState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Reads the resources (a collection <see cref="SourceDescription"/>s) of the current user.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionsState"/> instance containing the REST API response.
        /// </returns>
        public SourceDescriptionsState ReadResourcesOfCurrentUser(params IStateTransitionOption[] options)
        {
            var link = GetLink(Rel.CURRENT_USER_RESOURCES);
            if (link == null || link.Href == null)
            {
                return null;
            }

            var request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return stateFactory.NewSourceDescriptionsState(request, Invoke(request, options), Client, CurrentAccessToken);
        }

        /// <summary>
        /// Gets the <see cref="SourceDescription"/>s for the current <see cref="Gedcomx"/> entity.
        /// </summary>
        /// <returns>The <see cref="SourceDescription"/>s for the current <see cref="Gedcomx"/> entity.</returns>
        public List<SourceDescription> GetSourceDescriptions()
        {
            return Entity?.SourceDescriptions;
        }

        /// <summary>
        /// Gets the bytes from the specified stream.
        /// </summary>
        /// <param name="stream">The stream from which the bytes will be returned.</param>
        /// <returns>The bytes from the specified stream.</returns>
        private static byte[] GetBytes(Stream stream)
        {
            byte[] result = null;

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
