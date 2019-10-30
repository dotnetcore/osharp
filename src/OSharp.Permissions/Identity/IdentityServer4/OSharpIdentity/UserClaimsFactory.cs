// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace OSharp.Identity.IdentityServer4.OSharpIdentity
{
    internal class UserClaimsFactory<TUser> : IUserClaimsPrincipalFactory<TUser>
        where TUser : class
    {
        private readonly Decorator<IUserClaimsPrincipalFactory<TUser>> _inner;
        private UserManager<TUser> _userManager;

        public UserClaimsFactory(Decorator<IUserClaimsPrincipalFactory<TUser>> inner, UserManager<TUser> userManager)
        {
            _inner = inner;
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> CreateAsync(TUser user)
        {
            var principal = await _inner.Instance.CreateAsync(user);
            var identity = principal.Identities.First();

            var id = await _userManager.GetUserIdAsync(user);
            if (!identity.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
            {
                identity.AddClaim(new Claim(JwtClaimTypes.Subject, id));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, id));
                identity.AddClaim(new Claim(JwtClaimTypes.Id, id));
            }
            

            var username = await _userManager.GetUserNameAsync(user);
            var usernameClaim = identity.FindFirst(claim => claim.Type == _userManager.Options.ClaimsIdentity.UserNameClaimType && claim.Value == username);
            if (usernameClaim != null)
            {
                identity.RemoveClaim(usernameClaim);
                identity.AddClaim(new Claim(JwtClaimTypes.PreferredUserName, username));
                identity.AddClaim(new Claim(ClaimTypes.Name, username));
            }

            if (!identity.HasClaim(x => x.Type == JwtClaimTypes.Name))
            {
                identity.AddClaim(new Claim(JwtClaimTypes.Name, username));
            }

            if (_userManager.SupportsUserEmail)
            {
                var email = await _userManager.GetEmailAsync(user);
                if (!String.IsNullOrWhiteSpace(email))
                {
                    identity.AddClaims(new[]
                    {
                        new Claim(JwtClaimTypes.Email, email),
                        new Claim(JwtClaimTypes.EmailVerified,
                            await _userManager.IsEmailConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean)
                    });
                }
            }

            if (_userManager.SupportsUserPhoneNumber)
            {
                var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
                if (!String.IsNullOrWhiteSpace(phoneNumber))
                {
                    identity.AddClaims(new[]
                    {
                        new Claim(JwtClaimTypes.PhoneNumber, phoneNumber),
                        new Claim(JwtClaimTypes.PhoneNumberVerified,
                            await _userManager.IsPhoneNumberConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean)
                    });
                }
            }

            return principal;
        }
    }
}
