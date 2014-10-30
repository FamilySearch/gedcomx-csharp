using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Source;
using Gx.Links;

namespace Gx.Rs.Api
{
    /// <summary>
    /// The SourceDescriptionsState exposes management functions for a collection of source descriptions.
    /// </summary>
    public class SourceDescriptionsState : GedcomxApplicationState<Gedcomx>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceDescriptionsState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        internal SourceDescriptionsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
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
        protected override GedcomxApplicationState<Gedcomx> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new SourceDescriptionsState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        /// <summary>
        /// Gets the source descriptions represented by this current state instance.
        /// </summary>
        /// <value>
        /// The source descriptions represented by this current state instance.
        /// </value>
        public List<SourceDescription> SourceDescriptions
        {
            get
            {
                return Entity == null ? null : Entity.SourceDescriptions;
            }
        }

        /// <summary>
        /// Adds a source description the current collection of source descriptions.
        /// </summary>
        /// <param name="source">The source description to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="SourceDescriptionState"/> instance containing the REST API response.
        /// </returns>
        public SourceDescriptionState AddSourceDescription(SourceDescription source, params StateTransitionOption[] options)
        {
            Gedcomx entity = new Gedcomx();
            entity.AddSourceDescription(source);
            IRestRequest request = CreateAuthenticatedGedcomxRequest().SetEntity(entity).Build(GetSelfUri(), Method.POST);
            return this.stateFactory.NewSourceDescriptionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
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
