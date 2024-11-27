using System;
using System.Collections.Generic;
using RefinedShell.Tests.Examples.Correct;
using RefinedShell.Tests.Examples.Correct.Instance;
using RefinedShell.Tests.Examples.Incorrect;

namespace RefinedShell.Tests.Examples;

internal static class TestCaseCollection
{
    public static readonly HashSet<ITestCase> Examples =
    [
        new TestCase1_Instance(),
        new TestCase2_Instance(),
        new TestCase3_Instance(),
        new TestCase9(),
        new TestCase8(),
        new TestCase10_Error(),
        new TestCase16_Error(),
        new TestCase7(),
        new TestCase6(),
        new TestCase5(),
        new TestCase15_Error(),
        new TestCase4(),
        new TestCase14_Error(),
        new TestCase3(),
        new TestCase13_Error(),
        new TestCase12_Error(),
        new TestCase11_Error(),
        new TestCase23_Error(),
        new TestCase22_Error(),
        new TestCase21_Error(),
        new TestCase20_Error(),
        new TestCase19_Error(),
        new TestCase18_Error(),
        new TestCase24_Error(),
        new TestCase17_Error(),
        new TestCase25_Error(),
        new TestCase26_Error(),
        new TestCase27_Error(),
        new TestCase28_Error(),
        new TestCase29_Error(),
        new Example31_Error(),
        new TestCase4_Instance(),
        new TestCase5_Instance(),
        new TestCase6_Instance(),
        new TestCase7_Instance(),
        new TestCase32_Error(),
        new TestCase31_Error(),
        new TestCase2(),
        new TestCase1(),
        new TestCase8_Instance(),
        new TestCase30_Error(),
        //new TestCase42_IfElse()
    ];

    public static readonly HashSet<Type> CorrectInstanceTypes =
    [
        typeof(TestCase1_Instance),
        typeof(TestCase2_Instance),
        typeof(TestCase3_Instance),
        typeof(TestCase4_Instance),
        typeof(TestCase5_Instance),
        typeof(TestCase6_Instance),
        typeof(TestCase7_Instance),
        typeof(TestCase8_Instance)
    ];
}