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
            if (await this.userManager.FindByEmailAsync(input.Email) == null)
            {
                throw new Exception("Ne postoji korisnik");
            }

            var result = await this.signInManager.PasswordSignInAsync(input.Email, input.Password, false, false);

            if (result.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(input.Email);

                var roles = await userManager.GetRolesAsync(user);

                var key = configuration["Authorization:SecretKey"];

                return JwtAuthExtension.GenerateToken(user, roles, key, 7);
            }

            throw new Exception("Neuspesno logovanje");
        }

        public async Task<ApplicationUser> RegisterUserAsync(RegisterInputModel input)
        {

            //todo check password and confirmation password

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
            //todo ovo prebaci
            if (!(await roleManager.RoleExistsAsync(Role.Admin.ToString())))
                await roleManager.CreateAsync(new MongoRole(Role.Admin.ToString()));
            if (!(await roleManager.RoleExistsAsync(Role.User.ToString())))
                await roleManager.CreateAsync(new MongoRole(Role.User.ToString()));

            if (await roleManager.RoleExistsAsync(role.ToString()))
            {
                await userManager.AddToRoleAsync(user, role.ToString());
            }

            //obrisi usera iz baze
            throw new Exception("");
        }
    }
}
