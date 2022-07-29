namespace SocialNetwork.Application.Identity
{
    internal class IdentityErrorMessages
    {
        public const string IncorrectPassword = "Wrong username or password. Login failed.";
        public const string LockoutOnFailure = "User temporarily blocked for invalid attempts.";
        public const string IdentityUserAlreadyExists = "Provided email address already exists. Cannot register new user.";
    }
}
