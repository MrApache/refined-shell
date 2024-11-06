using System.Collections.Generic;
using RefinedShell.Tests.Examples.Correct;
using RefinedShell.Tests.Examples.Incorrect;

namespace RefinedShell.Tests.Examples;

internal static class ExampleCollection
{
    public static readonly HashSet<IExample> Examples =
    [
        new Example1(),
        new Example2(),
        new Example3(),
        new Example4(),
        new Example5(),
        new Example6(),
        new Example7_Error(),
        new Example8(),
        new Example9(),
        new Example10(),
        new Example11_Error(),
        new Example12(),
        new Example13(),
        new Example14(),
        new Example15_Error(),
        new Example16_Error(),
        new Example17_Error(),
        new Example18_Error(),
        new Example19_Error(),
        new Example20_Error(),
        new Example21_Error(),
        new Example22_Error(),
        new Example23_Error(),
        new Example24_Error(),
        new Example25_Error(),
        new Example26_Error(),
        new Example27_Error(),
        new Example28_Error(),
        new Example29_Error(),
        new Example30_Error()
    ];
}