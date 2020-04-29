using Microsoft.AspNetCore.Identity;

namespace ClassifiedAds.Data
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = $"An account for {userName} already exists."
            };
        }
    }
}
