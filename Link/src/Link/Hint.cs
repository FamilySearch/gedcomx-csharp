using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Tavis
{
    public class Hint
    {
        public Func<Hint, HttpRequestMessage, HttpRequestMessage> ConfigureRequest { get; set; }


        public string Name { get; set; }
        public JToken Content { get; set; }  // Json document
    }
}