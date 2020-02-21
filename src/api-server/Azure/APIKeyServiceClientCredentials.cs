using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Rest;

namespace TwentyFiveHours.API.Azure
{
    internal class APIKeyServiceClientCredentials : ServiceClientCredentials
    {
        private readonly string _key;

        public APIKeyServiceClientCredentials(string key)
        {
            this._key = key;
        }

        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException("Request was null.");
            request.Headers.Add("Ocp-Apim-Subscription-Key", this._key);

            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }
}
