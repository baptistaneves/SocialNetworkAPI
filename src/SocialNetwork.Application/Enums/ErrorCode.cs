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
       
        IdentityCreationFaild = 202,
        
        //Application error should be in the range 300-399
        PostUpdateNotPossible = 300,
        PostDeleteNotPossible = 301,
        InteractionRemovalNotAuthorized = 302,
        IdentityUserDoesNotExist = 303,
        InexistenUserProfile = 304,
        IncorrectPassword = 305,
        LockoutOnFailure = 306,
        UnathorizedAccountRemoval = 307,
        IdentityUserAlreadyExists = 308,
    }
}
