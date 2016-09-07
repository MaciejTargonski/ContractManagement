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
    public class TesterSalaryPolicyTests
    {
        TesterSalaryPolicy policy;

        [TestInitialize]
        public void Init()
        {
            policy = new TesterSalaryPolicy();
        }

        [TestMethod()]
        public async Task RecommendSalaryAsyncTest()
        {
            byte exp = 2;

            var result = await policy.RecommendSalaryAsync(exp);

            var expected = 3575;

            Assert.AreEqual(expected, result.NetSalary);
        }

        [TestMethod()]
        public void AdjustSalaryBasedOnWorkExperianceTest()
        {
            var minimum = 10;
            byte exp = 2;

            var result = policy.AdjustSalaryBasedOnWorkExperiance(exp, minimum);

            var expected = 212;

            Assert.AreEqual(expected, result.NetSalary);

        }

        [TestMethod()]
        [ExpectedException(typeof(ProffessionalExperianceTooLowException))]
        public void GetMinimalSalaryThrowsTooLowExpException()
        {
            policy.GetMinimalSalary(proffessionalExperianceInYears: 0);
        }

        [TestMethod()]
        public void GetMinimalSalaryForBeginnerTester()
        {

            var result = policy.GetMinimalSalary(proffessionalExperianceInYears: 1);

            Assert.AreEqual(2000, result);

        }

        [TestMethod()]
        public void GetMinimalSalaryForAdvancedTesterLowerBound()
        {

            var result = policy.GetMinimalSalary(proffessionalExperianceInYears: 2);

            Assert.AreEqual(2700, result);

        }

        [TestMethod()]
        public void GetMinimalSalaryForAdvancedTesterUpperBound()
        {

            var result = policy.GetMinimalSalary(proffessionalExperianceInYears: 4);

            Assert.AreEqual(2700, result);

        }

        [TestMethod()]
        public void GetMinimalSalaryForTesterProgrammer()
        {

            var result = policy.GetMinimalSalary(proffessionalExperianceInYears: 5);

            Assert.AreEqual(3200, result);

        }
    }
}