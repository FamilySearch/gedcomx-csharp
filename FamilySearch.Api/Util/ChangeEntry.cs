using Gx.Atom;
using Gx.Common;
using Gx.Fs.Rt;
using Gx.Fs.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilySearch.Api.Util
{
    public class ChangeEntry : Entry
    {
        private readonly Entry entry;
        private readonly ChangeInfo changeInfo;

        public ChangeEntry(Entry entry)
        {
            this.entry = entry;
            this.changeInfo = this.entry.FindExtensionOfType<ChangeInfo>();
        }

        public ChangeInfo ChangeInfo
        {
            get
            {
                return this.changeInfo;
            }
        }

        public Entry Entry
        {
            get
            {
                return entry;
            }
        }

        public ChangeOperation? Operation
        {
            get
            {
                return changeInfo == null ? null : (ChangeOperation?)changeInfo.KnownOperation;
            }
        }

        public ChangeObjectType? ObjectType
        {
            get
            {
                return changeInfo == null ? null : (ChangeObjectType?)changeInfo.KnownObjectType;
            }
        }

        public ChangeObjectModifier? ObjectModifier
        {
            get
            {
                return changeInfo == null ? null : (ChangeObjectModifier?)changeInfo.KnownObjectModifier;
            }
        }

        public String Reason
        {
            get
            {
                return changeInfo == null ? null : changeInfo.Reason;
            }
        }

        public ExtensibleData OriginalValue
        {
            get
            {
                ChangeInfo changeInfo = ChangeInfo;
                if (changeInfo != null && Entry.Content != null && Entry.Content.Gedcomx != null)
                {
                    ResourceReference original = changeInfo.Original;
                    if (original != null)
                    {
                        return FamilySearchPlatformLocalReferenceResolver.Resolve(original, Entry.Content.Gedcomx);
                    }
                }

                return null;
            }
        }

        public ExtensibleData ResultingValue
        {
            get
            {
                ChangeInfo changeInfo = ChangeInfo;
                if (changeInfo != null && Entry.Content != null && Entry.Content.Gedcomx != null)
                {
                    ResourceReference resulting = changeInfo.Resulting;
                    if (resulting != null)
                    {
                        return FamilySearchPlatformLocalReferenceResolver.Resolve(resulting, Entry.Content.Gedcomx);
                    }
                }

                return null;
            }
        }

        public ExtensibleData RemovedValue
        {
            get
            {
                ChangeInfo changeInfo = ChangeInfo;
                if (changeInfo != null && Entry.Content != null && Entry.Content.Gedcomx != null)
                {
                    ResourceReference removed = changeInfo.Removed;
                    if (removed != null)
                    {
                        return FamilySearchPlatformLocalReferenceResolver.Resolve(removed, Entry.Content.Gedcomx);
                    }
                }

                return null;
            }
        }
    }
}
