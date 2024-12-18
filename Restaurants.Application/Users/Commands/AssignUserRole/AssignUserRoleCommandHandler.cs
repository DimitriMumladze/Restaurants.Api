﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler(ILogger<AssignUserRoleCommandHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) 
    : IRequestHandler<AssignUserRoleCommand>
{
    public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Assaiging user role: {@Request}", request);
        var user = await userManager.FindByEmailAsync(request.UserEMail) 
            // Built in method that automatically searchs user with Email
            ?? throw new NotFoundException(nameof(User), request.UserEMail);
        // Same but on the role 
        var role = await roleManager.FindByNameAsync(request.RoleName)
            ?? throw new NotFoundException(nameof(IdentityRole), request.RoleName);

        // Assaign giver role
        await userManager.AddToRoleAsync(user, role.Name!);
    }
}
