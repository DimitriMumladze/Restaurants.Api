﻿using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Restaurants.Application.Users;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext 
    //this helps us to get current user
{
    public CurrentUser? GetCurrentUser() //might be null
    {
        var user = httpContextAccessor?.HttpContext?.User; //this might be null thats why here is two [?]
        //if user is not presenting
        if (user == null)
            throw new InvalidOperationException("User context is not present.");
        //if user is not properly authenticated
        if (user.Identity == null || user.Identity.IsAuthenticated)
            return null;

        //now true variation //getting values
        var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role)!.Select(c => c.Value);
        var nationality = user.FindFirst(c => c.Type == "Nationality")?.Value;
        var dateOfBirthString = user.FindFirst(c => c.Type == "DateOfBirth")?.Value;
        var dateOfBirth = dateOfBirthString == null
            ? (DateOnly?)null
            : DateOnly.ParseExact(dateOfBirthString, "yyyy-MM-dd");

        return new CurrentUser(userId, email, roles, nationality, dateOfBirth);
    }
}

public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}