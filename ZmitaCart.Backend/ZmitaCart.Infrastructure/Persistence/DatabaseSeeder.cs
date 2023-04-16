using Microsoft.AspNetCore.Identity;
using ZmitaCart.Application.Interfaces;
using ZmitaCart.Domain.Common;

namespace ZmitaCart.Infrastructure.Persistence;

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly ApplicationDbContext _dbContext;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public DatabaseSeeder(ApplicationDbContext dbContext, RoleManager<IdentityRole<int>> roleManager)
    {
        _dbContext = dbContext;
        _roleManager = roleManager;
    }

    public async Task Seed()
    {
        if (!await _dbContext.Database.CanConnectAsync())
        {
            return;
        }

        foreach (var supportedRole in Role.SupportedRoles)
        {
            var role = new IdentityRole<int>(supportedRole.Code);
            
            if (!_dbContext.Roles.Contains(role))
            {
                await _roleManager.CreateAsync(role);
            }
        }
    }
}