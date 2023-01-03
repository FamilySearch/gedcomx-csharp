using System.Collections.Generic;
using System.Linq;
using System.Net;

using FamilySearch.Api;
using FamilySearch.Api.Ft;

using Gx.Fs.Discussions;
using Gx.Rs.Api;

using NUnit.Framework;

namespace Gedcomx.Rs.Api.Test
{
    [TestFixture]
    public class DiscussionsTests
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
        public void TestCreateComment()
        {
            var discussion = (DiscussionState)tree.AddDiscussion(new Discussion().SetTitle("Comment").SetDetails("Comment")).Get();
            cleanup.Add(discussion);
            var state = discussion.AddComment(new Comment().SetText("Comment"));

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void TestReadComments()
        {
            var discussion = (DiscussionState)tree.AddDiscussion(new Discussion().SetTitle("Comment").SetDetails("Comment")).Get();
            cleanup.Add(discussion);
            discussion.AddComment(new Comment().SetText("Comment"));
            var state = discussion.LoadComments();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(state.Discussion, Is.Not.Null);
            Assert.That(state.Discussion.Comments, Is.Not.Null);
            Assert.That(state.Discussion.Comments, Is.Not.Empty);
        }

        [Test]
        public void TestUpdateComment()
        {
            var discussion = (DiscussionState)tree.AddDiscussion(new Discussion().SetTitle("Comment").SetDetails("Comment")).Get();
            cleanup.Add(discussion);
            discussion.AddComment(new Comment().SetText("Comment"));
            discussion.LoadComments();
            var comment = discussion.Discussion.Comments.First();
            var state = discussion.UpdateComment(comment);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestDeleteComment()
        {
            var discussion = (DiscussionState)tree.AddDiscussion(new Discussion().SetTitle("Comment").SetDetails("Comment")).Get();
            cleanup.Add(discussion);
            discussion.AddComment(new Comment().SetText("Comment"));
            discussion.LoadComments();
            var comment = discussion.Discussion.Comments.First();
            var state = discussion.DeleteComment(comment);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test]
        public void TestCreateDiscussion()
        {
            var state = tree.AddDiscussion(new Discussion().SetTitle("Comment").SetDetails("Comment"));
            cleanup.Add(state);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public void TestReadDiscussion()
        {
            var state = (DiscussionState)tree.AddDiscussion(new Discussion().SetTitle("Comment").SetDetails("Comment")).Get();
            cleanup.Add(state);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void TestUpdateDiscussion()
        {
            var discussion = (DiscussionState)tree.AddDiscussion(new Discussion().SetTitle("Comment").SetDetails("Comment")).Get();
            cleanup.Add(discussion);
            var state = discussion.Update(discussion.Discussion);

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Test, Category("AccountNeeded")]
        public void TestDeleteDiscussion()
        {
            var discussion = (DiscussionState)tree.AddDiscussion(new Discussion().SetTitle("Comment").SetDetails("Comment")).Get();
            var state = discussion.Delete();

            Assert.DoesNotThrow(() => state.IfSuccessful());
            Assert.That(state.Response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}
