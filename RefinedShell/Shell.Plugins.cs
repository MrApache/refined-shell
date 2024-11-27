using System;
using System.Collections.Generic;
using RefinedShell.Commands;
using RefinedShell.test;
using RefinedShell.Utilities;

namespace RefinedShell
{
    public sealed partial class Shell
    {
        private readonly PluginContext _pluginContext;
        private readonly List<IPlugin> _plugins;
 
        public void AttachPlugin(IPlugin plugin)
        {
            if(_plugins.Contains(plugin))
                return;

            _plugins.Add(plugin);
            InvokeAttachEvent(plugin);
        }

        private void InvokeAttachEvent(IPlugin newPlugin)
        {
            newPlugin.OnAttach(_pluginContext);
        }

        private void InvokeOnCommandRegisteredEvent(ICommand command)
        {
            PluginContext.Lifetime lifetime = new PluginContext.Lifetime(command);
            foreach(IPlugin plugin in _plugins) {
                plugin.OnCommandRegistered(lifetime);
            }
        }

        private void InvokeOnCommandUnregisteredEvent(ICommand command)
        {
            PluginContext.Lifetime lifetime = new PluginContext.Lifetime(command);
            foreach(IPlugin plugin in _plugins) {
                plugin.OnCommandUnregistered(lifetime);
            }
        }

        private void InvokeBeforeExecutionEvent(ReadOnlySpan<IReadOnlyCommand> commands, StringToken input)
        {
            PluginContext.Execution execution = new PluginContext.Execution(commands, input);
            foreach(IPlugin plugin in _plugins) {
                plugin.BeforeCommandExecution(execution);
            }
        }

        private void InvokeAfterExecutionEvent(ReadOnlySpan<IReadOnlyCommand> commands, StringToken input, ExecutionResult result)
        {
            PluginContext.Execution execution = new PluginContext.Execution(commands, input, result);
            foreach(IPlugin plugin in _plugins) {
                plugin.AfterCommandExecution(execution);
            }
        }
    }
}
