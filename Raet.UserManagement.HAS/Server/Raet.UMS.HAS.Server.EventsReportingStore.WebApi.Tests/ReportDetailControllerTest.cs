using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raet.UM.HAS.Core.Domain;
using Raet.UM.HAS.Core.Reporting.Interface;
using Raet.UM.HAS.DTOs;
using Raet.UM.HAS.Mocks;
using Raet.UMS.HAS.Server.EventReportingStore.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Permission = Raet.UM.HAS.Core.Domain.Permission;

namespace Raet.UMS.HAS.Server.EventsReportingStore.WebApi.Tests
{
    [TestClass]
    public class ReportDetailControllerTest
    {
        private Mock<IEAAggregateBusiness> _EAAggregateBusiness;
        private MockInMemoryLogger _Logger;
        private ReportDetailController _Controller;
        public ReportDetailControllerTest()
        {
            _EAAggregateBusiness = new Mock<IEAAggregateBusiness>();
            _Logger = new MockInMemoryLogger();
        }

        Func<List<EffectiveAuthorizationInterval>> GetEnrichedData = () =>
        {
            List<EffectiveAuthorizationInterval> effectiveAuthorizationIntervals = new List<EffectiveAuthorizationInterval>();
            for (int i = 1; i <= 3; i++)
            {
                var uniqueId = i.ToString();
                var tenantId = $"tenant-{uniqueId}";
                var interval = new Interval(DateTime.Now.AddMonths(-i), DateTime.Now);
                var externalId = new UM.HAS.Core.Domain.ExternalId() { Context = $"test.context-{uniqueId}", Id = $"test-{uniqueId}" };
                var personInfo = new PersonalInfo()
                {
                    BirthDate = new DateTime(1992, 3, 15),
                    Initials = $"user-{uniqueId}",
                    LastNameAtBirth = $"user-{uniqueId}",
                };
                var targetPersonInfo = new PersonalInfo()
                {
                    BirthDate = new DateTime(1992, 3, 15),
                    Initials = $"target-user-{uniqueId}",
                    LastNameAtBirth = $"target-user-{uniqueId}",
                };
                var permission = new Permission()
                {
                    Application = $"youforce-{uniqueId}",
                    Id = uniqueId,
                    Description = "youforce"
                };
                var sourceUser = new Person(externalId, personInfo);
                var targetUser = new Person(externalId, targetPersonInfo);
                effectiveAuthorizationIntervals.Add(new EffectiveAuthorizationInterval(interval, sourceUser, targetUser, permission, tenantId));
            }
            return effectiveAuthorizationIntervals;
        };



        [TestMethod]
        public void When_ApplicationEndPoint_Is_Called_Then_Should_Have_ValidApplication()
        {
            var applications = Task.FromResult(GetEnrichedData().Select(e => e.Permission.Application).ToList() as IList<string>);
            _EAAggregateBusiness.Setup(e => e.GetApplication(It.IsAny<string>())).Returns(applications);
            _Controller = new ReportDetailController(_EAAggregateBusiness.Object, _Logger);
            var result = _Controller.Application(It.IsAny<string>()).Result;
            var objectResult = (ObjectResult)result;
            var actualPermissions = (IList<string>)objectResult.Value;

            Assert.IsTrue(actualPermissions.Count == 3);
            Assert.IsTrue(actualPermissions[0].Contains("youforce-1"));
            Assert.AreEqual(objectResult.StatusCode, 200);
        }

        [TestMethod]
        public void When_Application_List_Is_Empty()
        {
            //Arrange
            IList<string> application = new List<string>();
            _EAAggregateBusiness.Setup(e => e.GetApplication(It.IsAny<string>())).Returns(Task.FromResult(application));
            _Controller = new ReportDetailController(_EAAggregateBusiness.Object, _Logger);

            //Act
            var result = (NoContentResult)_Controller.Application(It.IsAny<string>()).Result;

            //Assert
            Assert.AreEqual(204, result.StatusCode);
        }

        [TestMethod]
        public void When_Application_List_ThrowsError()
        {
            //Arrange
            _EAAggregateBusiness.Setup(e => e.GetApplication(It.IsAny<string>())).Throws<Exception>();
            _Controller = new ReportDetailController(_EAAggregateBusiness.Object, _Logger);

            //Act
            var result = (StatusCodeResult)_Controller.Application(It.IsAny<string>()).Result;

            //Assert
            Assert.AreEqual(500, result.StatusCode);
        }

        [TestMethod]
        public void When_PermissionEndPointWithValidApplication_Is_Called_Then_Should_Have_ValidPermission()
        {
            var enrichedData = GetEnrichedData();
            var appIds = enrichedData.Select(e => e.Permission.Application);

            var permissions = Task.FromResult(enrichedData.Where(x=>appIds.Contains(x.Permission.Application)).Select(e => new PermissionDto()
            {
                Description = e.Permission.Description,
                Id = e.Permission.Id,
                Application = e.Permission.Application
            }).ToList() as IList<PermissionDto>);
            _EAAggregateBusiness.Setup(e => e.GetPermissionData(It.IsAny<string>(), It.IsAny<string>())).Returns(permissions);
            _Controller = new ReportDetailController(_EAAggregateBusiness.Object, _Logger);

            var result = _Controller.Permission(appIds.FirstOrDefault(), It.IsAny<string>()).Result;
            var objectResult = (ObjectResult)result;
            var actualPermissions = (IList<PermissionDto>)objectResult.Value;

            Assert.IsTrue(actualPermissions.Count > 0);
            Assert.IsTrue(actualPermissions[0].Id.Contains("1"));
            Assert.IsTrue(actualPermissions[0].Application.Contains("youforce-1"));
            Assert.AreEqual(objectResult.StatusCode, 200);
        }

        [TestMethod]
        public void When_Permission_List_Is_Empty()
        {
            //Arrange
            IList<PermissionDto> permissions = new List<PermissionDto>();
            _EAAggregateBusiness.Setup(e => e.GetPermissionData(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(permissions));
            _Controller = new ReportDetailController(_EAAggregateBusiness.Object, _Logger);

            //Act
            var result = (NoContentResult)_Controller.Permission(It.IsAny<string>(), It.IsAny<string>()).Result;

            //Assert
            Assert.AreEqual(204, result.StatusCode);
        }

        [TestMethod]
        public void When_Permission_List_ThrowsError()
        {
            //Arrange
            _EAAggregateBusiness.Setup(e => e.GetPermissionData(It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
            _Controller = new ReportDetailController(_EAAggregateBusiness.Object, _Logger);

            //Act
            var result = (StatusCodeResult)_Controller.Permission(It.IsAny<string>(), It.IsAny<string>()).Result;

            //Assert
            Assert.AreEqual(500, result.StatusCode);
        }

        [TestMethod]
        public void When_SourceUserEndPointWithValidPermission_Is_Called_Then_Should_Have_ValidUser()
        {
            //Arrange
            IList<CustomUser> customUsers = new List<CustomUser>();
            customUsers.Add(new CustomUser() { UserName = "TestScouceUser"});
            _EAAggregateBusiness.Setup(e => e.GetUsers(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(customUsers));
            _Controller = new ReportDetailController(_EAAggregateBusiness.Object, _Logger);

            //Act
            var result = _Controller.SourceUser(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<string>()).Result;
            var objectResult = (ObjectResult)result;
            var actualUsers = (IList<CustomUser>)objectResult.Value;

            //Assert
            Assert.IsTrue(actualUsers.Count > 0);
            Assert.IsTrue(actualUsers[0].UserName.Contains("TestScouceUser"));
            Assert.AreEqual(objectResult.StatusCode, 200);
        }

        [TestMethod]
        public void When_SorceUser_List_Is_Empty()
        {
            //Arrange
            IList<CustomUser> customUsers = new List<CustomUser>();
            _EAAggregateBusiness.Setup(e => e.GetUsers(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(customUsers));
            _Controller = new ReportDetailController(_EAAggregateBusiness.Object, _Logger);

            //Act
            var result = (NoContentResult)_Controller.SourceUser(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<string>()).Result;

            //Assert
            Assert.AreEqual(204, result.StatusCode);
        }

        [TestMethod]
        public void When_SorceUser_List_Throws_Error()
        {
            //Arrange
            _EAAggregateBusiness.Setup(e => e.GetUsers(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
            _Controller = new ReportDetailController(_EAAggregateBusiness.Object, _Logger);

            //Act
            var result = (StatusCodeResult)_Controller.SourceUser(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<string>()).Result;

            //Assert
            Assert.AreEqual(500, result.StatusCode);
        }

        [TestMethod]
        public void When_TargetUserEndPointWithValidPermission_Is_Called_Then_Should_Have_ValidUser()
        {

            //Arrange
            IList<CustomUser> customUsers = new List<CustomUser>();
            customUsers.Add(new CustomUser() { UserName = "TestTargetUser" });
            _EAAggregateBusiness.Setup(e => e.GetUsers(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(customUsers));
            _Controller = new ReportDetailController(_EAAggregateBusiness.Object, _Logger);

            //Act
            var result = _Controller.TargetUser(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<string>()).Result;
            var objectResult = (ObjectResult)result;
            var actualUsers = (IList<CustomUser>)objectResult.Value;

            //Assert
            Assert.IsTrue(actualUsers.Count > 0);
            Assert.IsTrue(actualUsers[0].UserName.Contains("TestTargetUser"));
            Assert.AreEqual(objectResult.StatusCode, 200);
        }

        [TestMethod]
        public void When_TargetUser_List_Is_Empty()
        {
            //Arrange
            IList<CustomUser> customUsers = new List<CustomUser>();
            _EAAggregateBusiness.Setup(e => e.GetUsers(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(customUsers));
            _Controller = new ReportDetailController(_EAAggregateBusiness.Object, _Logger);

            //Act
            var result = (NoContentResult)_Controller.TargetUser(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<string>()).Result;

            //Assert
            Assert.AreEqual(204, result.StatusCode);
        }

        [TestMethod]
        public void When_TargetUser_List_Throws_Error()
        {
            //Arrange
            _EAAggregateBusiness.Setup(e => e.GetUsers(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws<Exception>();
            _Controller = new ReportDetailController(_EAAggregateBusiness.Object, _Logger);

            //Act
            var result = (StatusCodeResult)_Controller.TargetUser(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<string>()).Result;

            //Assert
            Assert.AreEqual(500, result.StatusCode);
        }

        private void setupUserApiEndPoint(string userType)
        {
            var enrichedData = GetEnrichedData();
            var permissionIds = enrichedData.Select(e => e.Permission.Id).ToList();
            if (userType.Contains("sourceUser"))
            {
                var user = Task.FromResult(enrichedData.Select(e => e.User).ToList() as IList<CustomUser>);
                _EAAggregateBusiness.Setup(e => e.GetUsers(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).
                    Returns(user);
            }
            else
            {
                var user = Task.FromResult(enrichedData.Select(e => e.TargetPerson).ToList() as IList<CustomUser>);
                _EAAggregateBusiness.Setup(e => e.GetUsers(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(user);
            }
            _Controller = new ReportDetailController(_EAAggregateBusiness.Object, _Logger);
        }
    }
}   
