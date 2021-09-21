using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Erm
{
    class UpdateQuery<T> : Query<T> where T : class, new()
    {
        protected Dictionary<string, object> _fields { get; set; }

        public UpdateQuery(){
            _fields = new Dictionary<string, object>();
        }

        public UpdateQuery<T> Set(T entity)
        {
       
            if(_fields.Count > 0)
            {
                throw new Exception("Cannot re-set entity!");
            }

            _parseFields(entity);

            return this;
        }

        protected void _parseFields(T entity)
        {
            bool foundID = false;
            string IDName = null;
            string IDValue = null;
            Type IDType = null;

            Type t = typeof(T);
            PropertyInfo[] props = t.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                IDFieldAttribute attr = prop.GetCustomAttribute<IDFieldAttribute>();
                if(attr != null)
                {
                    IDValue = prop.GetValue(entity).ToString();
                    IDName = prop.Name;
                    IDType = prop.PropertyType;
                    foundID = true;
                } 

                if(prop.Name.ToLower() == "id")
                {
                    IDValue = prop.GetValue(entity).ToString();
                    IDName = prop.Name;
                    IDType = prop.PropertyType;
                    foundID = true;
                }

                DbIgnoreAttribute dbIgnore = prop.GetCustomAttribute<DbIgnoreAttribute>();
                if(dbIgnore == null && prop.CanRead)
                {
                    object val = prop.GetValue(entity);
                    object value = null;
                    if(val != null)
                    {
                        value = val;
                    }

                    _fields.Add(prop.Name, value);
                }
            }

            if (!foundID)
            {
                throw new Exception("Unable to determine ID field for type " + t.FullName);
            }

            Parameter p = Parameter.Create(IDType, IDValue);
            _params.Add(p);
            _where = $"{IDName} = {p.Name}";
        }

        protected override string _generateQueryPartial()
        {
            string query = $"UPDATE {_table} SET ";

            foreach (var field in _fields)
            {
                if (field.Value == null)
                {
                    query += $"{field.Key} = NULL, ";
                }
                else
                {
                    Parameter p = Parameter.Create(field.Value.GetType(), field.Value);
                    query += $"{field.Key} = {p.Name}, ";
                    _params.Add(p);
                }
            }

            query = query.Substring(0, query.Length - 2);

            return query;
        }

        public override List<T> Execute()
        {
            string query = _getFullQuery();
            Console.WriteLine(query);

            Db.RunNonQuery(query, _db, _params);

            return new List<T>();
        }
    }
}
