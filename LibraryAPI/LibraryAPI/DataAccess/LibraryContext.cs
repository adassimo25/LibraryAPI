using System;
using System.Threading;

namespace LibraryAPI.DataAccess
{
    public class LibraryContext
    {
        public CancellationToken CancellationToken { get; private set; }
        public Guid UserId { get; private set; }

        public LibraryContext(CancellationToken cancellationToken)
        {
            CancellationToken = cancellationToken;
        }
    }
}