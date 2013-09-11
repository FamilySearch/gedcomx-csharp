// <auto-generated>
// 
//
// Generated by <a href="http://enunciate.codehaus.org">Enunciate</a>.
// </auto-generated>
using System;

namespace Gx.Types {

  /// <remarks>
  ///  Enumeration of standard event types.
  /// </remarks>
  /// <summary>
  ///  Enumeration of standard event types.
  /// </summary>
  public enum EventType {

    /// <summary>
    ///  Unspecified enum value.
    /// </summary>
    [System.Xml.Serialization.XmlEnumAttribute(Name="__NULL__")]
    [System.Xml.Serialization.SoapEnumAttribute(Name="__NULL__")]
    NULL,

    /// <summary>
    ///   An adoption event.
    /// </summary>
    Adoption,

    /// <summary>
    ///   An adult christening event.
    /// </summary>
    AdultChristening,

    /// <summary>
    ///   An annulment event of a marriage.
    /// </summary>
    Annulment,

    /// <summary>
    ///   A baptism event.
    /// </summary>
    Baptism,

    /// <summary>
    ///   A bar mitzvah event.
    /// </summary>
    BarMitzvah,

    /// <summary>
    ///   A bat mitzvah event.
    /// </summary>
    BatMitzvah,

    /// <summary>
    ///   A birth event.
    /// </summary>
    Birth,

    /// <summary>
    ///   A an official blessing event, such as at the hands of a clergy member or at another religious rite.
    /// </summary>
    Blessing,

    /// <summary>
    ///   A burial event.
    /// </summary>
    Burial,

    /// <summary>
    ///   A census event.
    /// </summary>
    Census,

    /// <summary>
    ///   A christening event *at birth*. Note: use `AdultChristening` for a christening event as an adult.
    /// </summary>
    Christening,

    /// <summary>
    ///   A circumcision event.
    /// </summary>
    Circumcision,

    /// <summary>
    ///   A confirmation event (or other rite of initiation) in a church or religion.
    /// </summary>
    Confirmation,

    /// <summary>
    ///   A cremation event after death.
    /// </summary>
    Cremation,

    /// <summary>
    ///   A death event.
    /// </summary>
    Death,

    /// <summary>
    ///   A divorce event.
    /// </summary>
    Divorce,

    /// <summary>
    ///   A divorce filing event.
    /// </summary>
    DivorceFiling,

    /// <summary>
    ///   A education or an educational achievement event (e.g. diploma, graduation, scholarship, etc.).
    /// </summary>
    Education,

    /// <summary>
    ///   An engagement to be married event.
    /// </summary>
    Engagement,

    /// <summary>
    ///   An emigration event.
    /// </summary>
    Emigration,

    /// <summary>
    ///   An excommunication event from a church.
    /// </summary>
    Excommunication,

    /// <summary>
    ///   A first communion event.
    /// </summary>
    FirstCommunion,

    /// <summary>
    ///   A funeral event.
    /// </summary>
    Funeral,

    /// <summary>
    ///   An immigration event.
    /// </summary>
    Immigration,

    /// <summary>
    ///   A land transaction event.
    /// </summary>
    LandTransaction,

    /// <summary>
    ///   A marriage event.
    /// </summary>
    Marriage,

    /// <summary>
    ///   A military award event.
    /// </summary>
    MilitaryAward,

    /// <summary>
    ///   A military discharge event.
    /// </summary>
    MilitaryDischarge,

    /// <summary>
    ///   A mission event.
    /// </summary>
    Mission,

    /// <summary>
    ///   An event of a move (i.e. change of residence) from a location.
    /// </summary>
    MoveFrom,

    /// <summary>
    ///   An event of a move (i.e. change of residence) to a location.
    /// </summary>
    MoveTo,

    /// <summary>
    ///   A naturalization event (i.e. acquisition of citizenship and nationality).
    /// </summary>
    Naturalization,

    /// <summary>
    ///   An ordination event.
    /// </summary>
    Ordination,

    /// <summary>
    ///   A retirement event.
    /// </summary>
    Retirement,

    /// <summary>
    ///  (no documentation provided)
    /// </summary>
    OTHER
  }

  /// <remarks>
  /// Utility class for converting to/from the QNames associated with EventType.
  /// </remarks>
  /// <summary>
  /// Utility class for converting to/from the QNames associated with EventType.
  /// </summary>
  public static class EventTypeQNameUtil {

    /// <summary>
    /// Get the known EventType for a given QName. If the QName isn't a known QName, EventType.OTHER will be returned.
    /// </summary>
    public static EventType ConvertFromKnownQName(string qname) {
      if (qname != null) {
        if ("http://gedcomx.org/Adoption".Equals(qname)) {
          return EventType.Adoption;
        }
        if ("http://gedcomx.org/AdultChristening".Equals(qname)) {
          return EventType.AdultChristening;
        }
        if ("http://gedcomx.org/Annulment".Equals(qname)) {
          return EventType.Annulment;
        }
        if ("http://gedcomx.org/Baptism".Equals(qname)) {
          return EventType.Baptism;
        }
        if ("http://gedcomx.org/BarMitzvah".Equals(qname)) {
          return EventType.BarMitzvah;
        }
        if ("http://gedcomx.org/BatMitzvah".Equals(qname)) {
          return EventType.BatMitzvah;
        }
        if ("http://gedcomx.org/Birth".Equals(qname)) {
          return EventType.Birth;
        }
        if ("http://gedcomx.org/Blessing".Equals(qname)) {
          return EventType.Blessing;
        }
        if ("http://gedcomx.org/Burial".Equals(qname)) {
          return EventType.Burial;
        }
        if ("http://gedcomx.org/Census".Equals(qname)) {
          return EventType.Census;
        }
        if ("http://gedcomx.org/Christening".Equals(qname)) {
          return EventType.Christening;
        }
        if ("http://gedcomx.org/Circumcision".Equals(qname)) {
          return EventType.Circumcision;
        }
        if ("http://gedcomx.org/Confirmation".Equals(qname)) {
          return EventType.Confirmation;
        }
        if ("http://gedcomx.org/Cremation".Equals(qname)) {
          return EventType.Cremation;
        }
        if ("http://gedcomx.org/Death".Equals(qname)) {
          return EventType.Death;
        }
        if ("http://gedcomx.org/Divorce".Equals(qname)) {
          return EventType.Divorce;
        }
        if ("http://gedcomx.org/DivorceFiling".Equals(qname)) {
          return EventType.DivorceFiling;
        }
        if ("http://gedcomx.org/Education".Equals(qname)) {
          return EventType.Education;
        }
        if ("http://gedcomx.org/Engagement".Equals(qname)) {
          return EventType.Engagement;
        }
        if ("http://gedcomx.org/Emigration".Equals(qname)) {
          return EventType.Emigration;
        }
        if ("http://gedcomx.org/Excommunication".Equals(qname)) {
          return EventType.Excommunication;
        }
        if ("http://gedcomx.org/FirstCommunion".Equals(qname)) {
          return EventType.FirstCommunion;
        }
        if ("http://gedcomx.org/Funeral".Equals(qname)) {
          return EventType.Funeral;
        }
        if ("http://gedcomx.org/Immigration".Equals(qname)) {
          return EventType.Immigration;
        }
        if ("http://gedcomx.org/LandTransaction".Equals(qname)) {
          return EventType.LandTransaction;
        }
        if ("http://gedcomx.org/Marriage".Equals(qname)) {
          return EventType.Marriage;
        }
        if ("http://gedcomx.org/MilitaryAward".Equals(qname)) {
          return EventType.MilitaryAward;
        }
        if ("http://gedcomx.org/MilitaryDischarge".Equals(qname)) {
          return EventType.MilitaryDischarge;
        }
        if ("http://gedcomx.org/Mission".Equals(qname)) {
          return EventType.Mission;
        }
        if ("http://gedcomx.org/MoveFrom".Equals(qname)) {
          return EventType.MoveFrom;
        }
        if ("http://gedcomx.org/MoveTo".Equals(qname)) {
          return EventType.MoveTo;
        }
        if ("http://gedcomx.org/Naturalization".Equals(qname)) {
          return EventType.Naturalization;
        }
        if ("http://gedcomx.org/Ordination".Equals(qname)) {
          return EventType.Ordination;
        }
        if ("http://gedcomx.org/Retirement".Equals(qname)) {
          return EventType.Retirement;
        }
      }
      return EventType.OTHER;
    }

    /// <summary>
    /// Convert the known EventType to a QName. If EventType.OTHER, an ArgumentException will be thrown.
    /// </summary>
    public static string ConvertToKnownQName(EventType known) {
      switch (known) {
        case EventType.Adoption:
          return "http://gedcomx.org/Adoption";
        case EventType.AdultChristening:
          return "http://gedcomx.org/AdultChristening";
        case EventType.Annulment:
          return "http://gedcomx.org/Annulment";
        case EventType.Baptism:
          return "http://gedcomx.org/Baptism";
        case EventType.BarMitzvah:
          return "http://gedcomx.org/BarMitzvah";
        case EventType.BatMitzvah:
          return "http://gedcomx.org/BatMitzvah";
        case EventType.Birth:
          return "http://gedcomx.org/Birth";
        case EventType.Blessing:
          return "http://gedcomx.org/Blessing";
        case EventType.Burial:
          return "http://gedcomx.org/Burial";
        case EventType.Census:
          return "http://gedcomx.org/Census";
        case EventType.Christening:
          return "http://gedcomx.org/Christening";
        case EventType.Circumcision:
          return "http://gedcomx.org/Circumcision";
        case EventType.Confirmation:
          return "http://gedcomx.org/Confirmation";
        case EventType.Cremation:
          return "http://gedcomx.org/Cremation";
        case EventType.Death:
          return "http://gedcomx.org/Death";
        case EventType.Divorce:
          return "http://gedcomx.org/Divorce";
        case EventType.DivorceFiling:
          return "http://gedcomx.org/DivorceFiling";
        case EventType.Education:
          return "http://gedcomx.org/Education";
        case EventType.Engagement:
          return "http://gedcomx.org/Engagement";
        case EventType.Emigration:
          return "http://gedcomx.org/Emigration";
        case EventType.Excommunication:
          return "http://gedcomx.org/Excommunication";
        case EventType.FirstCommunion:
          return "http://gedcomx.org/FirstCommunion";
        case EventType.Funeral:
          return "http://gedcomx.org/Funeral";
        case EventType.Immigration:
          return "http://gedcomx.org/Immigration";
        case EventType.LandTransaction:
          return "http://gedcomx.org/LandTransaction";
        case EventType.Marriage:
          return "http://gedcomx.org/Marriage";
        case EventType.MilitaryAward:
          return "http://gedcomx.org/MilitaryAward";
        case EventType.MilitaryDischarge:
          return "http://gedcomx.org/MilitaryDischarge";
        case EventType.Mission:
          return "http://gedcomx.org/Mission";
        case EventType.MoveFrom:
          return "http://gedcomx.org/MoveFrom";
        case EventType.MoveTo:
          return "http://gedcomx.org/MoveTo";
        case EventType.Naturalization:
          return "http://gedcomx.org/Naturalization";
        case EventType.Ordination:
          return "http://gedcomx.org/Ordination";
        case EventType.Retirement:
          return "http://gedcomx.org/Retirement";
        default:
          throw new System.ArgumentException("No known QName for: " + known, "known");
      }
    }
  }
}