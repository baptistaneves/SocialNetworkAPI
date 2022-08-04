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
        CommentRemovalNotAuthorized = 303,
        IdentityUserDoesNotExist = 304,
        InexistenUserProfile = 305,
        IncorrectPassword = 306,
        LockoutOnFailure = 307,
        UnathorizedAccountRemoval = 308,
        IdentityUserAlreadyExists = 309,
        CommentUpdateNotAuthorized = 310

    }
}
