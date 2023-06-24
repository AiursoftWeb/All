﻿using System.Linq;
using System.Text;
using Aiursoft.CSTools.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aiursoft.XelNaga.Tests.Tools;

[TestClass]
public class StringOperationTests
{
    [TestMethod]
    public void BytesToBase64()
    {
        var bytes = Encoding.ASCII.GetBytes("my_test_string_(17)[]@$");

        var result = bytes.BytesToBase64();

        Assert.AreEqual(result, "bXlfdGVzdF9zdHJpbmdfKDE3KVtdQCQ=");
    }

    [TestMethod]
    public void Base64ToBytes()
    {
        var base64 = "bXlfdGVzdF9zdHJpbmdfKDE3KVtdQCQ=";

        var result = base64.Base64ToBytes();

        var bytes = Encoding.ASCII.GetBytes("my_test_string_(17)[]@$");

        Assert.IsTrue(result.SequenceEqual(bytes));
    }

    [TestMethod]
    public void GetMd5()
    {
        var source = "my_test_string_(17)[]@$";

        var result = source.GetMd5();

        Assert.AreEqual(result, "0a4112dd96480d0f3eec8ce5b42082a6");
    }

    [TestMethod]
    public void IsValidJsonTest()
    {
        Assert.AreEqual(false, "my_test_string_(17)[]@$".IsValidJson());
        Assert.AreEqual(true, @"[""a"",""b""]".IsValidJson());
        Assert.AreEqual(true, @"[]".IsValidJson());
        Assert.AreEqual(true, @"{}".IsValidJson());
        Assert.AreEqual(true, @"""""".IsValidJson());
        Assert.AreEqual(false, @"""aaaaa,aaa""ddddddddd""".IsValidJson());
        Assert.AreEqual(false, @"{""a"":""a"",""b"":""b""]".IsValidJson());
        Assert.AreEqual(true, @"{""a"":""a"",""b"":""b""}".IsValidJson());
    }
}