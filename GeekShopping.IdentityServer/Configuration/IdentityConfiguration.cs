using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace GeekShopping.IdentityServer.Configuration;

public static class IdentityConfiguration
{
    public const string Admin = "Admin";
    public const string Client = "Client";

    public static IEnumerable<IdentityResource> IdentityResources
        =>
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile(),
        ];

    public static IEnumerable<ApiScope> ApiScopes
        =>
        [
            new ApiScope("geek_shopping", "Geek Shopping Server"),
            new ApiScope("read", "Read your data."),
            new ApiScope("write", "Write your data."),
            new ApiScope("delete", "Delete your data."),
        ];

    public static IEnumerable<Client> Clients
        =>
        [
            new Client
            {
                ClientId = "client_id_mvc",
                ClientSecrets = [ new Secret("client_secret_mvc".Sha256()) ],
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = [ "read", "write", "profile" ]
            },
            new Client
            {
                ClientId = "geek_shopping",
                ClientSecrets = [ new Secret("client_secret_mvc".Sha256()) ],
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = [ 
                    "https://localhost:4430/signin-oidc",
                    "https://localhost:4435/signin-oidc" 
                    ],
                PostLogoutRedirectUris = [ 
                    "https://localhost:4430/signout-callback-oidc" ,
                    "https://localhost:4435/signout-callback-oidc"
                    ],
                AllowedScopes = 
                [ 
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "geek_shopping"
                ]
            }
        ];
}
