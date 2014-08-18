// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Fs.Artifacts {

  /// <remarks>
  ///  Enumeration of known artifact types.
  /// </remarks>
  /// <summary>
  ///  Enumeration of known artifact types.
  /// </summary>
  public enum ArtifactType {

    /// <summary>
    ///  Unspecified enum value.
    /// </summary>
    [System.Xml.Serialization.XmlEnumAttribute(Name="__NULL__")]
    [System.Xml.Serialization.SoapEnumAttribute(Name="__NULL__")]
    NULL,

    /// <summary>
    ///   The artifact is an audio.
    /// </summary>
    Audio,

    /// <summary>
    ///   The artifact is an image of a document.
    /// </summary>
    Document,

    /// <summary>
    ///   The artifact is an image.
    /// </summary>
    Image,

    /// <summary>
    ///   The artifact is a portrait.
    /// </summary>
    Portrait,

    /// <summary>
    ///   The artifact is a story.
    /// </summary>
    Story,

    /// <summary>
    ///   The artifact is a video.
    /// </summary>
    Video,

    /// <summary>
    ///   Custom
    /// </summary>
    OTHER
  }

  /// <remarks>
  /// Utility class for converting to/from the QNames associated with ArtifactType.
  /// </remarks>
  /// <summary>
  /// Utility class for converting to/from the QNames associated with ArtifactType.
  /// </summary>
  public static class ArtifactTypeQNameUtil {

    /// <summary>
    /// Get the known ArtifactType for a given QName. If the QName isn't a known QName, ArtifactType.OTHER will be returned.
    /// </summary>
    public static ArtifactType ConvertFromKnownQName(string qname) {
      if (qname != null) {
        if ("http://familysearch.org/v1/Audio".Equals(qname)) {
          return ArtifactType.Audio;
        }
        if ("http://familysearch.org/v1/Document".Equals(qname)) {
          return ArtifactType.Document;
        }
        if ("http://familysearch.org/v1/Image".Equals(qname)) {
          return ArtifactType.Image;
        }
        if ("http://familysearch.org/v1/Portrait".Equals(qname)) {
          return ArtifactType.Portrait;
        }
        if ("http://familysearch.org/v1/Story".Equals(qname)) {
          return ArtifactType.Story;
        }
        if ("http://familysearch.org/v1/Video".Equals(qname)) {
          return ArtifactType.Video;
        }
      }
      return ArtifactType.OTHER;
    }

    /// <summary>
    /// Convert the known ArtifactType to a QName. If ArtifactType.OTHER, an ArgumentException will be thrown.
    /// </summary>
    public static string ConvertToKnownQName(ArtifactType known) {
      switch (known) {
        case ArtifactType.Audio:
          return "http://familysearch.org/v1/Audio";
        case ArtifactType.Document:
          return "http://familysearch.org/v1/Document";
        case ArtifactType.Image:
          return "http://familysearch.org/v1/Image";
        case ArtifactType.Portrait:
          return "http://familysearch.org/v1/Portrait";
        case ArtifactType.Story:
          return "http://familysearch.org/v1/Story";
        case ArtifactType.Video:
          return "http://familysearch.org/v1/Video";
        default:
          throw new System.ArgumentException("No known QName for: " + known, "known");
      }
    }
  }
}