using System;

namespace LeadGen.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    internal sealed class PubSubAttribute : Attribute
    {
        readonly string? _pubSubName;

        public PubSubAttribute(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _pubSubName = name;
            }
        }

        public string? Name => _pubSubName;
    }
}
