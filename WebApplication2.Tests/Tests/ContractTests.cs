using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
namespace WebApplication2.Tests.Tests
{
    public class ContractTests
    {
        [Fact]
        public void Contract_Status_Should_Be_Valid()
        {
            var contract = new Contract
            {
                ContractId = 1,
                Status = "Active"
            };

            Assert.True(contract.Status == "Active" ||
                        contract.Status == "Draft" ||
                        contract.Status == "Expired" ||
                        contract.Status == "On Hold");
        }
    }
}
