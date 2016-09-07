using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using ApiTypes = ContractManagement.WebApi.Types;
using AutoMapper.QueryableExtensions;
using ContractManagement.Domain.Models;
using System.Threading.Tasks;
using AutoMapper;

namespace ContractManagement.WebApi.Controllers.OData
{

    public class ContractsController : ODataController
    {
        private IContractsRepository contractsRepo;
        private ISalaryPolicyFactory salaryPolicyFactory;


        public ContractsController(IContractsRepository contractsRepo, ISalaryPolicyFactory salaryPolicyFactory)
        {
            this.contractsRepo = contractsRepo;
            this.salaryPolicyFactory = salaryPolicyFactory;
        }

        // GET: odata/Contracts
        [EnableQuery]
        public IQueryable<ApiTypes.Contract> GetContracts()
        {

            return contractsRepo.GetAll()
                .ProjectTo<ApiTypes.Contract>();
        }

        // GET: odata/Contracts(5)
        [EnableQuery]
        public SingleResult<ApiTypes.Contract> GetContract([FromODataUri] int key)
        {
            return SingleResult.Create(
                contractsRepo.GetAll()
                    .Where(contract => contract.Id == key)
                    .ProjectTo<ApiTypes.Contract>());
        }

        // POST: odata/Contracts
        public async Task<IHttpActionResult> Post(ApiTypes.Contract contract)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var domainContract = Mapper.Map<Contract>(contract);

            if(
                !salaryPolicyFactory
                .GetSalaryPolicy(domainContract.ContractType)
                .ContractSatisfiesPolicy(domainContract)
                )
            {
                return BadRequest("Given salary does not meet the salary policy");
            }

            try
            {
                //get the entity back with generated Id by EF
                contract = Mapper.Map<ApiTypes.Contract>(
                    contractsRepo.AddSingle(domainContract));
            }
            catch (System.ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            await contractsRepo.SaveChangesAsync();

            return Created(contract);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                contractsRepo.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
