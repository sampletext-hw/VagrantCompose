using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.BaseAbstractions;
using Models.Db.Common;

namespace Infrastructure.Abstractions
{
    public interface ICategoryRepository : IGetById<Category>, IAdd<Category>, IUpdate<Category>, IRemove<Category>, IGetMany<Category>, IGetAllAsDictionary<Category>, ICount<Category>
    {
        Task<Category> GetByTitle(string title);

        Task<ICollection<Category>> GetAllOrdered();
    }
}