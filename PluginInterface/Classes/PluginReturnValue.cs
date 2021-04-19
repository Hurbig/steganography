using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginInterface.Attributes;

namespace PluginInterface.Classes
{
    public class PluginReturnValue
    {
        public object Value { get; private set; }
        public ReturnTypes ReturnType { get; private set; }

        public PluginReturnValue(object value, ReturnTypes returnType)
        {
            Value = value;
            ReturnType = returnType;
        }
    }
}
