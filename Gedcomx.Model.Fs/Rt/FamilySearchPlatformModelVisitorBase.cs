using Gedcomx.Model.Rt;
using Gx.Conclusion;
using Gx.Fs.Discussions;
using Gx.Fs.Tree;
using Gx.Fs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Fs.Rt
{
    public class FamilySearchPlatformModelVisitorBase : GedcomxModelVisitorBase, IFamilySearchPlatformModelVisitor
    {
        public void VisitFamilySearchPlatform(FamilySearchPlatform fsp)
        {
            VisitGedcomx(fsp);

            this.contextStack.Push(fsp);
            List<Discussion> discussions = fsp.Discussions;
            if (discussions != null)
            {
                foreach (Discussion discussion in discussions)
                {
                    discussion.Accept(this);
                }
            }

            List<Merge> merges = fsp.Merges;
            if (merges != null)
            {
                foreach (Merge merge in merges)
                {
                    merge.Accept(this);
                }
            }

            List<MergeAnalysis> mergeAnalyses = fsp.MergeAnalyses;
            if (mergeAnalyses != null)
            {
                foreach (MergeAnalysis merge in mergeAnalyses)
                {
                    merge.Accept(this);
                }
            }

            List<ChildAndParentsRelationship> childAndParentsRelationships = fsp.ChildAndParentsRelationships;
            if (childAndParentsRelationships != null)
            {
                foreach (ChildAndParentsRelationship pcr in childAndParentsRelationships)
                {
                    pcr.Accept(this);
                }
            }

            List<User> users = fsp.Users;
            if (users != null)
            {
                foreach (User user in users)
                {
                    user.Accept(this);
                }
            }

            this.contextStack.Pop();
        }

        public override void VisitGedcomx(Gedcomx gx)
        {
            base.VisitGedcomx(gx);
            this.contextStack.Push(gx);
            List<Discussion> discussions = gx.FindExtensionsOfType<Discussion>();
            if (discussions != null)
            {
                foreach (Discussion discussion in discussions)
                {
                    discussion.Accept(this);
                }
            }

            List<Merge> merges = gx.FindExtensionsOfType<Merge>();
            if (merges != null)
            {
                foreach (Merge merge in merges)
                {
                    merge.Accept(this);
                }
            }

            List<MergeAnalysis> mergeAnalyses = gx.FindExtensionsOfType<MergeAnalysis>();
            if (mergeAnalyses != null)
            {
                foreach (MergeAnalysis merge in mergeAnalyses)
                {
                    merge.Accept(this);
                }
            }

            List<ChildAndParentsRelationship> childAndParentsRelationships = gx.FindExtensionsOfType<ChildAndParentsRelationship>();
            if (childAndParentsRelationships != null)
            {
                foreach (ChildAndParentsRelationship pcr in childAndParentsRelationships)
                {
                    pcr.Accept(this);
                }
            }

            this.contextStack.Pop();
        }

        public virtual void VisitChildAndParentsRelationship(ChildAndParentsRelationship pcr)
        {
            this.contextStack.Push(pcr);
            VisitConclusion(pcr);

            List<Fact> facts;

            facts = pcr.FatherFacts;
            if (facts != null)
            {
                foreach (Fact fact in facts)
                {
                    fact.Accept(this);
                }
            }

            facts = pcr.MotherFacts;
            if (facts != null)
            {
                foreach (Fact fact in facts)
                {
                    fact.Accept(this);
                }
            }

            this.contextStack.Pop();
        }

        public void VisitMerge(MergeAnalysis merge)
        {
            //no-op.
        }

        public void VisitMerge(Merge merge)
        {
            //no-op.
        }

        public virtual void VisitDiscussion(Discussion discussion)
        {
            this.contextStack.Push(discussion);
            List<Comment> comments = discussion.Comments;
            if (comments != null)
            {
                foreach (Comment comment in comments)
                {
                    comment.Accept(this);
                }
            }
            this.contextStack.Pop();
        }

        public virtual void VisitComment(Comment comment)
        {
            //no-op.
        }

        public virtual void VisitUser(User user)
        {
            //no-op.
        }
    }
}
