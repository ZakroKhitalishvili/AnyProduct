// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace AnyProduct.Identity.Api
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("Products.Admin","Products Administration"),
                new ApiScope("Products.Default","Products default access")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                 new Client
        {
            ClientId = "AnyProduct.ClientApp",

            // no interactive user, use the clientid/secret for authentication
            AllowedGrantTypes = GrantTypes.ClientCredentials,

            // secret for authentication
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },

            // scopes that client has access to
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                "Products.Default",
            },

        }};


        public static List<TestUser> TestUsers =>
    new List<TestUser>
    {
        new TestUser
        {
            SubjectId = "5555",
            Username = "zack",
            Password = "paroli",
            Claims =
            {
                new Claim(JwtClaimTypes.Name, "Zack Raw"),
                new Claim(JwtClaimTypes.GivenName, "Zack"),
                new Claim(JwtClaimTypes.FamilyName, "Raw"),
            }
        }
    };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("Resource.Products")
                {
                    Scopes = new List<string>{ "Products.Admin", "Products.Default" },
                    ApiSecrets = new List<Secret>{ new Secret("secret".Sha256()) }
                }
            };
    }
}