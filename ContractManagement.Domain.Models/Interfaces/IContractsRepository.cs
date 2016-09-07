using System;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.Domain.Models
{
    public interface IContractsRepository : IDisposable
    {
        IQueryable<Contract> GetAll();
        Contract AddSingle(Contract contract);
        Task SaveChangesAsync();
    }
}