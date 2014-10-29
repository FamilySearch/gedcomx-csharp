using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Links;

namespace Gx.Rs.Api
{
    public class AncestryResultsState : GedcomxApplicationState<Gedcomx>
    {
        internal AncestryResultsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
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
            return new AncestryResultsState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        public override String SelfRel
        {
            get
            {
                return Rel.ANCESTRY;
            }
        }

        public AncestryTree Tree
        {
            get
            {
                return Entity != null ? new AncestryTree(Entity) : null;
            }
        }

        public PersonState ReadPerson(int ancestorNumber, params StateTransitionOption[] options)
        {
            AncestryTree.AncestryNode ancestor = Tree.GetAncestor(ancestorNumber);
            if (ancestor == null)
            {
                return null;
            }

            Link selfLink = ancestor.Person.GetLink(Rel.PERSON);
            if (selfLink == null || selfLink.Href == null)
            {
                selfLink = ancestor.Person.GetLink(Rel.SELF);
            }

            String personUri = selfLink == null || selfLink.Href == null ? null : selfLink.Href;
            if (personUri == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(personUri, Method.GET);
            return this.stateFactory.NewPersonState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
