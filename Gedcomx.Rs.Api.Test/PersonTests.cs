using Gx.Atom;
using Gx.Links;
using Gx.Rs.Api;
using Gx.Rs.Api.Util;
using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class PersonTests
    {
        //public static readonly String SANDBOX_URI = "https://sandbox.familysearch.org/platform/collections/tree/persons/KWQ7-Y57";
        public static readonly String SANDBOX_URI = "https://sandbox.familysearch.org/platform/collections/tree";

        [Test]
        public void TestSearchForPersonsAndReadPerson()
        {
            CollectionState collection = new CollectionState(new Uri(SANDBOX_URI));
            collection.AuthenticateViaOAuth2Password("sdktester", "1234sdkpass", "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK");

            GedcomxPersonSearchQueryBuilder query = new GedcomxPersonSearchQueryBuilder()
                .Name("John Smith")
                .BirthDate("1 January 1900")
                .FatherName("Peter Smith");

            PersonSearchResultsState results = collection.SearchForPersons(query);

            Assert.DoesNotThrow(() => results.IfSuccessful());
            PersonState person = results.ReadPerson(results.Results.Entries.FirstOrDefault());

            Assert.IsNotNull(person);
            Assert.DoesNotThrow(() => person.IfSuccessful());
        }
    }
}
