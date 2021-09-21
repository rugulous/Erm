using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Erm
{
    public class Parameter
    {
        public Type Type { get; }
        public string Name { get; }
        public object Value { get; }

        private static int _count { get; set; }

        protected Parameter(Type t, string n, object v)
        {
            Type = t;
            Name = n;
            Value = v;
        }

        public static Parameter Create(Type t, object value)
        {
            _count++;
            string name = $"@p{_count}";

            return new Parameter(t, name, value);
        }
    }
}
