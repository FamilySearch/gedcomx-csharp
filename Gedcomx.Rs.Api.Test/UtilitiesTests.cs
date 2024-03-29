﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using FamilySearch.Api.Ft;
using FamilySearch.Api.Util;

using Gedcomx.Support;

using Gx.Fs;
using Gx.Rs.Api;
using Gx.Rs.Api.Util;

using NUnit.Framework;

using RestSharp;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class UtilitiesTests
    {
        private FamilySearchFamilyTree tree;
        private List<GedcomxApplicationState> cleanup;

        [OneTimeSetUp]
        public void Initialize()
        {
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            cleanup = new List<GedcomxApplicationState>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            foreach (var state in cleanup)
            {
                state.Delete();
            }
        }

        [Test]
        public void TestReadPersonWithMultiplePendingModificationsActivated()
        {
            // Make a separate copy from the main tree used in other tests
            var tempTree = new FamilySearchFamilyTree(true);
            var features = new List<Feature>();

            tempTree.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);

            // Get all the features that are pending
            IRestRequest request = new RedirectableRestRequest()
                .Accept(MediaTypes.APPLICATION_JSON_TYPE)
                .Build("https://api-integ.familysearch.org/platform/pending-modifications", Method.GET);
            IRestResponse response = tempTree.Client.Handle(request);

            // Get each pending feature
            features.AddRange(response.ToIRestResponse<FamilySearchPlatform>().Data.Features);

            // Add every pending feature to the tree's current client
            tempTree.Client.AddFilter(new ExperimentsFilter(features.Select(x => x.Name).ToArray()));

            var state = tempTree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(state);

            // Ensure a response came back
            Assert.That(state, Is.Not.Null);
            var requestedFeatures = String.Join(",", state.Request.GetHeaders().Get("X-FS-Feature-Tag").Select(x => x.Value.ToString()));
            // Ensure each requested feature was found in the request headers
            Assert.That(features.TrueForAll(x => requestedFeatures.Contains(x.Name)), Is.True);
        }

        [Test]
        public void TestReadPersonWithPendingModificationActivated()
        {
            // The default client is assumed to add a single pending feature (if it doesn't, this test will fail)
            var state = tree.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);

            Assert.That(state, Is.Not.Null);
            var requestedFeatures = String.Join(",", state.Request.GetHeaders().Get("X-FS-Feature-Tag").Select(x => x.Value.ToString()));
            Assert.That(requestedFeatures, Is.Not.Null);
            Assert.That(requestedFeatures.IndexOf(","), Is.EqualTo(-1));
            Assert.That(requestedFeatures, Is.Not.Empty);
        }

        [Test, Category("AccountNeeded")]
        public void TestRedirectToPerson()
        {
            var person = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(person);
            var id = person.Response.Headers.Get("X-ENTITY-ID").Single().Value.ToString();
            IRestRequest request = new RedirectableRestRequest()
                .Accept(MediaTypes.APPLICATION_JSON_TYPE)
                .Build("https://api-integ.familysearch.org/platform/redirect?person=" + id, Method.GET);
            var response = tree.Client.Execute(request);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.TemporaryRedirect));
        }

        [Test, Category("AccountNeeded")]
        public void TestRedirectToPersonMemories()
        {
            var person = tree.AddPerson(TestBacking.GetCreateMalePerson());
            cleanup.Add(person);
            var id = person.Response.Headers.Get("X-ENTITY-ID").Single().Value.ToString();
            IRestRequest request = new RedirectableRestRequest()
                .Accept(MediaTypes.APPLICATION_JSON_TYPE)
                .Build("https://api-integ.familysearch.org/platform/redirect?context=memories&person=" + id, Method.GET);
            var response = tree.Client.Execute(request);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.TemporaryRedirect));
        }

        [Test, Category("AccountNeeded")]
        public void TestRedirectToSourceLinker()
        {
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            var uri = String.Format("https://api-integ.familysearch.org/platform/redirect?context=sourcelinker&person={0}&hintId={1}", person.Person.Id, person.Person.Identifiers[0].Value);
            IRestRequest request = new RedirectableRestRequest()
                .Accept(MediaTypes.APPLICATION_JSON_TYPE)
                .Build(uri, Method.GET);
            var response = tree.Client.Execute(request);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.TemporaryRedirect));
        }

        [Test, Category("AccountNeeded")]
        public void TestRedirectToUri()
        {
            IRestRequest request = new RedirectableRestRequest()
                .Accept(MediaTypes.APPLICATION_JSON_TYPE)
                .Build("https://api-integ.familysearch.org/platform/redirect?uri=https://www.familysearch.org/some/path?p1%3Dp1-value%26p2%3Dp2-value", Method.GET);
            var response = tree.Client.Execute(request);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.TemporaryRedirect));
        }
    }
}
