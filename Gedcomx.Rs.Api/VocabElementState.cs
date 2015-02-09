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
    /// <summary>
    /// The VocabElementState exposes management functions for a vocab element.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="VocabElementState"/> class.
        /// </summary>
        /// <param name="request">The REST API request that will be used to instantiate this state instance.</param>
        /// <param name="response">The REST API response that was produced from the REST API request.</param>
        /// <param name="client">The REST API client to use for API calls.</param>
        /// <param name="accessToken">The access token to use for subsequent invocations of the REST API client.</param>
        /// <param name="stateFactory">The state factory to use for state instantiation.</param>
        protected internal VocabElementState(IRestRequest request, IRestResponse response, IFilterableRestClient client, String accessToken, StateFactory stateFactory)
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
            return new VocabElementState(request, response, client, this.CurrentAccessToken, this.stateFactory);
        }

        /// <summary>
        /// Returns the <see cref="RDFDataset"/> from the REST API response.
        /// </summary>
        /// <param name="response">The REST API response.</param>
        /// <returns>The <see cref="RDFDataset"/> from the REST API response.</returns>
        protected override RDFDataset LoadEntity(IRestResponse response)
        {
            var token = JSONUtils.FromString(response.Content);
            model = (RDFDataset)JsonLdProcessor.ToRDF(token, options);
            defaultQuads = model.GetQuads("@default");

            return model;
        }

        /// <summary>
        /// Gets the vocab element represented by this state instance.
        /// </summary>
        /// <returns>The vocab element represented by this state instance.</returns>
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

        /// <summary>
        /// Gets the main data element represented by this state instance.
        /// </summary>
        /// <value>
        /// The main data element represented by this state instance.
        /// </value>
        /// <remarks>
        /// This class does not have a <see cref="ISupportsLinks"/> entity; therefore, this
        /// always returns null.
        /// </remarks>
        protected override ISupportsLinks MainDataElement
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the <see cref="RDFDataset"/> represented by this state instance.
        /// </summary>
        /// <value>
        /// The <see cref="RDFDataset"/> represented by this state instance.
        /// </value>
        private RDFDataset Model
        {
            get
            {
                return this.model;
            }
        }
    }
}
