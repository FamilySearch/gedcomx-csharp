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
    /// <summary>
    /// Represents a historical record of some operation performed (such as deleting a person).
    /// </summary>
    public class ChangeEntry : Entry
    {
        private readonly Entry entry;
        private readonly ChangeInfo changeInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeEntry"/> class.
        /// </summary>
        /// <param name="entry">The associated atom entry with this change.</param>
        public ChangeEntry(Entry entry)
        {
            this.entry = entry;
            this.changeInfo = this.entry.FindExtensionOfType<ChangeInfo>();
        }

        /// <summary>
        /// Gets the change information associated with this change.
        /// </summary>
        /// <value>
        /// The change information associated with this change.
        /// </value>
        public ChangeInfo ChangeInfo
        {
            get
            {
                return this.changeInfo;
            }
        }

        /// <summary>
        /// Gets the atom entry associated with this change.
        /// </summary>
        /// <value>
        /// The atom entry associated with this change.
        /// </value>
        public Entry Entry
        {
            get
            {
                return entry;
            }
        }

        /// <summary>
        /// Gets the change operation associated with this change (if available).
        /// </summary>
        /// <value>
        /// The change operation associated with this change (if available).
        /// </value>
        public ChangeOperation? Operation
        {
            get
            {
                return changeInfo == null ? null : (ChangeOperation?)changeInfo.KnownOperation;
            }
        }

        /// <summary>
        /// Gets the change object type associated with this change (if available).
        /// </summary>
        /// <value>
        /// The change object type associated with this change (if available).
        /// </value>
        public ChangeObjectType? ObjectType
        {
            get
            {
                return changeInfo == null ? null : (ChangeObjectType?)changeInfo.KnownObjectType;
            }
        }

        /// <summary>
        /// Gets the change object modifier associated with this change (if available).
        /// </summary>
        /// <value>
        /// The change object modifier associated with this change (if available).
        /// </value>
        public ChangeObjectModifier? ObjectModifier
        {
            get
            {
                return changeInfo == null ? null : (ChangeObjectModifier?)changeInfo.KnownObjectModifier;
            }
        }

        /// <summary>
        /// Gets the reason this change was performed (if available).
        /// </summary>
        /// <value>
        /// The reason this change was performed (if available).
        /// </value>
        public String Reason
        {
            get
            {
                return changeInfo == null ? null : changeInfo.Reason;
            }
        }

        /// <summary>
        /// Gets the original value from before the change.
        /// </summary>
        /// <value>
        /// The original value from before the change.
        /// </value>
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

        /// <summary>
        /// Gets the resulting value from after the change.
        /// </summary>
        /// <value>
        /// The resulting value from after the change.
        /// </value>
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

        /// <summary>
        /// Gets the value that was removed by the change.
        /// </summary>
        /// <value>
        /// The value that was removed by the change.
        /// </value>
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
