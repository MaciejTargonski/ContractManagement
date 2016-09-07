using ContractManagement.Domain.Models;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ContractManagement.WebApi.Controllers
{
    [RoutePrefix("api/recommendedsalary")]
    public class RecommendedSalaryController : ApiController
    {
        ISalaryPolicyFactory salaryPolicyFactory;
        public RecommendedSalaryController(ISalaryPolicyFactory salaryPolicyFactory)
        {
            this.salaryPolicyFactory = salaryPolicyFactory;
        }

        public async Task<IHttpActionResult> GetRecommendedSalary(SoftwareEngineerType engineerType, byte workExperianceInYears)
        {
            Salary salary = null;

            try
            {
                salary = await salaryPolicyFactory.GetSalaryPolicy(engineerType).RecommendSalaryAsync(workExperianceInYears);

            }
            catch (ProffessionalExperianceTooLowException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotSupportedException ex)
            {
                return BadRequest(ex.Message);
            }

            if (salary == null)
            {
                return BadRequest();
            }

            return Ok(salary);
        }


        [ResponseType(typeof(Salary))]
        [Route("programmer/{exp:int}", Name = "RecommendedSalaryForProgrammer")]
        public async Task<IHttpActionResult> GetRecommendedSalaryForProgrammer(int exp)
        {
            return await GetRecommendedSalary(SoftwareEngineerType.Programmer, (byte)exp);
        }


        [ResponseType(typeof(Salary))]
        [Route("tester/{exp:int}", Name = "RecommendedSalaryForTester")]
        public async Task<IHttpActionResult> GetRecommendedSalaryForTester(int exp)
        {
            return await GetRecommendedSalary(SoftwareEngineerType.Tester, (byte)exp);
        }

    }
}
