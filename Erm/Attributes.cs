using System;
using System.Collections.Generic;
using System.Text;

namespace Erm
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IDFieldAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TableAttribute : Attribute
    {
        public string Name { get; set; }
        public TableAttribute(string name)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DbAttribute : Attribute
    {
        public string ConnectionString { get; set; }
        public DbAttribute(string name)
        {
            ConnectionString = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DbIgnoreAttribute : Attribute
    {

    }

    //used for e.g. function calls
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DbQueryFieldAttribute : Attribute
    {
        private string _function { get; set; }
        private string[] _args { get; set; }
        public DbQueryFieldAttribute(string function, params string[] args)
        {
            _function = function;
            _args = args;
        }

        public override string ToString()
        {
            string function = _function + "(";

            if (_args.Length > 0)
            {
                foreach (var arg in _args)
                {
                    function += arg + ",";
                }

                function = function.Substring(0, function.Length - 1);
            }

            function += ")";

            return function;
        }
    }
}
