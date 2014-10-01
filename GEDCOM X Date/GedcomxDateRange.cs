using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gedcomx.Date
{
    class GedcomxDateRange : GedcomxDate
    {
        private string date;

        public GedcomxDateRange(string date)
        {
            // TODO: Complete member initialization
            this.date = date;
        }

        public override GedcomxDateType Type
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsApproximate()
        {
            throw new NotImplementedException();
        }

        public override string ToFormalString()
        {
            throw new NotImplementedException();
        }
    }
}
