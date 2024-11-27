namespace RefinedShell.test
{
    public interface IPlugin
    {
        public void OnAttach(PluginContext context);
        public void OnCommandRegistered(in PluginContext.Lifetime context);
        public void OnCommandUnregistered(in PluginContext.Lifetime context);
        public void BeforeCommandExecution(in PluginContext.Execution context);
        public void AfterCommandExecution(in PluginContext.Execution context);
    }
}
