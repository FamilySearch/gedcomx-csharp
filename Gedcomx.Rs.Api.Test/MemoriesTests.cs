using FamilySearch.Api.Ft;
using Gedcomx.Support;
using Gx.Common;
using Gx.Rs.Api.Util;
using Gx.Source;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Extensions;
using System.Net;
using Gx.Rs.Api;
using FamilySearch.Api.Memories;
using Gx.Conclusion;
using Gx.Types;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class MemoriesTests
    {
        private FamilySearchFamilyTree memories;

        [TestFixtureSetUp]
        public void Initialize()
        {
            memories = new FamilySearchFamilyTree(true);
            memories.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
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
            var artifacts = new List<DataSource>();

            artifacts.Add(dataSource1);
            artifacts.Add(dataSource2);

            IRestRequest request = new RestRequest()
                .AddHeader("Authorization", "Bearer " + memories.CurrentAccessToken)
                .Accept(MediaTypes.GEDCOMX_JSON_MEDIA_TYPE)
                .ContentType(MediaTypes.MULTIPART_FORM_DATA_TYPE)
                .Build("https://sandbox.familysearch.org/platform/memories/memories", Method.POST);

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

            var state = memories.Client.Handle(request);

            Assert.IsNotNull(state);
            Assert.AreEqual(HttpStatusCode.Created, state.StatusCode);
        }

        [Test]
        public void TestUploadPdfDocument()
        {
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".pdf", "application/pdf", TestBacking.GetCreatePdf());
            var state = memories.AddArtifact(dataSource);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
            state.Delete();
        }

        [Test]
        public void TestUploadStory()
        {
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".txt", "text/plain", TestBacking.GetCreateTxt());
            var state = memories.AddArtifact(dataSource);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
            state.Delete();
        }

        [Test]
        public void TestReadMemory()
        {
            var converter = new ImageConverter();
            var bytes = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
            var image = memories.AddArtifact(new SourceDescription().SetTitle("PersonImage").SetCitation("Citation for PersonImage"), dataSource);
            var state = image.Get();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            image.Delete();
        }

        [Test]
        public void TestUpdateMemoryDescription()
        {
            var converter = new ImageConverter();
            var bytes = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
            var image = (SourceDescriptionState)memories.AddArtifact(new SourceDescription().SetTitle("PersonImage").SetCitation("Citation for PersonImage"), dataSource).Get();
            image.SourceDescription.SetDescription("Test description");
            var state = image.Update(image.SourceDescription);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
            image.Delete();
        }

        [Test]
        public void TestDeleteMemory()
        {
            var converter = new ImageConverter();
            var bytes = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
            var image = (SourceDescriptionState)memories.AddArtifact(new SourceDescription().SetTitle("PersonImage").SetCitation("Citation for PersonImage"), dataSource).Get();
            var state = image.Delete();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.NoContent, state.Response.StatusCode);
        }

        [Test]
        public void TestCreateMemoryPersona()
        {
            var converter = new ImageConverter();
            var bytes = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
            var description = new SourceDescription().SetTitle("PersonImage").SetCitation("Citation for PersonImage").SetDescription("Description");
            var image = (SourceDescriptionState)memories.AddArtifact(description, dataSource).Get();
            var state = image.AddPersona(new Person().SetName("John Smith"));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
            image.Delete();
        }

        [Test]
        public void TestReadMemoryPersonas()
        {
            var converter = new ImageConverter();
            var bytes = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource = new BasicDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes);
            var description = new SourceDescription().SetTitle("PersonImage").SetCitation("Citation for PersonImage").SetDescription("Description");
            var image = (SourceDescriptionState)memories.AddArtifact(description, dataSource).Get();
            image.AddPersona(new Person().SetName("John Smith"));
            var state = image.ReadPersonas();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.OK, state.Response.StatusCode);
            Assert.IsNotNull(state.Persons);
            Assert.AreEqual(1, state.Persons.Count);
            image.Delete();
        }
    }
}
