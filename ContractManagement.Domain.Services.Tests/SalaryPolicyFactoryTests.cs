using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContractManagement.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Moq;
using ContractManagement.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace ContractManagement.DomainServices.Tests
{
    [TestClass()]
    [ExcludeFromCodeCoverage]
    public class SalaryPolicyFactoryTests
    {
        SalaryPolicyFactory factory;

        private Mock<IProgrammerSalaryPolicy> programmerSalaryPolicyMock;
        private Mock<ITesterSalaryPolicy> testerSalaryPolicyMock;

        [TestInitialize]
        public void Init()
        {
            programmerSalaryPolicyMock = new Mock<IProgrammerSalaryPolicy>();
            testerSalaryPolicyMock = new Mock<ITesterSalaryPolicy>();

            factory = new SalaryPolicyFactory(programmerSalaryPolicyMock.Object, testerSalaryPolicyMock.Object);
        }

        [TestMethod()]
        public void GetSalaryPolicyForProgrammer()
        {
            //assign

            var programmer = SoftwareEngineerType.Programmer;

            //act
            var result = factory.GetSalaryPolicy(programmer);

            //assert
            Assert.AreSame(programmerSalaryPolicyMock.Object, result);
        }

        [TestMethod()]
        public void GetSalaryPolicyForTester()
        {
            //assign

            var tester = SoftwareEngineerType.Tester;

            //act
            var result = factory.GetSalaryPolicy(tester);

            //assert
            Assert.AreSame(testerSalaryPolicyMock.Object, result);
        }
    }
}