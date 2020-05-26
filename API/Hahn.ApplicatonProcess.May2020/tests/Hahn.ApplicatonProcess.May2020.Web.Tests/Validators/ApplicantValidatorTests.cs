using FluentValidation.TestHelper;
using Moq;
using System.Net.Http;
using Xunit;
using MockHttpClient;
using System.Net;
using Hahn.ApplicatonProcess.May2020.Domain.Models;
using System.Collections.Generic;
using System.Collections;

namespace Hahn.ApplicatonProcess.May2020.Web.Tests.Validators
{
    public class ApplicantValidatorTests
    {
        private const string DummyURI = "http://blah.com";
        ApplicantValidator sut;
        readonly Mock<IHttpClientFactory> mockFactory = new Mock<IHttpClientFactory>();

        readonly MockHttpClient.MockHttpClient mockHttpClinet = new MockHttpClient.MockHttpClient();
        public ApplicantValidatorTests()
        {
            SetUpMockHttpClient();


        }

        private void SetUpMockHttpClient()
        {
            SetUpMockClient(HttpStatusCode.OK);

            mockHttpClinet.BaseAddress = new System.Uri(DummyURI);
            mockFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
               .Returns(mockHttpClinet);

        }

        [Theory]
        [ClassData(typeof(ApplicantTestData))]
        public void GivenAValidApplicant_Validation_Succeeds(Applicant applicant, bool isvalid)
        {


            sut = new ApplicantValidator(mockFactory.Object);

            if (isvalid)
            {
                Assert.True(sut.Validate(applicant).IsValid);
            }

            else
            {
                Assert.False(sut.Validate(applicant).IsValid);
            }
        }

        [Theory]
        [InlineData(-1, false)]
        [InlineData(100, false)]
        [InlineData(20, true)]
        [InlineData(60, true)]
        [InlineData(21, true)]
        public void GivenAge_ValidationConfirmsToSpecifiedExpectedResults(int age, bool isValid)
        {

            var sut = new ApplicantValidator(mockFactory.Object);

            var applicant = new Applicant
            {
                Age = age
            };
            if (isValid)
            {
                sut.ShouldNotHaveValidationErrorFor(x => x.Age, applicant);
            }

            else
            {
                sut.ShouldHaveValidationErrorFor(x => x.Age, applicant);
            }

        }


        [Theory]
        [InlineData("a@a", true)]
        [InlineData("@", false)]
        [InlineData("aa", false)]
        public void GivenEmailAddress_ValidationConfirmsToSpecifiedExpectedResults(string emailAddress, bool isValid)
        {

            var sut = new ApplicantValidator(mockFactory.Object);

            var applicant = new Applicant
            {
                EmailAddress = emailAddress
            };
            if (isValid)
            {
                sut.ShouldNotHaveValidationErrorFor(x => x.EmailAddress, applicant);
            }

            else
            {
                sut.ShouldHaveValidationErrorFor(x => x.EmailAddress, applicant);
            }

        }



        [Theory]
        [InlineData("Name1", true)]
        [InlineData("Name", false)]
        [InlineData("Name#@!~", true)]
        public void GivenFamilyName_ValidationConfirmsToSpecifiedExpectedResults(string familyName, bool isValid)
        {

            var sut = new ApplicantValidator(mockFactory.Object);

            var applicant = new Applicant
            {
                FamilyName = familyName
            };
            if (isValid)
            {
                sut.ShouldNotHaveValidationErrorFor(x => x.FamilyName, applicant);
            }

            else
            {
                sut.ShouldHaveValidationErrorFor(x => x.FamilyName, applicant);
            }

        }

        [Theory]
        [InlineData("Name1", true)]
        [InlineData("Name", false)]
        [InlineData("Name#@!~", true)]
        public void GivenName_ValidationConfirmsToSpecifiedExpectedResults(string name, bool isValid)
        {

            var sut = new ApplicantValidator(mockFactory.Object);

            var applicant = new Applicant
            {
                Name = name
            };
            if (isValid)
            {
                sut.ShouldNotHaveValidationErrorFor(x => x.Name, applicant);
            }

            else
            {
                sut.ShouldHaveValidationErrorFor(x => x.Name, applicant);
            }

        }


        [Theory]
        [InlineData("Name1Name1", true)]
        [InlineData("Name1Name1!!,/-0", true)]
        [InlineData("Name", false)]
        [InlineData("Name#@!~", false)]
        public void GivenAddress_ValidationConfirmsToSpecifiedExpectedResults(string address, bool isValid)
        {

            var sut = new ApplicantValidator(mockFactory.Object);

            var applicant = new Applicant
            {
                Address = address
            };
            if (isValid)
            {
                sut.ShouldNotHaveValidationErrorFor(x => x.Address, applicant);
            }

            else
            {
                sut.ShouldHaveValidationErrorFor(x => x.Address, applicant);
            }

        }


        [Theory]
        [InlineData("EU", false)]
        [InlineData("UK", true)]
        public void GivenCountryName_ValidationConfirmsToSpecifiedExpectedResults(string countryname, bool isValid)
        {

            var sut = new ApplicantValidator(mockFactory.Object);

            var applicant = new Applicant
            {
                CountryOfOrigin = countryname
            };

            if (isValid)
            {
                SetUpMockClient(HttpStatusCode.OK);

                sut.ShouldNotHaveValidationErrorFor(x => x.CountryOfOrigin, applicant);
            }

            else
            {
                SetUpMockClient(HttpStatusCode.BadRequest);

                sut.ShouldHaveValidationErrorFor(x => x.CountryOfOrigin, applicant);
            }

        }


        private void SetUpMockClient(HttpStatusCode httpStatusCode)
        {
            mockHttpClinet.When(x => true).Then(httpStatusCode);
        }


        public class ApplicantTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {
                    new Applicant {

                                     EmailAddress = "a@a",
                                    Age = 21,
                                    Address = "abcdeabcde",
                                    CountryOfOrigin = "DE",
                                    FamilyName = "efghi",
                                    Hired = false,
                                    Name = "uuwxyz"
                    }, true };
                yield return new object[] {  new Applicant {

                                     EmailAddress = "a@a",
                                    Age = -1,
                                    Address = "abcdeabcde",
                                    CountryOfOrigin = "DE",
                                    FamilyName = "efghi",
                                    Hired = false,
                                    Name = "uuwxyz"
                    }, false };

                yield return new object[] {  new Applicant {

                                     EmailAddress = "aa",
                                    Age = -1,
                                    Address = "abcdeabcde",
                                    CountryOfOrigin = "DE",
                                    FamilyName = "efghi",
                                    Hired = false,
                                    Name = "uuwxyz"
                    }, false };

                yield return new object[] {  new Applicant {

                                     EmailAddress = "a@a",
                                    Age = -1,
                                    Address = "abcdabcde",
                                    CountryOfOrigin = "DE",
                                    FamilyName = "efghi",
                                    Hired = false,
                                    Name = "uuwxyz"
                    }, false };
                yield return new object[] {  new Applicant {

                                     EmailAddress = "a@a",
                                    Age = -1,
                                    Address = "abcdeabcde",
                                    CountryOfOrigin = "DE",
                                    FamilyName = "eghi",
                                    Hired = false,
                                    Name = "uuwxyz"
                    }, false };

                yield return new object[] {  new Applicant {

                                     EmailAddress = "a@a",
                                    Age = -1,
                                    Address = "abcdeabcde",
                                    CountryOfOrigin = "DE",
                                    FamilyName = "efghi",
                                    Hired = false,
                                    Name = "uuwyz"
                    }, false };
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

    }
}
