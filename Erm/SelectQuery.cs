using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Erm
{
    class SelectQuery<T> : Query<T> where T : new()
    {
        public override List<T> Execute()
        {
            string query = _getFullQuery();

            return Db.RunSelect<T>(query, _db, _params);
        }

        protected override string _generateQueryPartial()
        {
            string query = "SELECT ";
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                if (prop.GetCustomAttribute<DbIgnoreAttribute>() != null)
                {
                    continue;
                }

                if (prop.CanWrite)
                {
                    DbQueryFieldAttribute attr = prop.GetCustomAttribute<DbQueryFieldAttribute>();
                    if (attr != null)
                    {
                        query += $"{attr} AS {prop.Name}";
                    } else
                    {
                    query += prop.Name;

                    }

                    query += ", ";
                }
            }

            query = query.Substring(0, query.Length - 2);

            query += $" FROM {_table}";

            return query;
        }


    }
}
