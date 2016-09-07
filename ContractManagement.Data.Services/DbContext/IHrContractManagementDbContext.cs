using System.Threading.Tasks;

namespace ContractManagement
{
    public interface IContractManagementDbContext
    {
        System.Data.Entity.DbSet<ContractManagement.Data.Types.Contract> Contracts { get; set; }

        Task SaveChangesAsync();
        void Dispose();
    }
}