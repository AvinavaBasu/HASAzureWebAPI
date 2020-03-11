using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raet.UM.HAS.Core.Domain;

namespace Raet.UM.HAS.Core.Tests.Domain
{
    [TestClass]
    public class PersonalInfoTests
    {
        private readonly string _initials = "J.S";
        private readonly string _lastNameAtBirth = "Smith";
        private readonly string _lastNameAtBirthPrefix = "Mr";
        private readonly DateTime _birthDate = new DateTime(1980, 1, 1);

        private PersonalInfo GetPersonalInfo()
        {
            return new PersonalInfo() {
                Initials = _initials,
                LastNameAtBirth = _lastNameAtBirth,
                LastNameAtBirthPrefix = _lastNameAtBirthPrefix,
                BirthDate = _birthDate };
        }

        [TestMethod]
        public void Equals_OtherHasEqualFields_ReturnsTrue()
        {
            var firstPersonalInfo = GetPersonalInfo();
            var otherPersonalInfo = GetPersonalInfo();

            var comparisonResult = firstPersonalInfo.Equals(otherPersonalInfo);

            Assert.AreEqual(true, comparisonResult);
        }

        [TestMethod]
        public void Equals_OtherIsNull_ReturnsFalse()
        {
            //Arrange
            var firstPersonalInfo = GetPersonalInfo();

            //Act
            var comparisonResult = firstPersonalInfo.Equals(null);

            //Assert
            Assert.AreEqual(false, comparisonResult);
        }

        [TestMethod]
        public void Equals_OtherHasDifferentInitials_ReturnsFalse()
        {
            var firstPersonalInfo = GetPersonalInfo();
            var otherPersonalInfo = GetPersonalInfo();
            otherPersonalInfo.Initials = "someothervalue";

            var comparisonResult = firstPersonalInfo.Equals(otherPersonalInfo);

            Assert.AreEqual(false, comparisonResult);
        }

        [TestMethod]
        public void Equals_OtherHasDifferentLastNameAtBirth_ReturnsFalse()
        {
            var firstPersonalInfo = GetPersonalInfo();
            var otherPersonalInfo = GetPersonalInfo();
            otherPersonalInfo.LastNameAtBirth = "someothervalue";

            var comparisonResult = firstPersonalInfo.Equals(otherPersonalInfo);

            Assert.AreEqual(false, comparisonResult);
        }

        [TestMethod]
        public void Equals_OtherHasDifferentLastNameAtBirthPrefix_ReturnsFalse()
        {
            var firstPersonalInfo = GetPersonalInfo();
            var otherPersonalInfo = GetPersonalInfo();
            otherPersonalInfo.LastNameAtBirthPrefix = "someothervalue";

            var comparisonResult = firstPersonalInfo.Equals(otherPersonalInfo);

            Assert.AreEqual(false, comparisonResult);
        }

        [TestMethod]
        public void Equals_OtherHasDifferentBirthDate_ReturnsFalse()
        {
            var firstPersonalInfo = GetPersonalInfo();
            var otherPersonalInfo = GetPersonalInfo();
            otherPersonalInfo.BirthDate = otherPersonalInfo.BirthDate.AddYears(-1);

            var comparisonResult = firstPersonalInfo.Equals(otherPersonalInfo);

            Assert.AreEqual(false, comparisonResult);
        }

        [TestMethod]
        public void Equals_FirstHasNullInitialsAndOtherDoesnt_ReturnsFalse()
        {
            var firstPersonalInfo = GetPersonalInfo();
            var otherPersonalInfo = GetPersonalInfo();

            firstPersonalInfo.Initials = null;

            var comparisonResult = firstPersonalInfo.Equals(otherPersonalInfo);

            Assert.AreEqual(false, comparisonResult);
        }

        [TestMethod]
        public void Equals_FirstHasNullLastNameAtBirthAndOtherDoesnt_ReturnsFalse()
        {
            var firstPersonalInfo = GetPersonalInfo();
            var otherPersonalInfo = GetPersonalInfo();

            firstPersonalInfo.LastNameAtBirth = null;

            var comparisonResult = firstPersonalInfo.Equals(otherPersonalInfo);

            Assert.AreEqual(false, comparisonResult);
        }

        [TestMethod]
        public void Equals_FirstHasNullLastNameAtBirthPrefixAndOtherDoesnt_ReturnsFalse()
        {
            var firstPersonalInfo = GetPersonalInfo();
            var otherPersonalInfo = GetPersonalInfo();

            firstPersonalInfo.LastNameAtBirthPrefix = null;

            var comparisonResult = firstPersonalInfo.Equals(otherPersonalInfo);

            Assert.AreEqual(false, comparisonResult);
        }

        [TestMethod]
        public void Equals_FirstHasDefaultBirthDateDoesnt_ReturnsFalse()
        {
            var firstPersonalInfo = GetPersonalInfo();
            var otherPersonalInfo = GetPersonalInfo();

            firstPersonalInfo.BirthDate = default(DateTime);

            var comparisonResult = firstPersonalInfo.Equals(otherPersonalInfo);

            Assert.AreEqual(false, comparisonResult);
        }

        [TestMethod]
        public void Equals_BothHaveNullOrDefaultValues_ReturnsTrue()
        {
            var firstPersonalInfo = new PersonalInfo();
            var otherPersonalInfo = new PersonalInfo();

            var comparisonResult = firstPersonalInfo.Equals(otherPersonalInfo);

            Assert.AreEqual(null, firstPersonalInfo.Initials);
            Assert.AreEqual(null, firstPersonalInfo.LastNameAtBirth);
            Assert.AreEqual(null, firstPersonalInfo.LastNameAtBirthPrefix);
            Assert.AreEqual(default(DateTime), firstPersonalInfo.BirthDate);
            Assert.AreEqual(null, otherPersonalInfo.Initials);
            Assert.AreEqual(null, otherPersonalInfo.LastNameAtBirth);
            Assert.AreEqual(null, otherPersonalInfo.LastNameAtBirthPrefix);
            Assert.AreEqual(default(DateTime), otherPersonalInfo.BirthDate);

            Assert.AreEqual(true, comparisonResult);
        }
    }
}
