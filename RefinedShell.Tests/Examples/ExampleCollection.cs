using System;
using System.Collections.Generic;
using RefinedShell.Tests.Examples.Correct;
using RefinedShell.Tests.Examples.Correct.Instance;
using RefinedShell.Tests.Examples.Incorrect;

namespace RefinedShell.Tests.Examples;

internal static class ExampleCollection
{
    public static readonly HashSet<IExample> Examples =
    [
        new Example1_Instance_StringArgument(),
        new Example2_Instance_StringArgument(),
        new Example3_Instance_Sequence_Inline(),
        new Example4_TwoArguments_Number(),
        new Example5_Sequence_Inline(),
        new Example6_Empty_Error(),
        new Example7_Error(),
        new Example8_Newline_Inline(),
        new Example9_Newline_Inline_WithArguments(),
        new Example10_LongSequence_Inline(),
        new Example11_Error(),
        new Example12_LongSequence_Whitespaces(),
        new Example13_NewlineSequence(),
        new Example14_WithInlineArguments(),
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
        new Example30_Error(),
        new Example31_Error(),
        new Example32_Instance_GetSetProperty_WithArgument(),
        new Example33_Instance_GetSetProperty_WithoutArgument(),
        new Example34_Instance_GetProperty(),
        new Example35_Instance_SetProperty(),
        new Example36_Optional_Error(),
        new Example37_Optional_Error(),
        new Example38_OptionalParsing(),
        new Example39_Field(),
        new Example40_Instance_ReadOnlyField(),
        new Example41_ReadOnlyField_Assign_Error()
    ];

    public static readonly HashSet<Type> CorrectInstanceTypes =
    [
        typeof(Example1_Instance_StringArgument),
        typeof(Example2_Instance_StringArgument),
        typeof(Example3_Instance_Sequence_Inline),
        typeof(Example32_Instance_GetSetProperty_WithArgument),
        typeof(Example33_Instance_GetSetProperty_WithoutArgument),
        typeof(Example34_Instance_GetProperty),
        typeof(Example35_Instance_SetProperty),
        typeof(Example40_Instance_ReadOnlyField)
    ];
}