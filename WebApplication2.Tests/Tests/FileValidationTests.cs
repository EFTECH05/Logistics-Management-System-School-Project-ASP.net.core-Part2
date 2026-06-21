using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication2.Tests.Tests
{
    public class FileValidationTests
    {
        [Fact]
        public void Only_PDF_Files_Are_Allowed()
        {
            string fileName = "contract.pdf";

            bool isValid = Path.GetExtension(fileName).ToLower() == ".pdf";

            Assert.True(isValid);
        }

        [Fact]
        public void Reject_Exe_Files()
        {
            string fileName = "virus.exe";

            bool isValid = Path.GetExtension(fileName).ToLower() == ".pdf";

            Assert.False(isValid);
        }
    }
}