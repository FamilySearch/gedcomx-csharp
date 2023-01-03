using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using FamilySearch.Api;
using FamilySearch.Api.Ft;
using FamilySearch.Api.Memories;

using Gedcomx.Support;

using Gx.Common;
using Gx.Conclusion;
using Gx.Fs.Discussions;
using Gx.Rs.Api;
using Gx.Rs.Api.Util;
using Gx.Source;

using NUnit.Framework;

using RestSharp;
using RestSharp.Extensions;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class MemoriesTests
    {
        private FamilySearchFamilyTree tree;
        private FamilySearchMemories memories;
        private List<GedcomxApplicationState> cleanup;

        [OneTimeSetUp]
        public void Initialize()
        {
            tree = new FamilySearchFamilyTree(true);
            tree.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
            memories = new FamilySearchMemories(true);
            memories = (FamilySearchMemories)memories.AuthenticateWithAccessToken(tree.CurrentAccessToken).Get();
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
        public void TestUploadMultiplePhotoMemories()
        {
            var converter = new ImageConverter();
            var bytes1 = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var bytes2 = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource1 = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes1);
            var dataSource2 = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes2);
            var description = new SourceDescription().SetTitle(new TextValue().SetValue("PersonImage")).SetCitation("Citation for PersonImage");
            var artifacts = new List<IDataSource>();

            artifacts.Add(dataSource1);
            artifacts.Add(dataSource2);

            IRestRequest request = new RedirectableRestRequest()
              .AddHeader("Authorization", "Bearer " + tree.CurrentAccessToken)
              .Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE)
              .ContentType(MediaTypes.MULTIPART_FORM_DATA_TYPE)
              .Build("https://api-integ.familysearch.org/platform/memories/memories", Method.POST);

            foreach (var artifact in artifacts)
            {
                String mediaType = artifact.ContentType;

                foreach (TextValue value in description.Titles)
                {
                    request.AddFile("title", Encoding.UTF8.GetBytes(value.Value), null, MediaTypes.TEXT_PLAIN_TYPE);
                }

                foreach (SourceCitation citation in description.Citations)
                {
                    request.AddFile("citation", Encoding.UTF8.GetBytes(citation.Value), null, MediaTypes.TEXT_PLAIN_TYPE);
                }

                if (artifact.Name != null)
                {
                    request.Files.Add(new FileParameter()
                    {
                        Name = "artifact",
                        FileName = artifact.Name,
                        ContentType = artifact.ContentType,
                        Writer = new Action<Stream>(s =>
                        {
                            artifact.InputStream.Seek(0, SeekOrigin.Begin);
                            using (var ms = new MemoryStream(artifact.InputStream.ReadAsBytes()))
                            using (var reader = new StreamReader(ms))
                            {
                                reader.BaseStream.CopyTo(s);
                            }
                        })
                    });
                }
            }

            var state = tree.Client.Handle(request);

            Assert.That(state, Is.Not.Null);
            Assert.That(state.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void TestUploadPdfDocument()
        {
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".pdf", "application/pdf", TestBacking.GetCreatePdf());
            var state = tree.AddArtifact(dataSource);
            cleanup.Add(state);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            state.Delete();
        }

        [Test]
        public void TestUploadStory()
        {
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".txt", "text/plain", TestBacking.GetCreateTxt());
            var state = tree.AddArtifact(dataSource);
            cleanup.Add(state);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            state.Delete();
        }

        [Test]
        public void TestReadMemory()
        {
            var converter = new ImageConverter();
            var bytes = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
            var image = tree.AddArtifact(new SourceDescription().SetTitle("PersonImage").SetCitation("Citation for PersonImage"), dataSource);
            cleanup.Add(image);
            var state = image.Get();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            image.Delete();
        }

        [Test]
        public void TestUpdateMemoryDescription()
        {
            var converter = new ImageConverter();
            var bytes = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
            var image = (SourceDescriptionState)tree.AddArtifact(new SourceDescription().SetTitle("PersonImage").SetCitation("Citation for PersonImage"), dataSource).Get();
            cleanup.Add(image);
            image.SourceDescription.SetDescription("Test description");
            var state = image.Update(image.SourceDescription);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            image.Delete();
        }

        [Test]
        public void TestDeleteMemory()
        {
            var converter = new ImageConverter();
            var bytes = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
            var image = (SourceDescriptionState)tree.AddArtifact(new SourceDescription().SetTitle("PersonImage").SetCitation("Citation for PersonImage"), dataSource).Get();
            var state = image.Delete();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestCreateMemoryPersona()
        {
            var converter = new ImageConverter();
            var bytes = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
            var description = new SourceDescription().SetTitle("PersonImage").SetCitation("Citation for PersonImage").SetDescription("Description");
            var image = (SourceDescriptionState)tree.AddArtifact(description, dataSource).Get();
            cleanup.Add(image);
            var state = image.AddPersona(new Person().SetName("John Smith"));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void TestReadMemoryPersonas()
        {
            var converter = new ImageConverter();
            var bytes = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
            var description = new SourceDescription().SetTitle("PersonImage").SetCitation("Citation for PersonImage").SetDescription("Description");
            var image = (SourceDescriptionState)tree.AddArtifact(description, dataSource).Get();
            cleanup.Add(image);
            image.AddPersona(new Person().SetName("John Smith"));
            var state = image.ReadPersonas();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Persons, Is.Not.Null);
            Assert.That(state.Persons, Has.Count.EqualTo(1));
        }

        [Test, Category("AccountNeeded")]
        public void TestCreatePersonMemoryReference()
        {
            var converter = new ImageConverter();
            var bytes = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
            var description = new SourceDescription().SetTitle("PersonImage").SetCitation("Citation for PersonImage").SetDescription("Description");
            var image = (SourceDescriptionState)tree.AddArtifact(description, dataSource).Get();
            cleanup.Add(image);
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            var persona = (PersonState)image.AddPersona(new Person().SetName("John Smith")).Get();
            var state = person.AddPersonaReference(persona);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void TestReadPersonMemoryReferences()
        {
            var converter = new ImageConverter();
            var bytes = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
            var description = new SourceDescription().SetTitle("PersonImage").SetCitation("Citation for PersonImage").SetDescription("Description");
            var image = (SourceDescriptionState)tree.AddArtifact(description, dataSource).Get();
            cleanup.Add(image);
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            var persona = (PersonState)image.AddPersona(new Person().SetName("John Smith")).Get();
            person.AddPersonaReference(persona);
            var state = person.LoadPersonaReferences();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }


        [Test]
        public void TestDeletePersonMemoryReference()
        {
            var converter = new ImageConverter();
            var bytes = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
            var description = new SourceDescription().SetTitle("PersonImage").SetCitation("Citation for PersonImage").SetDescription("Description");
            var image = (SourceDescriptionState)tree.AddArtifact(description, dataSource).Get();
            cleanup.Add(image);
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            var persona = (PersonState)image.AddPersona(new Person().SetName("John Smith")).Get();
            person.AddPersonaReference(persona);

            // Confirm it is there.
            var state2 = tree.ReadPerson(new Uri(person.GetSelfUri()));
            Assert.That(state2.Person.Evidence, Is.Not.Empty);

            // Now delete it
            var state = state2.DeletePersonaReference(state2.Person.Evidence.Single());
            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestUpdateMemoryPersona()
        {
            var converter = new ImageConverter();
            var bytes = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
            var description = new SourceDescription().SetTitle("PersonImage").SetCitation("Citation for PersonImage").SetDescription("Description");
            var image = (SourceDescriptionState)tree.AddArtifact(description, dataSource).Get();
            cleanup.Add(image);
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            var persona = (PersonState)image.AddPersona(new Person().SetName("John Smith")).Get();
            person.AddPersonaReference(persona);
            var personas = person.LoadPersonaReferences();
            var state = personas.Update(personas.Person);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        // Depreciated https://www.familysearch.org/developers/docs/api/tree/Person_Memory_References_resource
        // TODO: GetLink(Rel.EVIDENCE_REFERENCES) doesn't work. "evidence-references"
        [Test]
        public void TestDeleteMemoryPersona()
        {
            var converter = new ImageConverter();
            var bytes = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
            var description = new SourceDescription().SetTitle("PersonImage").SetCitation("Citation for PersonImage").SetDescription("Description");
            var image = (SourceDescriptionState)tree.AddArtifact(description, dataSource).Get();
            cleanup.Add(image);
            var person = (FamilyTreePersonState)tree.AddPerson(TestBacking.GetCreateMalePerson()).Get();
            cleanup.Add(person);
            var persona = (PersonState)image.AddPersona(new Person().SetName("John Smith")).Get();
            person.AddPersonaReference(persona);

            // Confirm it is there.
            var personas = tree.ReadPerson(new Uri(person.GetSelfUri()));
            Assert.That(personas.Person.Evidence, Is.Not.Empty);

            var state = personas.DeletePersonaReference(personas.GetPersonaReference());

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestReadMemoriesForAUser()
        {
            var state = memories.ReadResourcesOfCurrentUser();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.SourceDescriptions, Is.Not.Null);
        }

        [Test]
        public void TestCreateMemoriesComment()
        {
            var artifact = (FamilySearchSourceDescriptionState)memories.AddArtifact(new BasicDataSource("Sample Memory", MediaTypes.TEXT_PLAIN_TYPE, Resources.MemoryTXT)).Get();
            cleanup.Add(artifact);
            var comments = artifact.ReadComments();
            var state = comments.AddComment(new Comment().SetText("Test comment."));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void TestReadMemoriesComments()
        {
            var artifact = (FamilySearchSourceDescriptionState)memories.AddArtifact(new BasicDataSource("Sample Memory", MediaTypes.TEXT_PLAIN_TYPE, Resources.MemoryTXT)).Get();
            cleanup.Add(artifact);
            var state = artifact.ReadComments();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void TestUpdateMemoriesComment()
        {
            var artifact = (FamilySearchSourceDescriptionState)memories.AddArtifact(new BasicDataSource("Sample Memory", MediaTypes.TEXT_PLAIN_TYPE, Resources.MemoryTXT)).Get();
            cleanup.Add(artifact);
            var comments = artifact.ReadComments();
            comments.AddComment(new Comment().SetText("Test comment."));
            comments = artifact.ReadComments();
            var update = comments.Discussion.Comments.Single();
            update.SetText("Updated comment");
            var state = comments.UpdateComment(update);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestDeleteMemoriesComment()
        {
            var artifact = (FamilySearchSourceDescriptionState)memories.AddArtifact(new BasicDataSource("Sample Memory", MediaTypes.TEXT_PLAIN_TYPE, Resources.MemoryTXT)).Get();
            cleanup.Add(artifact);
            var comments = artifact.ReadComments();
            comments.AddComment(new Comment().SetText("Test comment."));
            comments = artifact.ReadComments();
            var delete = comments.Discussion.Comments.Single();
            var state = comments.DeleteComment(delete);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}
