using System;
using System.Linq.Expressions;

namespace Erm
{
    internal class LINQTranslator
    {
        internal static string TranslateMember(Expression node)
        {
            MemberExpression member = (MemberExpression)node;
            //Console.WriteLine(member.Member.Name);
            return member.Member.Name;
        }

        internal static Parameter TranslateConstant(Expression node)
        {
            ConstantExpression constant = (ConstantExpression)node;
            Parameter p = Parameter.Create(constant.Type, constant.Value);
            return p;
        }

        internal static string TranslateMethod(MethodCallExpression call)
        {
            if(call.Method.Name == "Substring")
            {
                return Substring(call);
            }

            throw new NotSupportedException("Wtf is that method dude?");
        }

        internal static string Substring(MethodCallExpression call)
        {
            string value = $"SUBSTRING({TranslateMember(call.Object)}, {((int)TranslateConstant(call.Arguments[0]).Value) + 1}";

            if (call.Arguments.Count > 1)
            {
                value += $", {((int)TranslateConstant(call.Arguments[1]).Value) + 1})";
            } else
            {
                value += $", LEN({TranslateMember(call.Object)}))";
            }

            return value;
        }
    }
}