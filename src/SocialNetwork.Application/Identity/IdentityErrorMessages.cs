namespace SocialNetwork.Application.Identity
{
    internal class IdentityErrorMessages
    {
        public const string IncorrectPassword = "Wrong username or password. Login failed.";
        public const string LockoutOnFailure = "User temporarily blocked for invalid attempts.";
        public const string IdentityUserAlreadyExists = "Provided email address already exists. Cannot register new user.";
        public const string NonExistentIdentityUser = "Unable to find a user with the especified username";
        public const string UnathorizedAccountRemoval = "Cannot remove account as you are not its owner";
    }
}
