using RefinedShell;
using RefinedShell.Example;
using RefinedShell.Execution;
using RefinedShell.Utilities;

/*
add 2147483647 + 1
    [False]->UnknownToken: '2147483647'
add 2147483647 1
    [True]->-2147483648
    */

int[] list =
[
    0, 1, 2, 3, 4, 5, 6
];



return;

Shell shell = new Shell();
CommandCollection commandCollection = new CommandCollection(shell);
commandCollection.RegisterCommands();
Console.WriteLine($"Available commands: {shell.Count}");
shell.Register(Console.Clear, "clear");
shell.CreateAlias("ch", "clear; help");

while(true)
{
    string? input = Console.ReadLine();
    if(input == null)
    {
        continue;
    }
    ExecutionResult result = shell.Execute(input);
    switch (result.ErrorType)
    {
        case ExecutionError.None:
            if(result.ReturnValue == null)
                Console.WriteLine($"[{result.IsSuccess}]");
            else
                Console.WriteLine($"[{result.IsSuccess}]->{result.ReturnValue}");
            break;
        case ExecutionError.CommandNotFound:
            Console.WriteLine($"[{result.IsSuccess}]->Command '{input.Substring(result.Segment)}' not found");
            break;
        case ExecutionError.UnknownToken:
        case ExecutionError.InvalidUsageOfToken:
        case ExecutionError.UnexpectedToken:
        case ExecutionError.InsufficientArguments:
        case ExecutionError.TooManyArguments:
        case ExecutionError.InvalidArgumentType:
            Console.WriteLine($"[{result.IsSuccess}]->{result.ErrorType}: '{input.Substring(result.Segment)}'");
            break;
        case ExecutionError.CommandHasNoReturnResult:
            Console.WriteLine($"[{result.IsSuccess}]->Command '{input.Substring(result.Segment)}' has no return result");
            break;
        case ExecutionError.CommandNotValid:
            Console.WriteLine($"[{result.IsSuccess}]->Command '{input.Substring(result.Segment)}' is not valid");
            break;
        case ExecutionError.ArgumentError:
            Console.WriteLine("TODO: ArgumentError");
            break;
        case ExecutionError.Exception:
            Exception exception = (Exception)result.ReturnValue!;
            Console.WriteLine($"[{result.IsSuccess}]->{exception.GetType().Name}: {exception.Message}");
            break;
        default:
            throw new ArgumentOutOfRangeException();
    }
}