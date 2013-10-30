using System;
using Gx;
using RestSharp;

namespace Gx.Rs.Api
{
	public class GedcomxApiResponse<R> : RestResponse<Gedcomx>
	{

		public R Resource { get; set; }

		public static GedcomxApiResponse<R> Wrap(IRestResponse<Gedcomx> response, R resource)
		{
			GedcomxApiResponse<R> result = new GedcomxApiResponse<R>();
			result.ContentEncoding = response.ContentEncoding;
			result.ContentLength = response.ContentLength;
			result.ContentType = response.ContentType;
			result.Cookies = response.Cookies;
			result.ErrorMessage = response.ErrorMessage;
			result.ErrorException = response.ErrorException;
			result.Headers = response.Headers;
			result.RawBytes = response.RawBytes;
			result.ResponseStatus = response.ResponseStatus;
			result.ResponseUri = response.ResponseUri;
			result.Server = response.Server;
			result.StatusCode = response.StatusCode;
			result.StatusDescription = response.StatusDescription;
			result.Request = response.Request;
			result.Data = response.Data;
			result.Resource = resource;
			return result;
		}
	}

}

