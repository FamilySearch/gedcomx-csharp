using Gx.Atom;
using Gx.Fs.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilySearch.Api.Util
{
    /// <summary>
    /// Represents a page of change histories.
    /// </summary>
    public class ChangeHistoryPage : Entry
    {

        private readonly Feed feed;
        private readonly List<ChangeEntry> entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeHistoryPage"/> class.
        /// </summary>
        /// <param name="feed">The feed with change histories from which this page will be initialized.</param>
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

        /// <summary>
        /// Gets the feed associated with this page.
        /// </summary>
        /// <value>
        /// The feed associated with this page.
        /// </value>
        public Feed Feed
        {
            get
            {
                return feed;
            }
        }

        /// <summary>
        /// Gets the list of change entries associated with this page.
        /// </summary>
        /// <value>
        /// The list of change entries associated with this page.
        /// </value>
        public List<ChangeEntry> Entries
        {
            get
            {
                return entries;
            }
        }

        /// <summary>
        /// Searches the current page of change entries for the type of object and operation changed.
        /// </summary>
        /// <param name="operation">The change operation being sought.</param>
        /// <param name="objectType">The change object type being sought.</param>
        /// <returns>The first <see cref="ChangeEntry"/> matching the search conditions.</returns>
        public ChangeEntry FindChange(ChangeOperation operation, ChangeObjectType objectType)
        {
            return FindChange(operation, objectType, null);
        }

        /// <summary>
        /// Searches the current page of change entries for the type of object and operation changed and the modifier involved.
        /// </summary>
        /// <param name="operation">The change operation being sought.</param>
        /// <param name="objectType">The change object type being sought.</param>
        /// <param name="modifier">The change object modifier involved being sought.</param>
        /// <returns>
        /// The first <see cref="ChangeEntry" /> matching the search conditions.
        /// </returns>
        public ChangeEntry FindChange(ChangeOperation operation, ChangeObjectType objectType, ChangeObjectModifier? modifier)
        {
            List<ChangeEntry> changes = FindChanges(operation, objectType, modifier);
            return changes.Count > 0 ? changes.First() : null;
        }

        /// <summary>
        /// Searches the current page of change entries for the type of object and operation changed.
        /// </summary>
        /// <param name="operation">The change operation being sought.</param>
        /// <param name="objectType">The change object type being sought.</param>
        /// <returns>
        /// The list of <see cref="ChangeEntry" />s matching the search conditions.
        /// </returns>
        public List<ChangeEntry> FindChanges(ChangeOperation operation, ChangeObjectType objectType)
        {
            return FindChanges(new ChangeOperation[] { operation }, new ChangeObjectType[] { objectType });
        }

        /// <summary>
        /// Searches the current page of change entries for the type of object and operation changed and the modifier involved.
        /// </summary>
        /// <param name="operation">The change operation being sought.</param>
        /// <param name="objectType">The change object type being sought.</param>
        /// <param name="modifier">The change object modifier involved being sought.</param>
        /// <returns>
        /// The list of <see cref="ChangeEntry" />s matching the search conditions.
        /// </returns>
        public List<ChangeEntry> FindChanges(ChangeOperation operation, ChangeObjectType objectType, ChangeObjectModifier? modifier)
        {
            return FindChanges(new ChangeOperation[] { operation }, new ChangeObjectType[] { objectType }, modifier != null ? new ChangeObjectModifier[] { modifier.Value } : Enum.GetValues(typeof(ChangeObjectModifier)).Cast<ChangeObjectModifier>());
        }

        /// <summary>
        /// Searches the current page of change entries for the type of object and operation changed.
        /// </summary>
        /// <param name="operations">The collection of change operations being sought.</param>
        /// <param name="types">The collection of change object types being sought.</param>
        /// <returns>
        /// The list of <see cref="ChangeEntry" />s matching the search conditions.
        /// </returns>
        public List<ChangeEntry> FindChanges(IEnumerable<ChangeOperation> operations, IEnumerable<ChangeObjectType> types)
        {
            return FindChanges(operations, types, Enum.GetValues(typeof(ChangeObjectModifier)).Cast<ChangeObjectModifier>());
        }

        /// <summary>
        /// Searches the current page of change entries for the type of object and operation changed.
        /// </summary>
        /// <param name="operations">The collection of change operations being sought.</param>
        /// <param name="types">The collection of change object types being sought.</param>
        /// <param name="modifiers">The collection of change object modifiers involved being sought.</param>
        /// <returns>
        /// The list of <see cref="ChangeEntry" />s matching the search conditions.
        /// </returns>
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
