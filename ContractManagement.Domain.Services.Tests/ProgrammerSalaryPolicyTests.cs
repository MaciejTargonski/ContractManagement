using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractManagement.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractManagement.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace ContractManagement.DomainServices.Tests
{
    [TestClass()]
    [ExcludeFromCodeCoverage]
    public class ProgrammerSalaryPolicyTests
    {
        ProgrammerSalaryPolicy policy;

        [TestInitialize]
        public void Init()
        {
            policy = new ProgrammerSalaryPolicy();
        }

        [TestMethod()]
        public async Task RecommendSalaryAsyncTest()
        {
            byte exp = 2;

            var result = await policy.RecommendSalaryAsync(exp);

            var expected = 2750;

            Assert.AreEqual(expected, result.NetSalary);
        }

        [TestMethod()]
        public void AdjustSalaryBasedOnWorkExperianceTest()
        {
            var minimum = 10;
            byte exp = 2;

            var result = policy.AdjustSalaryBasedOnWorkExperiance(exp, minimum);

            var expected = 260;

            Assert.AreEqual(expected, result.NetSalary);

        }

        [TestMethod()]
        [ExpectedException(typeof(ProffessionalExperianceTooLowException))]
        public void GetMinimalSalaryThrowsTooLowExpException()
        {
            policy.GetMinimalSalary(proffessionalExperianceInYears: 0);
        }

        [TestMethod()]
        public void GetMinimalSalaryForBeginnerProgrammer()
        {

            var result = policy.GetMinimalSalary(proffessionalExperianceInYears: 2);

            Assert.AreEqual(2500, result);

        }

        [TestMethod()]
        public void GetMinimalSalaryForAdvancedProgrammerLowerBound()
        {

            var result = policy.GetMinimalSalary(proffessionalExperianceInYears: 3);

            Assert.AreEqual(5000, result);

        }

        [TestMethod()]
        public void GetMinimalSalaryForAdvancedProgrammerUpperBound()
        {

            var result = policy.GetMinimalSalary(proffessionalExperianceInYears: 5);

            Assert.AreEqual(5000, result);

        }

        [TestMethod()]
        public void GetMinimalSalaryForProffessionalProgrammer()
        {

            var result = policy.GetMinimalSalary(proffessionalExperianceInYears: 6);

            Assert.AreEqual(5500, result);

        }
    }
}