using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginInterface.PluginInterfaces
{
    public interface IStegoPlugin
    {
        string Name { get; }
        string Description { get; }

        void Activate();
        void Deactivate();
    }
}
