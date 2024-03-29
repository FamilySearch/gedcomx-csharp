// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Types {

  /// <remarks>
  ///  Enumeration of known gender types.
  /// </remarks>
  /// <summary>
  ///  Enumeration of known gender types.
  /// </summary>
  public enum GenderType {

    /// <summary>
    ///  Unspecified enum value.
    /// </summary>
    [System.Xml.Serialization.XmlEnumAttribute(Name="__NULL__")]
    NULL,

    /// <summary>
    ///   Male.
    /// </summary>
    [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Male")]
    Male,

    /// <summary>
    ///   Female.
    /// </summary>
    [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Female")]
    Female,

    /// <summary>
    ///   Unknown. Note that this should be used strictly as "unknown" and not to
    ///   indicate a type that is not set or not understood.
    /// </summary>
    [System.Xml.Serialization.XmlEnum("http://gedcomx.org/Unknown")]
    Unknown,

    /// <summary>
    ///   Custom
    /// </summary>
    [System.Xml.Serialization.XmlEnum("http://gedcomx.org/OTHER")]
    OTHER
  }
}
