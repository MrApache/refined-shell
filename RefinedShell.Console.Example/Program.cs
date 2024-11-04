using RefinedShell;
using RefinedShell.Example;
using RefinedShell.Execution;

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
    switch (result.Error)
    {
        case ExecutionError.None:
            if(result.ReturnValue == null)
                Console.WriteLine($"[{result.Success}]");
            else
                Console.WriteLine($"[{result.Success}]->{result.ReturnValue}");
            break;
        case ExecutionError.CommandNotFound:
            Console.WriteLine("[F]->Command not found");
            break;
        case ExecutionError.UnknownToken:
        case ExecutionError.InvalidUsageOfToken:
        case ExecutionError.UnexpectedToken:
        case ExecutionError.InsufficientArguments:
        case ExecutionError.TooManyArguments:
        case ExecutionError.InvalidArgumentType:
            Console.WriteLine($"[{result.Success}]->{result.Error}: '{input.Substring(result.Segment.Start, result.Segment.Length)}'");
            break;
        case ExecutionError.CommandHasNoReturnResult:
            break;
        case ExecutionError.CommandNotValid:
            break;
        case ExecutionError.ArgumentError:
            break;
        case ExecutionError.UnhandledException:
            break;
        default:
            throw new ArgumentOutOfRangeException();
    }
}