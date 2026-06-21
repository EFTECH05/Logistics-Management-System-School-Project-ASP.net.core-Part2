using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication2.Tests.Helpers;

namespace WebApplication2.Tests.Tests
{
    public class ServiceRequestTests
    {
        [Fact]
        public void Can_Create_ServiceRequest()
        {
            // Arrange
            var context = TestHelper.GetInMemoryContext();

            var request = new ServiceRequest
            {
                ContractId = 1,
                Description = "Test request",
                CostUSD = 100,
                CostZAR = 1800,
                Status = "Pending"
            };

            // Act
            context.ServiceRequests.Add(request);
            context.SaveChanges();

            // Assert
            Assert.Single(context.ServiceRequests);
        }
    }
}
