using System.Net.Http;
using System.Threading.Tasks;

namespace Tavis
{
    /// <summary>
    /// Interface used to implement in client state machines to enable aggregating client state when following a LE link.
    /// </summary>
    public interface IEmbedTarget
    {
        Task EmbedContent(Link link, HttpContent content);
    }
}