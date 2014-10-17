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

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class MemoriesTests
    {
        private CollectionState collection;

        [TestFixtureSetUp]
        public void Initialize()
        {
            collection = new CollectionState(new Uri("https://sandbox.familysearch.org/platform/collections/memories"));
            collection.AuthenticateViaOAuth2Password(Resources.TestUserName, Resources.TestPassword, Resources.TestClientId);
        }

        [Test]
        public void TestUploadMultiplePhotoMemories()
        {
            var converter = new ImageConverter();
            var bytes1 = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var bytes2 = (Byte[])converter.ConvertTo(TestBacking.GetCreatePhoto(), typeof(Byte[]));
            var dataSource1 = TestBacking.GetDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes1);
            var dataSource2 = TestBacking.GetDataSource(Guid.NewGuid().ToString("n") + ".jpg", "image/jpeg", bytes2);
            var description = new SourceDescription().SetTitle(new TextValue().SetValue("PersonImage")).SetCitation("Citation for PersonImage");
            var artifacts = new List<DataSource>();

            artifacts.Add(dataSource1);
            artifacts.Add(dataSource2);

            IRestRequest request = new RestRequest()
                .AddHeader("Authorization", "Bearer " + collection.CurrentAccessToken)
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

            var state = collection.Client.Handle(request);

            Assert.IsNotNull(state);
            Assert.AreEqual(HttpStatusCode.Created, state.StatusCode);
        }

        [Test]
        public void TestUploadPdfDocument()
        {
            var dataSource = TestBacking.GetDataSource(Guid.NewGuid().ToString("n") + ".pdf", "application/pdf", TestBacking.GetCreatePdf());
            var state = collection.AddArtifact(dataSource);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
            state.Delete();
        }

        [Test]
        public void TestUploadStory()
        {
            var dataSource = TestBacking.GetDataSource(Guid.NewGuid().ToString("n") + ".txt", "text/plain", TestBacking.GetCreateTxt());
            var state = collection.AddArtifact(dataSource);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.AreEqual(HttpStatusCode.Created, state.Response.StatusCode);
            state.Delete();
        }
    }
}
