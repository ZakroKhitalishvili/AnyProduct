using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace AnyProduct.Identity.Api.Entities;

public class User : IdentityUser
{
    public string FullName { get; set; }

    public string PersonalNumber { get; set; }

    public User() : base() { }
}

public class UserValidator : IUserValidator<User>
{
    public async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
    {
        if (!string.IsNullOrWhiteSpace(user.PersonalNumber))
        {
            string pattern = @"^\d{11}$";

            bool isMatch = Regex.IsMatch(user.PersonalNumber, pattern);

            if (!isMatch)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "WrongPersonalNumber",
                    Description = "Personal number should contain only 11 digits"
                });
            }

        }

        return IdentityResult.Success;


    }
}