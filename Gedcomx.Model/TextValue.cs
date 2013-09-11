// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Common {

  /// <remarks>
  ///  An element representing a text value that may be in a specific language.
  /// </remarks>
  /// <summary>
  ///  An element representing a text value that may be in a specific language.
  /// </summary>
  [System.SerializableAttribute()]
  [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://gedcomx.org/v1/",TypeName="TextValue")]
  [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://gedcomx.org/v1/",TypeName="TextValue")]
  public partial class TextValue {

    private string _lang;
    private string _value;
    /// <summary>
    ///  The language of the text value.
    /// </summary>
    [System.Xml.Serialization.XmlAttributeAttribute(AttributeName="lang",Namespace="http://www.w3.org/XML/1998/namespace")]
    [System.Xml.Serialization.SoapAttributeAttribute(AttributeName="lang",Namespace="http://www.w3.org/XML/1998/namespace")]
    public string Lang {
      get {
        return this._lang;
      }
      set {
        this._lang = value;
      }
    }
    /// <summary>
    ///  The text value.
    /// </summary>
    [System.Xml.Serialization.XmlTextAttribute()]
    public string Value {
      get {
        return this._value;
      }
      set {
        this._value = value;
      }
    }
  }
}  