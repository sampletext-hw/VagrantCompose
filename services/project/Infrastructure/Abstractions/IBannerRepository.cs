using Infrastructure.BaseAbstractions;
using Models.Db;

namespace Infrastructure.Abstractions
{
    using T = Banner;

    public interface IBannerRepository : IGetById<T>, IAdd<T>, IRemove<T>, IUpdate<T>, IGetMany<T>, ICount<T>
    {
    }
}