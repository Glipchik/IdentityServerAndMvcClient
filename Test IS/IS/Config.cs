// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IS
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            yield return new ApiResource("SwaggerAPI");
            yield return new ApiResource("OrdersAPI");
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            yield return new IdentityResources.OpenId();
            yield return new IdentityResources.Profile();
        }

        /// <summary>
        /// IdentityServer4 version 4.x.x changes
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            yield return new ApiScope("SwaggerAPI", "Swagger API");
            yield return new ApiScope("blazor", "Blazor WebAssembly");
            yield return new ApiScope("OrdersAPI", "Orders API");
        }

        public static IEnumerable<Client> GetClients() =>
        new List<Client>
        {
            new Client
            {
                ClientId = "add_mvc",
                ClientSecrets = { new Secret("add_mvc_secret".ToSha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                AllowedScopes =
                {
                    "OrdersAPI",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },

                RedirectUris = {"https://localhost:7272/signin-oidc"},
                PostLogoutRedirectUris = {"https://localhost:7272/signout-callback-oidc"},

                RequireConsent = false,

                AccessTokenLifetime = 5,

                AllowOfflineAccess = true

                // AlwaysIncludeUserClaimsInIdToken = true
            }
        };
    }
}