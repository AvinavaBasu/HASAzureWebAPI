using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Raet.UM.HAS.Client.InitialLoad.FileGenerator;
using Raet.UM.HAS.Client.InitialLoad.FileGenerator.IntegrationTests;
using Raet.UM.HAS.Client.InitialLoad.FileReader;
using Raet.UM.HAS.DTOs;
using System.IO;

namespace Raet.UM.HAS.Client.InitialLoad.IntegrationTests
{
    [TestClass]
    public class InitialLoadTests
    {
        private readonly string _tenantId = "InitialLoadTests";
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
        public void FileGenerator_GenerateInitialLoadFile_ValidEvent_FileReader_ReadInitialLoadFile_Returns_ValidEvent()
        {
            //arrange
            var authorizations = AuthorizationsHelper.GetAuthorizations(1, _tenantId, _defaultDateTime);
            var outputPath = ".\\";
            var _tempTenantId = _tenantId + DateTime.Now.Ticks;
            var filePath = string.Format("{0}\\InitialLoad_{1}.bin", outputPath, _tempTenantId);
            var generator = new InitialLoadFileGenerator();
            var reader = new InitialLoadFileReader();

            _filesToDelete.Add(filePath);
            //act
            generator.GenerateInitialLoadFile(authorizations, outputPath, _tempTenantId);
            var eaGrantedEvents = reader.ReadInitialLoadFile(filePath);

            //assert
            Assert.IsNotNull(eaGrantedEvents);
            Assert.AreEqual(1, eaGrantedEvents.Count);
            Assert.AreEqual(authorizations[0].EffectiveAuthorization, eaGrantedEvents[0].EffectiveAuthorization);

        }
    }
}
