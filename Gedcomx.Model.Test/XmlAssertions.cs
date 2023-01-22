using Gx.Agent;
using Gx.Common;
using Gx.Conclusion;
using Gx.Links;
using Gx.Source;

using NUnit.Framework;

namespace Gedcomx.Model.Test
{
    public static class XmlAssertions
    {
        public static void ShouldContain(this string result, Agent agent)
        {
            Assert.That(result, Does.Contain("<agent "));
            Assert.That(result, Does.Contain("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\""));
            Assert.That(result, Does.Contain("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\""));
            Assert.That(result, Does.Contain("xmlns=\"http://gedcomx.org/v1/\""));
            Assert.That(result.Contains("<account"), Is.EqualTo(agent.AnyAccounts()));
            Assert.That(result.Contains("<address"), Is.EqualTo(agent.AnyAddresses()));
            Assert.That(result.Contains("<email"), Is.EqualTo(agent.AnyEmails()));
            Assert.That(result.Contains("<homepage"), Is.EqualTo(agent.Homepage != null));
            Assert.That(result.Contains("<identifier"), Is.EqualTo(agent.AnyIdentifiers()));
            Assert.That(result.Contains("<name"), Is.EqualTo(agent.AnyNames()));
            Assert.That(result.Contains("<openid"), Is.EqualTo(agent.Openid != null));
            Assert.That(result.Contains("<phone"), Is.EqualTo(agent.AnyPhones()));
            result.ShouldContain(agent as HypermediaEnabledData);
        }

        public static void ShouldContain(this string result, SourceDescription sourceDescription)
        {
            Assert.That(result, Does.Contain("<sourceDescription "));
            Assert.That(result, Does.Contain("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\""));
            Assert.That(result, Does.Contain("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\""));
            Assert.That(result, Does.Contain("xmlns=\"http://gedcomx.org/v1/\""));
            Assert.That(result.Contains("about="), Is.EqualTo(sourceDescription.About != null));
            Assert.That(result.Contains("lang="), Is.EqualTo(sourceDescription.Lang != null));
            Assert.That(result.Contains("mediaType="), Is.EqualTo(sourceDescription.MediaType != null));
            Assert.That(result.Contains("sortKey="), Is.EqualTo(sourceDescription.SortKey != null));
            Assert.That(result.Contains("resourceType="), Is.EqualTo(sourceDescription.ResourceType != null));
            Assert.That(result.Contains("<citation"), Is.EqualTo(sourceDescription.AnyCitations()));
            Assert.That(result.Contains("<mediator"), Is.EqualTo(sourceDescription.Mediator != null));
            Assert.That(result.Contains("<source "), Is.EqualTo(sourceDescription.AnySources()));
            Assert.That(result.Contains("<analysis"), Is.EqualTo(sourceDescription.Analysis != null));
            Assert.That(result.Contains("<componentOf "), Is.EqualTo(sourceDescription.ComponentOf != null));
            Assert.That(result.Contains("<title>"), Is.EqualTo(sourceDescription.AnyTitles()));
            Assert.That(result.Contains("<titleLabel"), Is.EqualTo(sourceDescription.TitleLabel != null));
            Assert.That(result.Contains("<note"), Is.EqualTo(sourceDescription.AnyNotes()));
            Assert.That(result.Contains("<description"), Is.EqualTo(sourceDescription.AnyDescriptions()));
            Assert.That(result.Contains("<identifier"), Is.EqualTo(sourceDescription.AnyIdentifiers()));
            Assert.That(result.Contains("<coverage"), Is.EqualTo(sourceDescription.AnyCoverage()));
            Assert.That(result.Contains("<rights"), Is.EqualTo(sourceDescription.AnyRights()));
            Assert.That(result.Contains("<field"), Is.EqualTo(sourceDescription.AnyFields()));
            result.ShouldContain(sourceDescription as HypermediaEnabledData);
        }

        public static void ShouldContain(this string result, HypermediaEnabledData hypermediaEnabledData)
        {
            Assert.That(result.Contains("<link"), Is.EqualTo(hypermediaEnabledData.AnyLinks()));
            result.ShouldContain(hypermediaEnabledData as ExtensibleData);
        }

        public static void ShouldContain(this string result, OnlineAccount onlineAccount)
        {
            Assert.That(result, Does.Contain("<OnlineAccount "));
            Assert.That(result, Does.Contain("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\""));
            Assert.That(result, Does.Contain("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\""));
            Assert.That(result.Contains("accountName"), Is.EqualTo(onlineAccount.AccountName != null));
            Assert.That(result.Contains("serviceHomepage"), Is.EqualTo(onlineAccount.ServiceHomepage != null));
            result.ShouldContain(onlineAccount as ExtensibleData);
        }

        public static void ShouldContain(this string result, ExtensibleData extensibleData)
        {
            Assert.That(result.Contains("id"), Is.EqualTo(extensibleData.Id != null));
        }
    }
}
