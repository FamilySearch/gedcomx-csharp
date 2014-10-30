using Gedcomx.Model;
using Gx.Links;
using Gx.Records;
using Gx.Source;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;

namespace Gx.Rs.Api
{
    public class CollectionsState : GedcomxApplicationState<Gedcomx>
    {
        internal CollectionsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
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
            return new CollectionsState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        public List<Collection> Collections
        {
            get
            {
                return this.Entity == null ? null : this.Entity.Collections;
            }
        }

        public List<SourceDescription> SourceDescriptions
        {
            get
            {
                return this.Entity == null ? null : this.Entity.SourceDescriptions;
            }
        }

        /// <summary>
        /// Reads the specified collection.
        /// </summary>
        /// <param name="collection">The collection to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="CollectionState" /> instance containing the REST API response.
        /// </returns>
        public CollectionState ReadCollection(Collection collection, params StateTransitionOption[] options)
        {
            Link link = collection.GetLink("self");
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewCollectionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Updates the specified collection.
        /// </summary>
        /// <param name="collection">The collection to update.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="CollectionState"/> instance containing the REST API response.
        /// </returns>
        public CollectionState UpdateCollection(Collection collection, params StateTransitionOption[] options)
        {
            Link link = collection.GetLink("self");
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.POST);
            return this.stateFactory.NewCollectionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the collection specified by <see cref="P:SourceDescription.About" />.
        /// </summary>
        /// <param name="sourceDescription">The source description that has a collection reference (in <see cref="P:SourceDescription.About"/>).</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="CollectionState" /> instance containing the REST API response.
        /// </returns>
        public CollectionState ReadCollection(SourceDescription sourceDescription, params StateTransitionOption[] options)
        {
            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(sourceDescription.About, Method.GET);
            return this.stateFactory.NewCollectionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
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

        /// <summary>
        /// Adds a collection to the collection by this state instance.
        /// </summary>
        /// <param name="collection">The collection to add.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="CollectionState" /> instance containing the REST API response.
        /// </returns>
        public CollectionState AddCollection(Collection collection, params StateTransitionOption[] options)
        {
            Link link = GetLink("self");
            String href = link == null ? null : link.Href == null ? null : link.Href;
            href = href == null ? GetUri() : href;

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(href, Method.POST);
            return (CollectionState)this.stateFactory.NewCollectionState(request, Invoke(request, options), this.Client, this.CurrentAccessToken).IfSuccessful();
        }
    }
}