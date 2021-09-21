using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Erm
{
    //represents the main query object
    public abstract class Query<T> where T : new()
    {
        protected string _db { get; set; }
        protected string _table { get; set; }
        protected string _where { get; set; }

        protected List<Parameter> _params { get; set; }

        public Query()
        {
            _params = new List<Parameter>();
            _where = "";
            _table = _getTableName();

            Type t = typeof(T);
            DbAttribute attr = t.GetCustomAttribute<DbAttribute>();
            if (attr == null)
            {
                throw new Exception($"{t.FullName} does not have a database set");
            }

            _db = attr.ConnectionString;
        }

        protected string _getTableName()
        {
            Type t = typeof(T);
            TableAttribute tbl = t.GetCustomAttribute<TableAttribute>();

            if (tbl != null)
            {
                return tbl.Name;
            }

            return t.Name;
        }

        public Query<T> Where(Expression<Func<T, bool>> predicate)
        {
            while (predicate.CanReduce)
            {
                predicate = (Expression<Func<T, bool>>)predicate.Reduce();
            }

            BinaryExpression be = (BinaryExpression)predicate.Body;
            _where += _visitNode(be);

            return this;
        }


        protected string _visitNode(Expression node)
        {

            if (node.NodeType == ExpressionType.MemberAccess)
            {
                return LINQTranslator.TranslateMember(node);
            }
            else if (node.NodeType == ExpressionType.Constant)
            {
                Parameter p = LINQTranslator.TranslateConstant(node);
                _params.Add(p);
                return p.Name;
                //return constant.Value.ToString();
            } else if(node.NodeType == ExpressionType.Call)
            {
                MethodCallExpression call = (MethodCallExpression)node;
                return LINQTranslator.TranslateMethod(call);
            }

            BinaryExpression bin = (BinaryExpression)node;
            string query = "(";

            //visit current node
            query += _visitNode(bin.Left);
            query += " ";
            //bin.NodeType.ToString();

            switch (bin.NodeType)
            {
                case ExpressionType.Add:
                case ExpressionType.AddAssign:
                case ExpressionType.AddAssignChecked:
                case ExpressionType.AddChecked:
                    query += "+";
                    break;

                case ExpressionType.And:
                case ExpressionType.AndAlso:
                case ExpressionType.AndAssign:
                    query += "AND";
                    break;
                case ExpressionType.ArrayIndex:
                    break;
                case ExpressionType.ArrayLength:
                    break;
                case ExpressionType.Assign:
                    break;
                case ExpressionType.Block:
                    break;
                case ExpressionType.Call:
                    break;
                case ExpressionType.Coalesce:
                    break;
                case ExpressionType.Conditional:
                    break;
                case ExpressionType.Constant:
                    break;
                case ExpressionType.Convert:
                    break;
                case ExpressionType.ConvertChecked:
                    break;
                case ExpressionType.DebugInfo:
                    break;
                case ExpressionType.Decrement:
                    break;
                case ExpressionType.Default:
                    break;
                case ExpressionType.Divide:
                    query += "/";
                    break;
                case ExpressionType.DivideAssign:
                    break;
                case ExpressionType.Dynamic:
                    break;
                case ExpressionType.Equal:
                    query += "=";
                    break;

                case ExpressionType.Or:
                case ExpressionType.OrAssign:
                case ExpressionType.OrElse:
                case ExpressionType.ExclusiveOr:
                case ExpressionType.ExclusiveOrAssign:
                    query += "OR";
                    break;
                case ExpressionType.Extension:
                    break;
                case ExpressionType.Goto:
                    break;
                case ExpressionType.GreaterThan:
                    query += ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    query += ">=";
                    break;
                case ExpressionType.Increment:
                    break;
                case ExpressionType.Index:
                    break;
                case ExpressionType.Invoke:
                    break;
                case ExpressionType.IsFalse:
                    break;
                case ExpressionType.IsTrue:
                    break;
                case ExpressionType.Label:
                    break;
                case ExpressionType.Lambda:
                    break;
                case ExpressionType.LeftShift:
                    break;
                case ExpressionType.LeftShiftAssign:
                    break;
                case ExpressionType.LessThan:
                    query += "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    query += "<=";
                    break;
                case ExpressionType.ListInit:
                    break;
                case ExpressionType.Loop:
                    break;
                case ExpressionType.MemberAccess:
                    break;
                case ExpressionType.MemberInit:
                    break;
                case ExpressionType.Modulo:
                    break;
                case ExpressionType.ModuloAssign:
                    break;
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyAssign:
                case ExpressionType.MultiplyAssignChecked:
                case ExpressionType.MultiplyChecked:
                    query += "*";
                    break;


                case ExpressionType.Negate:
                    break;
                case ExpressionType.NegateChecked:
                    break;
                case ExpressionType.New:
                    break;
                case ExpressionType.NewArrayBounds:
                    break;
                case ExpressionType.NewArrayInit:
                    break;
                case ExpressionType.Not:
                case ExpressionType.NotEqual:
                    query += "<>";
                    break;

                case ExpressionType.OnesComplement:
                    break;

                case ExpressionType.Parameter:
                    break;
                case ExpressionType.PostDecrementAssign:
                    break;
                case ExpressionType.PostIncrementAssign:
                    break;
                case ExpressionType.Power:
                    break;
                case ExpressionType.PowerAssign:
                    break;
                case ExpressionType.PreDecrementAssign:
                    break;
                case ExpressionType.PreIncrementAssign:
                    break;
                case ExpressionType.Quote:
                    break;
                case ExpressionType.RightShift:
                    break;
                case ExpressionType.RightShiftAssign:
                    break;
                case ExpressionType.RuntimeVariables:
                    break;
                case ExpressionType.Subtract:
                case ExpressionType.SubtractAssign:
                case ExpressionType.SubtractAssignChecked:
                case ExpressionType.SubtractChecked:
                    query += "-";
                    break;
                case ExpressionType.Switch:
                    break;
                case ExpressionType.Throw:
                    break;
                case ExpressionType.Try:
                    break;
                case ExpressionType.TypeAs:
                    break;
                case ExpressionType.TypeEqual:
                    break;
                case ExpressionType.TypeIs:
                    break;
                case ExpressionType.UnaryPlus:
                    break;
                case ExpressionType.Unbox:
                    break;
                default:
                    break;
            }

            query += " ";
            query += _visitNode(bin.Right);

            query += ")";

            return query;
        }

        protected string _getFullQuery()
        {
            string query = _generateQueryPartial();

            if (!string.IsNullOrEmpty(_where))
            {
                query += " WHERE " + _where;
            }

            return query;
        }

        public abstract List<T> Execute();

        protected abstract string _generateQueryPartial();

        public override string ToString()
        {
            string query = _getFullQuery();
            foreach (var param in _params)
            {
                query = query.Replace(param.Name, param.Value.ToString());
            }

            return query;
        }

    }
}
