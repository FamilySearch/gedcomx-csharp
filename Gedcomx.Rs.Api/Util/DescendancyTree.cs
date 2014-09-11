using Gx.Conclusion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    public class DescendancyTree
    {
        private DescendancyNode root;

        public DescendancyTree(Gedcomx gx)
        {
            this.root = BuildTree(gx);
        }

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

        public DescendancyNode Root
        {
            get
            {
                return root;
            }
        }

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

        public class DescendancyNode
        {
            public Person Person
            {
                get;
                set;
            }

            public Person Spouse
            {
                get;
                set;
            }

            public List<DescendancyNode> Children
            {
                get;
                set;
            }
        }
    }
}
