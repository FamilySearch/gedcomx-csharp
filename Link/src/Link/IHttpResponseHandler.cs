using System.Net.Http;
using System.Threading.Tasks;

namespace Tavis
{
    /// <summary>
    /// This interface is used to provide link objects with behaviour to be performed on the response
    /// from following a link.
    /// </summary>
    public interface IHttpResponseHandler
    {
        Task<HttpResponseMessage> HandleAsync(Link link, HttpResponseMessage responseMessage);
    }


  
}