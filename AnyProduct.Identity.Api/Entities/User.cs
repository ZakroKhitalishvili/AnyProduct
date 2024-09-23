using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
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
    public Task<IdentityResult> ValidateAsync([NotNull] UserManager<User> manager, [NotNull] User user)
    {
        var result = IdentityResult.Success;

        if (!string.IsNullOrWhiteSpace(user.PersonalNumber))
        {
            string pattern = @"^\d{11}$";

            bool isMatch = Regex.IsMatch(user.PersonalNumber, pattern);

            if (!isMatch)
            {
                result = IdentityResult.Failed(new IdentityError()
                {
                    Code = "WrongPersonalNumber",
                    Description = "Personal number should contain only 11 digits"
                });
            }

        }

        return Task.FromResult(result);

    }
}