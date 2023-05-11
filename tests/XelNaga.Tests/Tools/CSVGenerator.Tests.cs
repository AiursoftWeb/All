﻿using System;
using Aiursoft.XelNaga.Attributes;
using Aiursoft.XelNaga.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aiursoft.XelNaga.Tests.Tools;

internal class Model
{
    [CSVProperty(nameof(Id))] public int Id { get; set; }

    public string Useless { get; set; }
}

internal class Person : Model
{
    [CSVProperty("Person Name")] public string Name { get; set; }
}

[TestClass]
public class CSVGeneratorTests
{
    [TestMethod]
    public void BuildFromListEmpty()
    {
        var persons = Array.Empty<Person>();
        var expect = @"""Person Name"",""Id""".Trim();
        var generator = new CSVGenerator();
        var generated = generator.BuildFromCollection(persons).BytesToString();
        Assert.AreEqual(expect, generated.Trim());
    }

    [TestMethod]
    public void BuildFromList()
    {
        var persons = new[]
        {
            new Person { Id = 1, Name = "Alice Li" },
            new Person { Id = 2, Name = "我能吞下玻璃而不伤身体。" }
        };

        var newLine = Environment.NewLine;
        var expect = "\"Person Name\",\"Id\"" + newLine + "\"Alice Li\",\"1\"" + newLine +
                     "\"我能吞下玻璃而不伤身体。\",\"2\"".Trim();
        var generator = new CSVGenerator();
        var generated = generator.BuildFromCollection(persons).BytesToString();
        Assert.AreEqual(expect, generated.Trim());
    }
}