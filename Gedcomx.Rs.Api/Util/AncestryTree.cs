using Gx.Conclusion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// A model representation of ancestry.
    /// </summary>
    public class AncestryTree
    {
        private readonly List<Person> ancestry;

        /// <summary>
        /// Initializes a new instance of the <see cref="AncestryTree"/> class.
        /// </summary>
        /// <param name="gx">The input model for which an ancestry model will be built.</param>
        public AncestryTree(Gedcomx gx)
        {
            this.ancestry = BuildArray(gx);
        }

        /// <summary>
        /// Builds an array of persons to be placed in the ancestry tree.
        /// </summary>
        /// <param name="gx">The input model for which the array of persons will be parsed and analyzed.</param>
        /// <returns>An array of persons to be placed in the ancestry tree.</returns>
        protected List<Person> BuildArray(Gedcomx gx)
        {
            List<Person> ancestry = new List<Person>();
            if (gx.Persons != null)
            {
                foreach (Person person in gx.Persons)
                {
                    DisplayProperties display = person.DisplayExtension;
                    if (display != null && display.AscendancyNumber != null)
                    {
                        try
                        {
                            int number = int.Parse(display.AscendancyNumber);
                            while (ancestry.Count < number)
                            {
                                ancestry.Add(null);
                            }
                            ancestry[number - 1] = person;
                        }
                        catch (FormatException)
                        {
                            //fall through...
                        }
                    }
                }
            }
            return ancestry;
        }

        /// <summary>
        /// Gets the root person of the ancestry tree.
        /// </summary>
        /// <value>
        /// The root person of the ancestry tree.
        /// </value>
        public AncestryNode Root
        {
            get
            {
                return GetAncestor(1);
            }
        }

        /// <summary>
        /// Gets an ancestor from the ancestry tree at the specified index.
        /// </summary>
        /// <param name="number">The parsed ahnen number of the person within the ancestry tree, which is used internally as a one-based index number. See remarks.</param>
        /// <returns>An ancestor node from the current tree, or null if the index is out of range.</returns>
        /// <remarks>
        /// Information on the ahnen number can be found here: http://en.wikipedia.org/wiki/Ahnentafel.
        /// </remarks>
        public AncestryNode GetAncestor(int number)
        {
            return ancestry.Count < number ? null : new AncestryNode(this, number);
        }

        /// <summary>
        /// Represents an ancestor within an ancestry tree.
        /// </summary>
        public class AncestryNode
        {
            private readonly int number;
            private readonly AncestryTree tree;

            /// <summary>
            /// Initializes a new instance of the <see cref="AncestryNode"/> class.
            /// </summary>
            /// <param name="tree">The ancestry tree in which this person belongs.</param>
            /// <param name="number">The one-based index number of where this person is in the specified tree. See remarks.</param>
            /// <remarks>
            /// When calling this constructor, the number parameter is one-based, but the tree indexing is zero-based, and the difference is adjusted automatically.
            /// </remarks>
            public AncestryNode(AncestryTree tree, int number)
            {
                this.number = number;
                this.tree = tree;
            }

            /// <summary>
            /// Gets the current person this node represents.
            /// </summary>
            /// <value>
            /// The current person this node represents.
            /// </value>
            /// <remarks>
            /// This is simply the person in the tree at the index specified in the constructor.
            /// </remarks>
            public Person Person
            {
                get
                {
                    return tree.ancestry[this.number - 1];
                }
            }

            /// <summary>
            /// Gets the father of the person represented by this node.
            /// </summary>
            /// <value>
            /// The father of the person represented by this node.
            /// </value>
            /// <remarks>
            /// The actual index of the father will be the index of the current person multiplied by 2.
            /// </remarks>
            public AncestryNode Father
            {
                get
                {
                    return tree.GetAncestor(this.number * 2);
                }
            }

            /// <summary>
            /// Gets the mother of the person represented by this node.
            /// </summary>
            /// <value>
            /// The mother of the person represented by this node.
            /// </value>
            /// <remarks>
            /// The actual index of the mother will be the index of the current person multiplied by 2 plus 1.
            /// </remarks>
            public AncestryNode Mother
            {
                get
                {
                    return tree.GetAncestor((this.number * 2) + 1);
                }
            }
        }
    }
}
