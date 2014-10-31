using Gx.Conclusion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    /// <summary>
    /// A model representation of descendancy.
    /// </summary>
    public class DescendancyTree
    {
        private DescendancyNode root;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescendancyTree"/> class.
        /// </summary>
        /// <param name="gx">The input model for which a descendancy model will be built.</param>
        public DescendancyTree(Gedcomx gx)
        {
            this.root = BuildTree(gx);
        }

        /// <summary>
        /// Builds an array of persons to be placed in the descendancy tree.
        /// </summary>
        /// <param name="gx">The input model for which the array of persons will be parsed and analyzed.</param>
        /// <returns>An array of persons to be placed in the descendancy tree.</returns>
        protected DescendancyNode BuildTree(Gedcomx gx)
        {
            DescendancyNode root = null;
            if (gx.Persons != null && gx.Persons.Count > 0)
            {
                List<DescendancyNode> rootArray = new List<DescendancyNode>();
                foreach (Person person in gx.Persons)
                {
                    if (person.DisplayExtension != null && person.DisplayExtension.DescendancyNumber != null)
                    {
                        String number = person.DisplayExtension.DescendancyNumber;
                        bool spouse = number.EndsWith("-S") || number.EndsWith("-s");
                        if (spouse)
                        {
                            number = number.Substring(0, number.Length - 2);
                        }
                        int[] coordinates = ParseCoordinates(number);
                        List<DescendancyNode> current = rootArray;
                        int i = 0;
                        DescendancyNode node = null;
                        while (current != null)
                        {
                            int coordinate = coordinates[i];
                            while (current.Count < coordinate)
                            {
                                current.Add(null);
                            }

                            node = current[coordinate - 1];
                            if (node == null)
                            {
                                node = new DescendancyNode();
                                current[coordinate - 1] = node;
                            }

                            if (++i < coordinates.Length)
                            {
                                //if we still have another generation to descend, make sure the list is initialized.
                                List<DescendancyNode> children = node.Children;
                                if (children == null)
                                {
                                    children = new List<DescendancyNode>();
                                    node.Children = children;
                                }
                                current = children;
                            }
                            else
                            {
                                current = null;
                            }
                        }

                        if (spouse)
                        {
                            node.Spouse = person;
                        }
                        else
                        {
                            node.Person = person;
                        }
                    }
                }

                if (rootArray.Count > 0)
                {
                    root = rootArray[0];
                }
            }
            return root;
        }

        /// <summary>
        /// Gets the root person of the descendancy tree.
        /// </summary>
        /// <value>
        /// The root person of the descendancy tree.
        /// </value>
        public DescendancyNode Root
        {
            get
            {
                return root;
            }
        }

        /// <summary>
        /// Parses the coordinates of the specified d'Aboville number. See remarks.
        /// </summary>
        /// <param name="number">The d'Aboville number number. See remarks.</param>
        /// <returns></returns>
        /// <remarks>
        /// More information on a d'Aboville number can be found here: http://en.wikipedia.org/wiki/Genealogical_numbering_system#d.27Aboville_System.
        /// </remarks>
        protected int[] ParseCoordinates(String number)
        {
            List<StringBuilder> coords = new List<StringBuilder>();
            StringBuilder current = new StringBuilder();
            for (int i = 0; i < number.Length; i++)
            {
                char ch = number[i];
                if (ch == '.')
                {
                    coords.Add(current);
                    current = new StringBuilder();
                }
                else
                {
                    current.Append(ch);
                }
            }
            coords.Add(current);

            int[] coordinates = new int[coords.Count];
            for (int i = 0; i < coords.Count; i++)
            {
                StringBuilder num = coords[i];
                coordinates[i] = int.Parse(num.ToString());
            }
            return coordinates;
        }

        /// <summary>
        /// Represents a person, spouse, and descendancy in a tree.
        /// </summary>
        public class DescendancyNode
        {
            /// <summary>
            /// Gets or sets the main person of a tree.
            /// </summary>
            /// <value>
            /// The main person of a tree.
            /// </value>
            public Person Person
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the spouse of the main person.
            /// </summary>
            /// <value>
            /// The spouse of the main person.
            /// </value>
            public Person Spouse
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the children of the main person
            /// </summary>
            /// <value>
            /// The children of the main person
            /// </value>
            public List<DescendancyNode> Children
            {
                get;
                set;
            }
        }
    }
}
