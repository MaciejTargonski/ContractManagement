using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContractManagement.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.Data.Services
{
    public class EntityFrameworkContractsRepository : IContractsRepository
    {
        private IContractManagementDbContext dbContext;


        public EntityFrameworkContractsRepository(IContractManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Contract> GetAll()
        {

            return dbContext.Contracts.ProjectTo<Contract>();
        }

        public Contract AddSingle(Contract contract)
        {
            var addedEntry = dbContext.Contracts.Add(Mapper.Map<Data.Types.Contract>(contract));
            return Mapper.Map<Contract>(addedEntry);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {

            dbContext.Dispose();
        }

    }
}
