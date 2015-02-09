using Gedcomx.Model.Rt;
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
    public interface IFamilySearchPlatformModelVisitor : IGedcomxModelVisitor
    {
        void VisitFamilySearchPlatform(FamilySearchPlatform fsp);

        void VisitChildAndParentsRelationship(ChildAndParentsRelationship pcr);

        void VisitMerge(MergeAnalysis merge);

        void VisitMerge(Merge merge);

        void VisitDiscussion(Discussion discussion);

        void VisitComment(Comment comment);

        void VisitUser(User user);
    }
}
