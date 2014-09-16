using Gx.Atom;
using Gx.Fs.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilySearch.Api.Util
{
    public class ChangeHistoryPage : Entry
    {

        private readonly Feed feed;
        private readonly List<ChangeEntry> entries;

        public ChangeHistoryPage(Feed feed)
        {
            this.feed = feed;

            List<Entry> entries = feed.Entries;
            List<ChangeEntry> changes = new List<ChangeEntry>();
            if (entries != null)
            {
                foreach (Entry entry in entries)
                {
                    changes.Add(new ChangeEntry(entry));
                }
            }

            this.entries = changes;
        }

        public Feed Feed
        {
            get
            {
                return feed;
            }
        }

        public List<ChangeEntry> Entries
        {
            get
            {
                return entries;
            }
        }

        public ChangeEntry FindChange(ChangeOperation operation, ChangeObjectType objectType)
        {
            return FindChange(operation, objectType, null);
        }

        public ChangeEntry FindChange(ChangeOperation operation, ChangeObjectType objectType, ChangeObjectModifier? modifier)
        {
            List<ChangeEntry> changes = FindChanges(operation, objectType, modifier);
            return changes.Count > 0 ? changes.First() : null;
        }

        public List<ChangeEntry> FindChanges(ChangeOperation operation, ChangeObjectType objectType)
        {
            return FindChanges(new ChangeOperation[] { operation }, new ChangeObjectType[] { objectType });
        }

        public List<ChangeEntry> FindChanges(ChangeOperation operation, ChangeObjectType objectType, ChangeObjectModifier? modifier)
        {
            return FindChanges(new ChangeOperation[] { operation }, new ChangeObjectType[] { objectType }, modifier != null ? new ChangeObjectModifier[] { modifier.Value } : Enum.GetValues(typeof(ChangeObjectModifier)).Cast<ChangeObjectModifier>());
        }

        public List<ChangeEntry> FindChanges(IEnumerable<ChangeOperation> operations, IEnumerable<ChangeObjectType> types)
        {
            return FindChanges(operations, types, Enum.GetValues(typeof(ChangeObjectModifier)).Cast<ChangeObjectModifier>());
        }

        public List<ChangeEntry> FindChanges(IEnumerable<ChangeOperation> operations, IEnumerable<ChangeObjectType> types, IEnumerable<ChangeObjectModifier> modifiers)
        {
            List<ChangeEntry> changes = new List<ChangeEntry>();
            foreach (ChangeEntry entry in this.entries)
            {
                ChangeOperation? operation = entry.Operation;
                ChangeObjectType? type = entry.ObjectType;
                ChangeObjectModifier? modifier = entry.ObjectModifier;
                if (operation != null && type != null & operations.Contains(operation.Value) && types.Contains(type.Value))
                {
                    if (modifier == null || modifiers.Contains(modifier.Value))
                    {
                        changes.Add(entry);
                    }
                }
            }
            return changes;
        }
    }
}
