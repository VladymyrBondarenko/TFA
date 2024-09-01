﻿
using TFA.Domain.Exceptions;

namespace TFA.Domain.UseCases.GetForums
{
    internal static class GetForumsStorageExtensions
    {
        public static async Task<bool> ForumExists(this IGetForumsStorage storage,
            Guid forumId, CancellationToken cancellationToken)
        {
            var forums = await storage.GetForums(cancellationToken);
            return forums.Any(x => x.Id == forumId);
        }

        public static async Task ThrowIfForumNotFound(this IGetForumsStorage storage,
            Guid forumId, CancellationToken cancellationToken)
        {
            if(!await storage.ForumExists(forumId, cancellationToken))
            {
                throw new ForumNotFoundException(forumId);
            }
        } 
    }
}
