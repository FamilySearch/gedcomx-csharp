using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gx.Rs.Api.Util;
using Gx.Atom;
using Gx.Links;

namespace Gx.Rs.Api
{
    /// <summary>
    /// The RecordsState exposes management functions for a collection of records.
    /// </summary>
    public class RecordsState : GedcomxApplicationState<Feed>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordsState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        internal RecordsState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
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
            return new RecordsState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        /// <summary>
        /// Gets the set of records represented by this state instance.
        /// </summary>
        /// <value>
        /// The set of records represented by this state instance.
        /// </value>
        public List<Gedcomx> Records
        {
            get
            {
                List<Gedcomx> records = null;

                Feed feed = Entity;
                if (feed != null && feed.Entries != null && feed.Entries.Count > 0)
                {
                    records = new List<Gedcomx>();
                    foreach (Entry entry in feed.Entries)
                    {
                        if (entry.Content != null && entry.Content.Gedcomx != null)
                        {
                            records.Add(entry.Content.Gedcomx);
                        }
                    }
                }

                return records;
            }
        }

        /// <summary>
        /// Reads the specified record.
        /// </summary>
        /// <param name="entry">The entry record to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RecordState"/> instance containing the REST API response.
        /// </returns>
        public RecordState ReadRecord(Entry entry, params StateTransitionOption[] options)
        {
            Link link = entry.GetLink(Rel.RECORD);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewRecordState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }

        /// <summary>
        /// Reads the Gedcomx record.
        /// </summary>
        /// <param name="record">The Gedcomx record to read.</param>
        /// <param name="options">The options to apply before executing the REST API call.</param>
        /// <returns>
        /// A <see cref="RecordState"/> instance containing the REST API response.
        /// </returns>
        public RecordState ReadRecord(Gedcomx record, params StateTransitionOption[] options)
        {
            Link link = record.GetLink(Rel.RECORD);
            if (link == null || link.Href == null)
            {
                return null;
            }

            IRestRequest request = CreateAuthenticatedGedcomxRequest().Build(link.Href, Method.GET);
            return this.stateFactory.NewRecordState(request, Invoke(request, options), this.Client, this.CurrentAccessToken);
        }
    }
}
