using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Tavis.IANA;

namespace Tavis
{
    public class HintFactory
    {
        private readonly Dictionary<string, HintRegistration> _HintRegistry = new Dictionary<string, HintRegistration>(StringComparer.OrdinalIgnoreCase);

        public HintFactory()
        {
            // Register all official IANA link types
            AddHintType<AllowHint>();
            AddHintType<FormatsHint>();
            AddHintType<LinksHint>();
            AddHintType<AcceptPostHint>();
            AddHintType<AcceptPatchHint>();
            AddHintType<AcceptRanges>();
            AddHintType<AcceptPreferHint>();
            AddHintType<PreconditionReqHint>();
            AddHintType<AuthSchemesHint>();
            AddHintType<StatusHint>();

        }

        public void AddHintType<T>() where T : Hint, new()
        {
            var t = new T();
            _HintRegistry.Add(t.Name, new HintRegistration() {HintType =typeof(T) } ); 
        }

        public void SetHandler<T>(Func<Hint,HttpRequestMessage, HttpRequestMessage> handler) where T : Hint, new()
        {
            var t = new T();
            var reg = _HintRegistry[t.Name];
            reg.RequestHandler = handler;
        }

        public Hint CreateHint(string name)
        {
            var reg = _HintRegistry[name];
            var t = Activator.CreateInstance(reg.HintType) as Hint;
            SetupHandlers(reg, t);
            return t;

        }
        
        public T CreateHint<T>() where T : Hint, new()
        {
            var t = new T();
            var reg = _HintRegistry[t.Name];
            SetupHandlers(reg, t);
            return t;

        }

        private static void SetupHandlers(HintRegistration reg, Hint t)
        {
            t.ConfigureRequest = reg.RequestHandler;
        }

    }

    internal class HintRegistration
    {
        public Type HintType { get; set; }
        public Func<Hint, HttpRequestMessage, HttpRequestMessage> RequestHandler { get; set; }

        
    }
}
