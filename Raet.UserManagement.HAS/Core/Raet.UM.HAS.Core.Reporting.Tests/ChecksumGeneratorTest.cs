using Raet.UM.HAS.Core.Reporting.Business;
using Xunit;

namespace Raet.UM.HAS.Core.Reporting.Tests
{
    public class ChecksumGeneratorTest
    {
        private ChecksumGenerator _checksumGenerator;

        [Fact]
        public void GenerateTest()
        {
            //Arrange
            _checksumGenerator = new ChecksumGenerator();

            //Act
            var result = _checksumGenerator.Generate("");

            //Assert
            Assert.Equal("E3-B0-C4-42-98-FC-1C-14-9A-FB-F4-C8-99-6F-B9-24-27-AE-41-E4-64-9B-93-4C-A4-95-99-1B-78-52-B8-55", result);

        }
    }
}
