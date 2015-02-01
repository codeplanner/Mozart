using System;

namespace Mozart.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class NoExportAttribute : Attribute{}
}