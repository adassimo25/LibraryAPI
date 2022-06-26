using System;
using System.Threading;

namespace LibraryAPI.DataAccess
{
    public class LibraryContext
    {
        public CancellationToken CancellationToken { get; private set; }
        public Guid UserId { get; private set; }

        public LibraryContext(Guid userId, CancellationToken cancellationToken)
        {
            UserId = userId;
            CancellationToken = cancellationToken;
        }
    }
}