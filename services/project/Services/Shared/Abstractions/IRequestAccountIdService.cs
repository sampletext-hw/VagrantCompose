namespace Services.Shared.Abstractions
{
    public interface IRequestAccountIdService
    {
        public long Id { get; }

        public bool IsSet { get; }
        public bool HasFullAccess { get; }
        public bool IsTechnical { get; }
    }

    public interface IRequestAccountIdSetterService
    {
        void Set(long id, bool hasFullAccess, bool isTechnical);
    }
}