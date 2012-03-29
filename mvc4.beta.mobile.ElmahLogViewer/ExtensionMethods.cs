using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System
{
    public static class ExtensionMethods
    {
        public static T Chain<T>(this T source, Action<T> action)
        {
            action(source);
            return source;
        }
    }
}