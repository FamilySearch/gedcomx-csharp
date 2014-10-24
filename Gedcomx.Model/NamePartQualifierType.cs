using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Types
{
    public enum NamePartQualifierType
    {

        /**
         * A designation for honorifics (e.g. Dr., Rev., His Majesty, Haji),
         * ranks (e.g. Colonel, General, Knight, Esquire),
         * positions (e.g. Count, Chief, Father, King) or other titles (e.g., PhD, MD)
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Title")]
        Title,

        /**
         * A designation for the name of most prominent in importance among the names of that type (e.g., the primary given name).
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Primary")]
        Primary,

        /**
         * A designation for a name that is not primary in its importance among the names of that type (e.g., a secondary given name).
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Secondary")]
        Secondary,

        /**
         * A designation useful for cultures that designate a middle name that is distinct from a given name and a surname.
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Middle")]
        Middle,

        /**
         * A designation for one's familiar name.
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Familiar")]
        Familiar,

        /**
         * A designation for a name given for religious purposes.
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Religious")]
        Religious,

        /**
         * A name that associates a person with a group, such as a clan, tribe, or patriarchal hierarchy.
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Family")]
        Family,

        /**
         * A designation given by women to their original surname after they adopt a new surname upon marriage.
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Maiden")]
        Maiden,

        /**
         * A name derived from a father or paternal ancestor.
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Patronymic")]
        Patronymic,

        /**
         * A name derived from a mother or maternal ancestor.
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Matronymic")]
        Matronymic,

        /**
         * A name derived from associated geography.
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Geographic")]
        Geographic,

        /**
         * A name derived from one's occupation.
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Occupational")]
        Occupational,

        /**
         * A name derived from a characteristic.
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Characteristic")]
        Characteristic,

        /**
         * A name mandedated by law populations from Congo Free State / Belgian Congo / Congo / Democratic Republic of Congo (formerly Zaire).
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Postnom")]
        Postnom,

        /**
         * A grammatical designation for articles (a, the, dem, las, el, etc.),
         * prepositions (of, from, aus, zu, op, etc.), initials (e.g. PhD, MD),
         * annotations (e.g. twin, wife of, infant, unknown),
         * comparators (e.g. Junior, Senior, younger, little), ordinals (e.g. III, eighth),
         * and conjunctions (e.g. and, or, nee, ou, y, o, ne, &amp;).
         */
        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Particle")]
        Particle,

        [System.Xml.Serialization.XmlEnum("http://gedcomx.org/OTHER")]
        OTHER
    }
}
