using System.Net.Http;
using System.Threading.Tasks;

namespace Tavis
{
    /// <summary>
    /// HttpResponseHandler that can be chained into a response pipeline
    /// </summary>
    public abstract class DelegatingResponseHandler : IHttpResponseHandler
    {
        public DelegatingResponseHandler InnerResponseHandler { get; set; }

        protected DelegatingResponseHandler()
        {
            
        }

        protected DelegatingResponseHandler(DelegatingResponseHandler innerResponseHandler)
        {
            InnerResponseHandler = innerResponseHandler;
        }

        public virtual Task<HttpResponseMessage> HandleAsync(Link link, HttpResponseMessage responseMessage)
        {
            if (InnerResponseHandler != null)
            {
                return InnerResponseHandler.HandleAsync(link, responseMessage);
            }

            var tcs = new TaskCompletionSource<HttpResponseMessage>();
            tcs.SetResult(responseMessage);
            return tcs.Task;
        }
    }


 
}