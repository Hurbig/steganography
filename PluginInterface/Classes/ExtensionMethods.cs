using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PluginInterface.Attributes;

namespace PluginInterface.Classes
{
    public static class ExtensionMethods
    {
        public static UIElement GetUiComponent(this PluginReturnValue value)
        {
            switch(value.ReturnType)
            {
                case ReturnTypes.Text:
                    return new TextBlock() { Text = value.Value.ToString(), TextWrapping = TextWrapping.Wrap };
                case ReturnTypes.List:
                    return new ItemsControl() { ItemsSource = (ICollection) value.Value };
                case ReturnTypes.UserControl:
                    return value.Value as UserControl;
                case ReturnTypes.Image:
                    return new Image() {Source = value.Value as ImageSource};
                    
                default:
                    throw new NotImplementedException();
            }
        }

        public static MemberInfo GetMember<T, R>(this T instance, Expression<Func<T, R>> selector)
        {
            var member = selector.Body as MemberExpression;
            return member?.Member;
        }

        public static MethodInfo GetMethod<T>(this T instance, Expression<Action<T>> selector)
        {
            var method = selector.Body as MethodCallExpression;
            return method?.Method;
        }

        public static MethodInfo GetMethod<T, R>(this T instance, Expression<Func<T, R>> selector)
        {
            var method = selector.Body as MethodCallExpression;
            return method?.Method;
        }

        public static T GetAttribute<T>(this MemberInfo meminfo) where T : Attribute
        {
            return meminfo.GetCustomAttributes(typeof(T)).FirstOrDefault() as T;
        }
    }
}
