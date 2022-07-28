namespace SocialNetwork.Application.Enums
{
    public enum ErrorCode
    {
        NotFound = 404,
        ServerError = 500,
        UnknownError = 999,

        //Validation errors should be in the range 100-199
        ValidationError = 101,

        //Infrastructure erros should be in the range 200-299
        IdentityUserAlreadyExists = 201,
        IdentityCreationFaild = 202,
        IdentityUserDoesNotExist = 203,
        InexistenUserProfile = 204,
        IncorrectPassword = 205,
        LockoutOnFailure = 206,

        //Application error should be in the range 300-399
        PostUpdateNotPossible = 300,
        PostDeleteNotPossible = 301
    }
}
