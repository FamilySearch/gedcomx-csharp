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
    public class RecordsState : GedcomxApplicationState<Feed>
    {
        internal RecordsState(IRestRequest request, IRestResponse response, IRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        protected override GedcomxApplicationState Clone(IRestRequest request, IRestResponse response, IRestClient client)
        {
            return new RecordsState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

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
