using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface.Attributes
{
    public enum ReturnTypes
    {
        Text,
        Image,
        UserControl,
        List
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class ReturnType : Attribute
    {
        private readonly ReturnTypes _ReturnType;

        public ReturnType(ReturnTypes returnType)
        {
            _ReturnType = returnType;
        }

        public ReturnTypes GetReturntype()
        {
            return _ReturnType;
        }
    }
}
