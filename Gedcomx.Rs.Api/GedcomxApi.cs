using System;

namespace Gx.Rs.Api
{
	/// <summary>
	/// The basic interface for a GEDCOM X API.
	/// </summary>
    public interface GedcomxApi
    {

    }

	public static class Rel
	{
		public const string OAuth2Authorization = @"http://oauth.net/core/2.0/endpoint/authorize";
		public const string OAuth2Token = @"http://oauth.net/core/2.0/endpoint/token";
		public const string Person = "person";
		public const string PersonTemplate = Rel.Person + "-template";
	}

	public static class TemplateVariables
	{
		public const string PersonId = "pid";
		public const string ConclusionId = "cid";
		public const string CoupleRelationshipId = "crid";
		public const string SourceReferenceId = "srid";
		public const string NoteId = "nid";
		public const string CollectionId = "clid";
		public const string SourceDescriptionId = "sdid";
		public const string ArtifactId = "aid";
		public const string PersonaReferenceId = "prid";
	}

	public static class MediaTypes
	{
		public const string GedcomxXml = "application/x-gedcomx-v1+xml";
		public const string AtomXml = "application/atom+xml";
	}
}

