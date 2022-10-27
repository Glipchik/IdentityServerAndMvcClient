using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("test.api", new []
                    { JwtClaimTypes.Name})
                {
                    Scopes = {"test.api"}
                },
                new ApiResource("test.web", new []
                    { JwtClaimTypes.Name})
                {
                    Scopes ={"test.web"}
                }
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("test.api"),
                new ApiScope("test.web")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client-web-app",
                    ClientName = "Client",
                    ClientSecrets = {new Secret("client-secrets-test".ToSha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                         JwtClaimTypes.Role,
                        "test.api"
                    },
                    AlwaysIncludeUserClaimsInIdToken = true
                },
                new Client
                {
                    ClientId = "web_client",
                    ClientName = "Web Client",
                    ClientSecrets = { new Secret("client_secret_mvc".ToSha256()) },
                    AllowedGrantTypes =  GrantTypes.Code,
                    RedirectUris = { "https://localhost:7138/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:7138/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:7138/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
            };
    }
}