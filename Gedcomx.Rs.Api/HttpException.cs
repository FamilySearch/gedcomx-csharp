using System;
using System.Net;

namespace Gx.Rs.Api
{
	public class HttpException : System.Exception
	{
		private readonly HttpStatusCode statusCode;

		public HttpException (HttpStatusCode statusCode)
		{
			this.statusCode = statusCode;
		}

		public HttpStatusCode StatusCode {
			get {
				return this.statusCode;
			}
		}
	}
}

