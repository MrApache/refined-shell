using System;
using System.Reflection;
using RefinedShell.Utilities;

namespace RefinedShell.Commands
{
    internal abstract class Property : MemberCommand
    {
        public override Arguments Arguments => _arguments;
        public override bool ReturnsResult => _returnResult;

        private readonly bool _returnResult;
        private readonly Arguments _arguments;

        protected Property(StringToken name, PropertyInfo p, object? target)
            : base(name, p, target)
        {
            _returnResult = p.GetMethod != null;
            if (p.SetMethod != null)
            {
                bool optional = p.GetMethod != null;
                ParameterInfo[] parameters = p.SetMethod.GetParameters();
                Argument[] arguments = new Argument[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    ParameterInfo parameter = parameters[i];
                    arguments[i] = new Argument(parameter.ParameterType, parameter.Name, optional);
                }

                _arguments = new Arguments(arguments);
            }
            else
            {
                _arguments = new Arguments(Array.Empty<Argument>());
            }
        }
    }
}