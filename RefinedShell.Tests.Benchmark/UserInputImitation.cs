using BenchmarkDotNet.Attributes;

namespace RefinedShell.Tests.Benchmark;

[MemoryDiagnoser]
public class UserInputImitation
{
    private readonly string[] _input =
    [
        "",
        "c",
        "co",
        "com",
        "comm",
        "comma",
        "comman",
        "command",
        "command ",
        "command a",
        "command ar",
        "command arg",
        "command arg1",
        "command arg1 ",
        "command arg1 a",
        "command arg1 ar",
        "command arg1 arg",
        "command arg1 arg2",
        "command arg1 arg2;",
        "command arg1 arg2; ",
        "command arg1 arg2; t",
        "command arg1 arg2; te",
        "command arg1 arg2; tel",
        "command arg1 arg2; tele",
        "command arg1 arg2; telep",
        "command arg1 arg2; telepo",
        "command arg1 arg2; telepor",
        "command arg1 arg2; teleport",
        "command arg1 arg2; teleport ",
        "command arg1 arg2; teleport $",
        "command arg1 arg2; teleport $(",
        "command arg1 arg2; teleport $(g",
        "command arg1 arg2; teleport $(ge",
        "command arg1 arg2; teleport $(get",
        "command arg1 arg2; teleport $(getp",
        "command arg1 arg2; teleport $(getpl",
        "command arg1 arg2; teleport $(getpla",
        "command arg1 arg2; teleport $(getplay",
        "command arg1 arg2; teleport $(getplaye",
        "command arg1 arg2; teleport $(getplayer",
        "command arg1 arg2; teleport $(getplayerp",
        "command arg1 arg2; teleport $(getplayerpo",
        "command arg1 arg2; teleport $(getplayerpos",
        "command arg1 arg2; teleport $(getplayerpos)"
    ];

    private readonly Shell _shell;

    public UserInputImitation()
    {
        _shell = new Shell();
        RegisterCommands(_shell);
    }

    [Benchmark]
    public void Imitation()
    {
        for (int i = 0; i < _input.Length; i++) {
            _shell.Analyze(_input[i]);
        }

        _shell.Execute(_input[^1]);
    }

    private void RegisterCommands(Shell shell)
    {
        shell.Register(Command, "command");
        shell.Register(Teleport, "teleport");
        shell.Register(GetPlayerPos, "getplayerpos");
    }

    private void Command(string arg1, string arg2) { }
    private void Teleport(short position) { }
    private short GetPlayerPos() { return (short)Random.Shared.Next(); }

    //Before optimizations
    //| Method    | Mean     | Error   | StdDev  | Gen0    | Allocated |
    //|---------- |---------:|--------:|--------:|--------:|----------:|
    //| Imitation | 254.6 us | 1.31 us | 1.16 us | 21.9727 | 137.23 KB |

    //With semantic memory optimization #1
    //Replace array allocations with ArrayPool<string>.Shared
    //| Method    | Mean     | Error   | StdDev  | Gen0    | Allocated |
    //|---------- |---------:|--------:|--------:|--------:|----------:|
    //| Imitation | 264.5 us | 4.84 us | 5.94 us | 21.9727 | 135.17 KB |
    
    //With semantic memory optimizations #2
    //Reduced ArrayPool<string>.Shared.Rent calls from 2 to 1 #2
    //| Method    | Mean     | Error   | StdDev  | Gen0    | Allocated |
    //|---------- |---------:|--------:|--------:|--------:|----------:|
    //| Imitation | 258.6 us | 1.29 us | 1.08 us | 21.9727 | 135.17 KB |

    //Used custom regex
    //| Method    | Mean     | Error   | StdDev  | Gen0   | Allocated |
    //|---------- |---------:|--------:|--------:|-------:|----------:|
    //| Imitation | 204.8 us | 1.25 us | 1.04 us | 4.8828 |  30.32 KB |
}