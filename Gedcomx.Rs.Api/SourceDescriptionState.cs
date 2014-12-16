using Gedcomx.Model;
using Gx.Links;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Source;
using Gx.Conclusion;

namespace Gx.Rs.Api
{
    /// <summary>
    /// The SourceDescriptionState exposes management functions for a source description.
    /// </summary>
    public class SourceDescriptionState : GedcomxApplicationState<Gedcomx>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceDescriptionState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal SourceDescriptionState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        /// <summary>
        /// Gets the rel name for the current state instance. This is expected to be overridden.
        /// </summary>
        /// <value>
        /// The rel name for the current state instance
        /// </value>
        public override String SelfRel
        {
            get
            {
                return Rel.DESCRIPTION;
            }
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
            return new SourceDescriptionState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        /// <summary>
        /// Gets the main data element represented by this state instance.
        /// </summary>
        /// <value>
        /// The main data element represented by this state instance.
        /// </value>
        protected override SupportsLinks MainDataElement
        {
            get
            {
                return SourceDescription;
            }
        }

        /// <summary>
        /// Gets the source description represented by this state instance.
        /// </summary>
        /// <value>
        /// The source description represented by this state instance.
        /// </value>
        public SourceDescription SourceDescription
        {
            get
            {
                return Entity == null ? null : Entity.SourceDescriptions == null ? null : Entity.SourceDescriptions.FirstOrDefault();
            }
        }

        /// <summary>
        /// Updates the specified source description.
        /// </summary>
        /// <param name="description">The source description to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionState"/> instance containing the REST API response.
        /// </returns>
        public SourceDescriptionState Update(SourceDescription description, params StateTransitionOption[] options)
        {
            Gedcomx entity = new Gedcomx();
            entity.SourceDescriptions = new List<SourceDescription>() { description };
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(GetSelfUri(), Method.POST);
            return this.stateFactory.NewSourceDescriptionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Read personas associated with the current source description.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonsState"/> instance containing the REST API response.
        /// </returns>
        public PersonsState ReadPersonas(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.PERSONS);
            if (link == null || link.Href == null)
            {
                return this.stateFactory.NewPersonsState(this.Request, this.Response, this.Client, this.CurrentAccessToken);
            }
            else
            {
                IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
                return this.stateFactory.NewPersonsState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
            }
        }

        /// <summary>
        /// Adds a persona to the current source description.
        /// </summary>
        /// <param name="person">The person to associate with the current source description.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddPersona(Person person, params StateTransitionOption[] options)
        {
            Gedcomx entity = new Gedcomx();
            entity.AddPerson(person);
            return AddPersona(entity, options);
        }

        /// <summary>
        /// Adds a persona to the current source description.
        /// </summary>
        /// <param name="entity">The Gedcomx entity with a person to associate with the current source description.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="PersonState"/> instance containing the REST API response.
        /// </returns>
        public PersonState AddPersona(Gedcomx entity, params StateTransitionOption[] options)
        {
            String target = GetSelfUri();
            Link link = GetLink(Rel.PERSONS);
            if (link != null && link.Href != null)
            {
                target = link.Href;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(target, Method.POST);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Queries for attached references to this source description.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionState"/> instance containing the REST API response.
        /// </returns>
        public SourceDescriptionState QueryAttachedReferences(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.SOURCE_REFERENCES_QUERY);
            if (link == null || link.Href == null)
            {
                return null;
            }
            else
            {
                IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
                return this.stateFactory.NewSourceDescriptionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
            }
        }

        /// <summary>
        /// Reads the collection specified by this state instance.
        /// </summary>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="CollectionState"/> instance containing the REST API response.
        /// </returns>
        public CollectionState ReadCollection(params StateTransitionOption[] options)
        {
            Link link = GetLink(Rel.COLLECTION);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewCollectionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
