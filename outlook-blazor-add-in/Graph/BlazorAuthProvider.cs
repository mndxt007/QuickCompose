// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace QuickCompose.Graph
{
    public class BlazorAuthProvider : IAuthenticationProvider
    {
        private readonly IAccessTokenProviderAccessor accessor;

        public BlazorAuthProvider(IAccessTokenProviderAccessor accessor)
        {
            this.accessor = accessor;
        }

        // Function called every time the GraphServiceClient makes a call
        public async Task AuthenticateRequestAsync(
            RequestInformation request,
            Dictionary<string, object>? additionalAuthenticationContext = null,
            CancellationToken cancellationToken = default)
        {
            // Request the token from the accessor
            var result = await accessor.TokenProvider.RequestAccessToken();

            if (result.TryGetToken(out var token))
            {
                // Add the token to the Authorization header
                request.Headers.Add("Authorization", $"Bearer {token.Value}");
            }
        }
    }
}
