using Gedcomx.Model;
using Gx.Rs.Api.Util;
using JsonLD.Core;
using JsonLD.Util;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gx.Rs.Api
{
    public class VocabElementState : GedcomxApplicationState<RDFDataset>
    {
        private RDFDataset model;
        private IEnumerable<RDFDataset.Quad> defaultQuads;
        private static JsonLdOptions options;

        static VocabElementState()
        {
            options = new JsonLdOptions();
            options.useNamespaces = true;
        }

        protected internal VocabElementState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
            : base(request, response, client, accessToken, stateFactory)
        {
        }

        public override String SelfRel
        {
            get
            {
                return Rel.DESCRIPTION;
            }
        }

        protected override GedcomxApplicationState<RDFDataset> Clone(IRestRequest request, IRestResponse response, IFilterableRestClient client)
        {
            return new VocabElementState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        protected override RDFDataset LoadEntity(IRestResponse response)
        {
            var token = JSONUtils.FromString(response.Content);
            model = (RDFDataset)JsonLdProcessor.ToRDF(token, options);
            defaultQuads = model.GetQuads("@default");

            return model;
        }

        public VocabElement GetVocabElement()
        {
            VocabElement vocabElement = new VocabElement();

            // Map required attributes into the VocabElement
            vocabElement.Id = defaultQuads.GetPredicateQuad(VocabConstants.DC_NAMESPACE + "identifier").GetObject().GetValue();
            vocabElement.Uri = defaultQuads.First().GetSubject().GetValue();

            // Get optional attributes into the VocabElement
            String property = VocabConstants.RDFS_NAMESPACE + "subClassOf";
            if (defaultQuads.HasPredicateQuad(property))
            {
                vocabElement.Subclass = defaultQuads.GetPredicateQuad(property).GetObject().GetValue();
            }
            property = VocabConstants.DC_NAMESPACE + "type";
            if (defaultQuads.HasPredicateQuad(property))
            {
                vocabElement.Type = defaultQuads.GetPredicateQuad(property).GetObject().GetValue();
            }

            // Map the labels into the VocabElement
            var labels = defaultQuads.GetPredicateQuads(VocabConstants.RDFS_NAMESPACE + "label");
            if (labels != null)
            {
                foreach (var label in labels)
                {
                    var node = label.GetObject();
                    vocabElement.AddLabel(node.GetValue(), node.GetLanguage().ToLower());
                }
            }

            // Map the descriptions into the VocabElement
            var descriptions = defaultQuads.GetPredicateQuads(VocabConstants.RDFS_NAMESPACE + "comment");
            if (descriptions != null)
            {
                foreach (var description in descriptions)
                {
                    var node = description.GetObject();
                    vocabElement.AddDescription(node.GetValue(), node.GetLanguage().ToLower());
                }
            }
            return vocabElement;
        }

        protected override SupportsLinks MainDataElement
        {
            get
            {
                return null;
            }
        }

        private RDFDataset Model
        {
            get
            {
                return this.model;
            }
        }
    }
}
