using Services.Shared.Abstractions;

namespace Services.Shared.Implementations
{
    public class RequestAccountIdService : IRequestAccountIdService, IRequestAccountIdSetterService
    {
        public long Id { get; private set; }
        public bool IsSet { get; private set; }
        public bool HasFullAccess { get; set; }

        public bool IsTechnical { get; set; }

        public void Set(long id, bool hasFullAccess, bool isTechnical)
        {
            Id = id;
            IsSet = true;
            HasFullAccess = hasFullAccess;
            IsTechnical = isTechnical;
        }
    }
}