using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Raet.UM.HAS.DTOs;
using System.IO;


namespace Raet.UM.HAS.Client.InitialLoad.FileGenerator.IntegrationTests
{
    [TestClass]
    public class InitialLoadFileGeneratorTests
    {
        private string _tenantId = "InitialLoadFileGeneratorTests";

        private readonly DateTime _defaultDateTime = new DateTime(2018, 1, 1);

        private List<string> _filesToDelete = new List<string>();

        [TestCleanup]
        public void DiscardFiles()
        {
            if (_filesToDelete.Count > 0)
            {
                foreach (string filepath in _filesToDelete)
                    File.Delete(filepath);

                _filesToDelete.Clear();
            }
        }

        [TestMethod]
        public void GenerateInitialLoadFile_RightEvent_Creates_File()
        {
            //arrange
            var authorizations = AuthorizationsHelper.GetAuthorizations(1, _tenantId, _defaultDateTime);
            var outputPath = ".\\";
            var _tempTenantId = _tenantId + DateTime.Now.Ticks;
            var filePath = string.Format("{0}\\InitialLoad_{1}.bin", outputPath, _tempTenantId);
            var generator = new InitialLoadFileGenerator();
            //the file needs to be removed after testing to prevent filling up disc with useless data.
            _filesToDelete.Add(filePath);

            //act
            generator.GenerateInitialLoadFile(authorizations, outputPath, _tempTenantId);

            //assert
            Assert.AreEqual(true, File.Exists(filePath));
        }
    }
}
