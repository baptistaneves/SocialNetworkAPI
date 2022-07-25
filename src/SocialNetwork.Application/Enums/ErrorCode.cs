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
        IdentityCreationFaild = 202
    }
}
