using System.Threading.Tasks;

namespace Infrastructure.BaseAbstractions;

public interface ISaveChanges
{
    Task SaveChanges();
}