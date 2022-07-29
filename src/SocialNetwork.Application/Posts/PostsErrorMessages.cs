﻿namespace SocialNetwork.Application.Posts
{
    internal class PostsErrorMessages
    {
        public const string PostNotFound = "No Post found with the especified ID {0}";
        public const string PostDeleteNotPossible = "Post delete not possible because it's not the post owner that initiates the update";
        public static readonly string PostUpdateNotPossible = "Post update not possible because it's not the post owner that initiates the update";
    }
}
