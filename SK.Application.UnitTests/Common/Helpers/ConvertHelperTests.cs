using FluentAssertions;
using NUnit.Framework;
using SK.Application.Common.Exceptions;
using SK.Application.Common.Helpers;
using SK.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SK.Application.UnitTests.Common.Helpers
{ 
    public class ConvertHelperTests
    {
        [Test]
        public void Should_ConvertNumberTypeOfFieldEnumToStringType_Success()
        {
            TypeOfField inputTypeOfField = TypeOfField.Number;

            string result = ConvertHelper.ConvertTypeOfFieldEnumToStringType(inputTypeOfField);

            Assert.AreEqual("int", result);
        }

        [Test]
        public void Should_ConvertTextTypeOfFieldEnumToStringType_Success()
        {
            TypeOfField inputTypeOfField = TypeOfField.Text;

            string result = ConvertHelper.ConvertTypeOfFieldEnumToStringType(inputTypeOfField);

            Assert.AreEqual("string", result);
        }

        [Test]
        public void Should_ConvertStringToTypeOfFieldEnum_Success()
        {
            string inputString = "string";

            var result = ConvertHelper.ConvertStringTypeToTypeOfFieldEnum(inputString);

            Assert.AreEqual(TypeOfField.Text, result);
        }

        [Test]
        public void Should_ConvertIntToTypeOfFieldEnum_Success()
        {
            string inputString = "int";

            var result = ConvertHelper.ConvertStringTypeToTypeOfFieldEnum(inputString);

            Assert.AreEqual(TypeOfField.Number, result);
        }

        [Test]
        public void Should_ConvertUnknownValueToTypeOfFieldEnum_Failure()
        {
            string inputString = "abcdef";

            FluentActions.Invoking(() =>
                ConvertHelper.ConvertStringTypeToTypeOfFieldEnum(inputString)).Should().Throw<ArgumentOutOfRangeException>();
        }

        [Test]
        public void Should_ConvertEmptyStringToTypeOfFieldEnum_Failure()
        {
            string inputString = String.Empty;

            FluentActions.Invoking(() =>
                ConvertHelper.ConvertStringTypeToTypeOfFieldEnum(inputString)).Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
