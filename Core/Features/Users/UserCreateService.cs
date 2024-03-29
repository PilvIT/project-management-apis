﻿using Core.Features.GitHub.ViewModels;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Features.Users;

public class UserCreateService
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<AppUser> _userManager;

    public UserCreateService(AppDbContext dbContext, UserManager<AppUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<AppUser> CreateAsync(GitHubUserDetail ghUser)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();

        var user = new AppUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = ghUser.Email,
            EmailConfirmed = true
        };

        IdentityResult result = await _userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            throw new ArgumentException("User already exists, try login instead", nameof(ghUser));
        }

        var profile = new Profile
        {
            AppUserId = user.Id,
            DisplayName = ghUser.Name,
            GitHubId = ghUser.Id,
            GitHubUrl = ghUser.Url
        };
        user.Profile = profile;
        _dbContext.Profiles.Add(profile);

        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return user;
    }
}
