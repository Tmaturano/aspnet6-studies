﻿using Blog.Models;
using System.Security.Claims;

namespace Blog.Extensions
{
    public static class RoleClaimsExtension
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var result = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email), //User.Identity.Name
                //new Claim(ClaimTypes.Role, "admin"), //User.IsInRole               
            };

            result.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Slug)));

            return result;
        }
    }
}
