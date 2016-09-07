using System.Data.Entity;
using System.Threading.Tasks;

namespace ContractManagement
{
    public class ContractManagementDbContext : DbContext, IContractManagementDbContext
    {

        public ContractManagementDbContext() : base("name=ContractManagementDbContext")
        {
        }

        public System.Data.Entity.DbSet<ContractManagement.Data.Types.Contract> Contracts { get; set; }

        async Task IContractManagementDbContext.SaveChangesAsync()
        {
            await this.SaveChangesAsync();
        }
    }
}
