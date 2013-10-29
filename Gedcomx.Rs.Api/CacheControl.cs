using System;
using System.Collections.Generic;

namespace Gx.Rs.Api
{
	public class CacheControl
	{
		private readonly Dictionary<string, string> cacheExtension;
		private readonly int maxAge;
		private readonly int sMaxAge;
		private readonly List<string> noCacheFields;
		private readonly List<string> privateFields;
		private readonly bool noStore;
		private readonly bool noTransform;
		private readonly bool mustRevalidate;
		private readonly bool proxyRevalidate;

		public CacheControl (Dictionary<string, string> cacheExtension, int maxAge, int sMaxAge, List<string> noCacheFields, List<string> privateFields, bool noStore, bool noTransform, bool mustRevalidate, bool proxyRevalidate)
		{
			this.cacheExtension = cacheExtension;
			this.maxAge = maxAge;
			this.sMaxAge = sMaxAge;
			this.noCacheFields = noCacheFields;
			this.privateFields = privateFields;
			this.noStore = noStore;
			this.noTransform = noTransform;
			this.mustRevalidate = mustRevalidate;
			this.proxyRevalidate = proxyRevalidate;
		}

		public static CacheControl Parse(string header)
		{
			Dictionary<string, string> cacheExtension = new Dictionary<string, string>();
			int maxAge = -1;
			int sMaxAge = -1;
			List<string> noCacheFields = new List<string>();
			List<string> privateFields = new List<string>();
			bool noStore = false;
			bool noTransform = false;
			bool mustRevalidate = false;
			bool proxyRevalidate = false;

			string[] directives = header.Split(',');
			foreach (string directive in directives) {
				string[] directiveParts = directive.Split('=');
				string directiveName = directiveParts[0].Trim().ToLowerInvariant();
				if ("max-age".Equals(directiveName)) {
					if (directiveParts.Length > 1) {
						maxAge = Convert.ToInt32(directiveParts[1]);
					}
				}
				else if ("s-maxage".Equals(directiveName)) {
					if (directiveParts.Length > 1) {
						sMaxAge = Convert.ToInt32 (directiveParts[1]);
					}
				}
				else if ("private".Equals(directiveName)) {
					//todo: 'private' list. (comma-separated, so the tokenizer mechanism needs to be rewritten).
				}
				else if ("no-cache".Equals(directiveName)) {
					//todo: 'no-cache' list. (comma-separated, so the tokenizer mechanism needs to be rewritten).
				}
				else if ("no-store".Equals(directiveName)) {
					noStore = true;
				}
				else if ("no-transform".Equals(directiveName)) {
					noTransform = true;
				}
				else if ("must-revalidate".Equals(directiveName)) {
					mustRevalidate = true;
				}
				else if ("proxy-revalidate".Equals(directiveName)) {
					proxyRevalidate = true;
				}
				else if (!"public".Equals(directiveName)) {
					//todo: cache extensions
				}
			}

			return new CacheControl(cacheExtension, 
			                        maxAge,
			                        sMaxAge,
			                        noCacheFields, 
			                        privateFields, 
			                        noStore, 
			                        noTransform, 
			                        mustRevalidate, 
			                        proxyRevalidate);
		}

		public Dictionary<string, string> CacheExtension {
			get {
				return this.cacheExtension;
			}
		}

		public int MaxAge {
			get {
				return this.maxAge;
			}
		}

		public int SMaxAge {
			get {
				return this.sMaxAge;
			}
		}

		public List<string> NoCacheFields {
			get {
				return this.noCacheFields;
			}
		}

		public List<string> PrivateFields {
			get {
				return this.privateFields;
			}
		}

		public bool NoStore {
			get {
				return this.noStore;
			}
		}

		public bool NoTransform {
			get {
				return this.noTransform;
			}
		}

		public bool MustRevalidate {
			get {
				return this.mustRevalidate;
			}
		}

		public bool ProxyRevalidate {
			get {
				return this.proxyRevalidate;
			}
		}
		
	}
}

