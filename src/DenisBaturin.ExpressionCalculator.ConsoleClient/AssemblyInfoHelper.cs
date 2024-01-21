namespace DenisBaturin.ExpressionCalculator.ConsoleClient
{
    using System;
    using System.Reflection;

    public class AssemblyInfoHelper
    {
        private readonly Assembly _assembly;

        public AssemblyInfoHelper(Assembly assembly)
        {
            _assembly = assembly;
        }

        public string Name
            => _assembly.GetName().Name;

        public string Version
            => _assembly.GetName().Version.ToString();

        public string Description
            => ((AssemblyDescriptionAttribute)GetCustomAttribute(typeof(AssemblyDescriptionAttribute))).Description;

        private Attribute GetCustomAttribute(Type type)
        {
            var customAttributes = _assembly.GetCustomAttributes(type, false);
            var customAttribute = (Attribute)customAttributes[0];
            return customAttribute;
        }
    }
}