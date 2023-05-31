using FrontEnd.Data;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Services;

public class AdminService : IAdminService
{
    //private readonly IdentityDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    private bool _adminExists;

    //public AdminService(IdentityDbContext dbContext)
    public AdminService(IServiceProvider serviceProvider)
    {
        //_dbContext = dbContext;
        _serviceProvider = serviceProvider;
    }

    public async Task<bool> AllowAdminUserCreationAsync()
    {
        if (_adminExists)
        {
            return false;
        }
        else
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

                //if (await _dbContext.Users.AnyAsync(user => user.IsAdmin))
                if (await dbContext.Users.AnyAsync(user => user.IsAdmin))
                {
                    // There are already admin users
                    _adminExists = true;
                    return false;
                }

                return true;
            }
            
        }
    }
}
