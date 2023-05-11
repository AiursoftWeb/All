﻿using System.ComponentModel.DataAnnotations;
using System.Net;
using Aiursoft.SDKTools.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aiursoft.SDKTools.Tests.AttributesTests;

[TestClass]
public class NoSpaceTests
{
    private NoSpace _validator;

    [TestInitialize]
    public void CreateValidator()
    {
        _validator = new NoSpace();
    }

    [TestMethod]
    [DataRow(nameof(HttpStatusCode))]
    [DataRow("aaaa.txt")]
    [DataRow("aaaatxt")]
    [DataRow("asdfasdfadfasdfasdfadf___^&(ad")]
    public void PassingTests(object inputValue)
    {
        Assert.AreEqual(ValidationResult.Success, _validator.TestEntry(inputValue));
    }

    [TestMethod]
    [DataRow(typeof(HttpStatusCode))]
    [DataRow(HttpStatusCode.OK)]
    [DataRow(5)]
    [DataRow("asdfasdf adfasdfasdfadf___^&(ad*")]
    [DataRow(@"
")]
    [DataRow("\u2029 \u2028")]
    [DataRow("\u2028\u2029 ")]
    [DataRow(" ")]
    [DataRow("    ")]
    public void FailingTests(object inputValue)
    {
        Assert.AreNotEqual(ValidationResult.Success, _validator.TestEntry(inputValue));
    }
}