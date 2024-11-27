using System.Collections.Generic;
using RefinedShell.test;
using RefinedShell.Commands;
using RefinedShell.Utilities;

namespace RefinedShell.Tests.Plugins;

internal sealed class EventListener : IPlugin
{
    private int _attachCount;
    private Shell _attachedShell = null!;
    private bool _isAttached;
    private readonly List<IReadOnlyCommand> _registeredCommands;
    private StringToken _string;
    private readonly List<IReadOnlyCommand> _commands;
    private ExecutionResult _result;

    public int AttachCount => _attachCount;
    public bool IsAttached => _isAttached;
    public Shell AttachedShell => _attachedShell;
    public List<IReadOnlyCommand> RegisteredCommands => _registeredCommands;
    public StringToken Input => _string;
    public List<IReadOnlyCommand> Commands => _commands;
    public ExecutionResult Result => _result;

    public EventListener()
    {
        _registeredCommands = [];
        _commands = [];
    }

    public void OnAttach(PluginContext context)
    {
        _attachedShell = context.Shell;
        _isAttached = true;
        _attachCount++;
    }

    public void OnCommandRegistered(in PluginContext.Lifetime context)
    {
        _registeredCommands.Add(context.Command);
    }

    public void OnCommandUnregistered(in PluginContext.Lifetime context)
    {
        _registeredCommands.Remove(context.Command);
    }

    public void BeforeCommandExecution(in PluginContext.Execution context)
    {
        _commands.Clear();
        _string = context.Input;
        _commands.AddRange(context.Command);
    }

    public void AfterCommandExecution(in PluginContext.Execution context)
    {
        _result = context.Result!.Value;
    }
}
