using Gx.Conclusion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Rs.Api.Util
{
    public class AncestryTree
    {
        private readonly List<Person> ancestry;

        public AncestryTree(Gedcomx gx)
        {
            this.ancestry = BuildArray(gx);
        }

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

        public AncestryNode Root
        {
            get
            {
                return GetAncestor(1);
            }
        }

        public AncestryNode GetAncestor(int number)
        {
            return ancestry.Count < number ? null : new AncestryNode(this, number);
        }

        public class AncestryNode
        {
            private readonly int number;
            private readonly AncestryTree tree;

            public AncestryNode(AncestryTree tree, int number)
            {
                this.number = number;
                this.tree = tree;
            }

            public Person Person
            {
                get
                {
                    return tree.ancestry[this.number - 1];
                }
            }

            public AncestryNode Father
            {
                get
                {
                    return tree.GetAncestor(this.number * 2);
                }
            }

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
