using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using TennisClub.Api.Enums;
using TennisClub.Api.Extensions;
using TennisClub.Api.Models.Identity;
using TennisClub.Api.Models.Mongo;

namespace TennisClub.Api.Services
{
    public class IdentityService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<MongoRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public IdentityService(UserManager<ApplicationUser> userManager,
            RoleManager<MongoRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        public async Task<TokenOutputModel> LoginUserAsync(LoginInputModel input)
        {
            var user = await this.userManager.FindByEmailAsync(input.Email);

            if (user == null)
            {
                throw new Exception("User does not exists");
            }

            var result = await this.signInManager.PasswordSignInAsync(user.UserName, input.Password, false, false);

            if (result.Succeeded)
            {
                var roles = await userManager.GetRolesAsync(user);

                var key = configuration["Authorization:SecretKey"];

                return JwtAuthExtension.GenerateToken(user, roles, key, 7);
            }

            throw new Exception("Login error");
        }

        public async Task<ApplicationUser> RegisterUserAsync(RegisterInputModel input)
        {
            var user = new ApplicationUser(input.UserName)
            {
                Email = input.Email,
                FirstName = input.FirstName,
                LastName = input.LastName
            };

            var result = await userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(" ", result.Errors.Select(x => x.Description)));
            };

            return user;
        }

        public async Task RegisterUserToRoleAsync(ApplicationUser user, Role role)
        {
            if (!(await roleManager.RoleExistsAsync(Role.Admin.ToString())))
                await roleManager.CreateAsync(new MongoRole(Role.Admin.ToString()));
            if (!(await roleManager.RoleExistsAsync(Role.User.ToString())))
                await roleManager.CreateAsync(new MongoRole(Role.User.ToString()));

            if (await roleManager.RoleExistsAsync(role.ToString()))
            {
                await userManager.AddToRoleAsync(user, role.ToString());
            }
            else
            {
                throw new Exception("Register to role error");
            }
        }
    }
}
