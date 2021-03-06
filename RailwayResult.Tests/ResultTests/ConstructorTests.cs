﻿using System;
using FluentAssertions;
using Railway.Result;
using RailwayResultTests.StubDomain;
using Xunit;

namespace RailwayResultTests.ResultTests
{
    public class ConstructorTests
    {

        [Fact]
        public void ConstructWithNothing_ExpectIsNullFailure()
        {
            var result = Result<Customer>.ToResult(() => Repository.GetCustomer(Const.NullCustomerId));

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.FailureInfo.IsNull.Should().BeTrue();
        }

        [Fact]
        public void ConstructWithSuccess_ExpectSuccessAndBoolValueTrue()
        {
            var result = Result<bool>.Succeeded();

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.FailureInfo.Should().BeNull();
            result.ReturnValue.Should().Be(true);
        }

        [Fact]
        public void ConstructWithFailure_ExpectNotSuccessAndBoolValueFalse()
        {
            var result = Result<bool>.Failed();

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.FailureInfo.Should().NotBeNull();
            result.ReturnValue.Should().Be(false);
        }

        [Fact]
        public void ConstructFromBool_WhenValueTrue_ExpectSuccessAndTrue()
        {
            var result = Result<bool>.ToResult(true);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.FailureInfo.Should().BeNull();
            result.ReturnValue.Should().Be(true);
        }
        [Fact]
        public void ConstructFromBool_WhenValueFalse_ExpectSuccessAndFalse()
        {
            var result = Result<bool>.ToResult(false);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.FailureInfo.Should().BeNull();
            result.ReturnValue.Should().Be(false);
        }

        [Fact]
        public void ConstructFrom_WhenTrue_ExpectSuccessAndTrue()
        {
            var result = Result<bool>.FromBool(true);

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.FailureInfo.Should().BeNull();
            result.ReturnValue.Should().Be(true);
        }
        [Fact]
        public void ConstructFrom_WhenFalse_ExpectFailureAndFalse()
        {
            var result = Result<bool>.FromBool(false);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.FailureInfo.Should().NotBeNull();
            result.ReturnValue.Should().Be(false);
        }


        [Fact]
        public void ConstructWithCustomer_ExpectSuccessAndValue()
        {
            var result = Result<Customer>.ToResult(Repository.GetCustomer(Const.CustomerId));

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.FailureInfo.Should().BeNull();
            result.ReturnValue.Id.Should().Be(123);
        }

        [Fact]
        public void ConstructWithCustomerDelegate_ExpectSuccessAndValue()
        {
            var result = Result<Customer>.ToResult(() => Repository.GetCustomer(Const.CustomerId));

            result.IsSuccess.Should().BeTrue();
            result.IsFailure.Should().BeFalse();
            result.FailureInfo.Should().BeNull();
            result.ReturnValue.Id.Should().Be(123);
        }

        [Fact]
        public void ConstructWithFailureInfo_ExpectFailureValue()
        {
            var failureInfo = new ResultFailure(typeof(Customer), - 1, "error");

            var result = Result<Customer>.Failed(failureInfo);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.IsException.Should().BeFalse();
            result.FailureInfo.Should().NotBeNull();
            result.ReturnValue.Should().BeNull();
            result.FailureInfo.Code.Should().Be(-1);
            result.FailureInfo.Message.Should().Be("error");
        }

        [Fact]
        public void ConstructWithFailureMessage_ExpectFailureMessage()
        {
            var result = Result<Customer>.Failed("error");

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.IsException.Should().BeFalse();
            result.FailureInfo.Should().NotBeNull();
            result.ReturnValue.Should().BeNull();
            result.FailureInfo.Code.Should().Be(0);
            result.FailureInfo.Message.Should().Be("error");
        }

        [Fact]
        public void ConstructWithFailureCodeAndMessage_ExpectFailureCodeAndMessage()
        {
            var result = Result<Customer>.Failed(-1, "error");

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.IsException.Should().BeFalse();
            result.FailureInfo.Should().NotBeNull();
            result.ReturnValue.Should().BeNull();
            result.FailureInfo.Code.Should().Be(-1);
            result.FailureInfo.Message.Should().Be("error");
        }

        [Fact]
        public void ConstructWithException_ExpectFailureException()
        {
            var ex = new Exception("app error");
            var result = Result<Customer>.Failed(ex);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.IsException.Should().BeTrue();
            result.FailureInfo.Should().NotBeNull();
            result.ReturnValue.Should().BeNull();
            result.FailureInfo.Code.Should().Be(ex.HResult);
            result.FailureInfo.Message.Should().Be(ex.Message);
        }

        [Fact]
        public void ConstructWithExceptionAndMessage_ExpectFailureExceptionAndMessage()
        {
            var ex = new Exception("app error");
            var result = Result<Customer>.Failed(ex, "custom message");

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.IsException.Should().BeTrue();
            result.FailureInfo.Should().NotBeNull();
            result.ReturnValue.Should().BeNull();
            result.FailureInfo.Code.Should().Be(ex.HResult);
            result.FailureInfo.Message.Should().Be("custom message");
        }

        [Fact]
        public void ConstructWithExceptionMessageAndInstance_ExpectFailureExceptionMessageAndInstance()
        {
            var customer = Repository.GetCustomer(Const.CustomerId);
            var ex = new Exception("app error");
            var result = Result<Customer>.Failed(ex, "custom message", customer);

            result.IsSuccess.Should().BeFalse();
            result.IsFailure.Should().BeTrue();
            result.IsException.Should().BeTrue();
            result.FailureInfo.Should().NotBeNull();
            result.ReturnValue.Should().BeNull();
            result.FailureInfo.Code.Should().Be(ex.HResult);
            result.FailureInfo.Message.Should().Be("custom message");

            Assert.Same(result.FailureInfo.Object, customer);
        }

        [Fact]
        public void ConstructViaToResult_FromBool_ExpectBool()
        {
            bool value = false;
            Result<bool> result = value.ToResult();

            result.IsSuccess.Should().BeTrue();
            result.ReturnValue.Should().Be(false);
        }

        [Fact]
        public void ConstructViaToResult_FromCustomer_ExpectCustomer()
        {
            var customer = Repository.GetCustomer(Const.CustomerId);
            Result<Customer> result = customer.ToResult();

            result.IsSuccess.Should().BeTrue();
            result.ReturnValue.Should().Be(customer);
        }
    }
}
