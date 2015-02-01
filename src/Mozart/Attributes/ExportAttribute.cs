using System;
using Mozart.Model;

namespace Mozart.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class ExportAttribute : Attribute
    {
        public bool Rewritable { get; private set; }
        public InstanceRule InstanceRule { get; private set; }        
        public Type Type { get; set; }

        public ExportAttribute()
        {
            
        }
        public ExportAttribute(bool rewritable = false, InstanceRule instanceRule = InstanceRule.Multiple)
        {
            InstanceRule = instanceRule;
            Rewritable = rewritable;
        }

        public ExportAttribute(Type type, bool rewritable = false, InstanceRule instanceRule = InstanceRule.Multiple):this(rewritable,instanceRule)
        {
            if (!type.IsInterface)
                throw new Exception("You can only export interfaces");
            InstanceRule = instanceRule;
            //Rewritable = rewritable;
            //Type = type;
        }

    }
}
