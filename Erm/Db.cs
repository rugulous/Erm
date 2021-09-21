using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Erm
{
    public class Db
    {
        protected static Dictionary<Type, DbType> typeMap { get; set; }

        public static Query<T> GetAll<T>() where T : class, new()
        {
            return new SelectQuery<T>();
        }

        public static void Update<T>(T entity) where T : class, new()
        {
            new UpdateQuery<T>().Set(entity).Execute();
        }

        public static bool TryUpdate<T>(T entity) where T : class, new()
        {
            try
            {
                Update(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected static SqlConnection _getConnection(string db)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString);
            con.Open();

            return con;
        }

        public static List<T> RunSelect<T>(string query, string db, List<Parameter> param = null) where T : new()
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            List<T> results = new List<T>();

            SqlConnection con = _getConnection(db);

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = System.Data.CommandType.Text;
                _setParams(param, cmd);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    T entity = _createEntityFromRow<T>(props, rdr);

                    results.Add(entity);
                }

                rdr.Close();
            }

            con.Close();

            return results;
        }

        public static bool RunNonQuery(string query, string db, List<Parameter> param = null)
        {
            int rows = 0;
            SqlConnection con = _getConnection(db);
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.CommandType = CommandType.Text;
                _setParams(param, cmd);

                rows = cmd.ExecuteNonQuery();
            }

            con.Close();

            return (rows > 0);
        }

        private static void _setParams(List<Parameter> param, SqlCommand cmd)
        {
            if (param != null)
            {
                foreach (var p in param)
                {
                    cmd.Parameters.AddWithValue(p.Name, p.Value);
                }
            }
        }

        private static T _createEntityFromRow<T>(PropertyInfo[] props, SqlDataReader rdr) where T : new()
        {
            T entity = new T();
            foreach (PropertyInfo prop in props)
            {
                if (prop.GetCustomAttribute<DbIgnoreAttribute>() != null)
                {
                    continue;
                }

                if (prop.CanWrite)
                {
                    object val = rdr[prop.Name];
                    if(val.GetType() == typeof(DBNull))
                    {
                        val = null;
                    }

                    prop.SetValue(entity, val);
                }
            }

            return entity;
        }

    //    protected static DbType _mapParameterType(object parameter)
    //    {
    //        if (typeMap == null)
    //        {
    //            typeMap = new Dictionary<Type, DbType>();
    //            typeMap[typeof(byte)] = DbType.Byte;
    //            typeMap[typeof(sbyte)] = DbType.SByte;
    //            typeMap[typeof(short)] = DbType.Int16;
    //            typeMap[typeof(ushort)] = DbType.UInt16;
    //            typeMap[typeof(int)] = DbType.Int32;
    //            typeMap[typeof(uint)] = DbType.UInt32;
    //            typeMap[typeof(long)] = DbType.Int64;
    //            typeMap[typeof(ulong)] = DbType.UInt64;
    //            typeMap[typeof(float)] = DbType.Single;
    //            typeMap[typeof(double)] = DbType.Double;
    //            typeMap[typeof(decimal)] = DbType.Decimal;
    //            typeMap[typeof(bool)] = DbType.Boolean;
    //            typeMap[typeof(string)] = DbType.String;
    //            typeMap[typeof(char)] = DbType.StringFixedLength;
    //            typeMap[typeof(Guid)] = DbType.Guid;
    //            typeMap[typeof(DateTime)] = DbType.DateTime;
    //            typeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
    //            typeMap[typeof(byte[])] = DbType.Binary;
    //            typeMap[typeof(byte?)] = DbType.Byte;
    //            typeMap[typeof(sbyte?)] = DbType.SByte;
    //            typeMap[typeof(short?)] = DbType.Int16;
    //            typeMap[typeof(ushort?)] = DbType.UInt16;
    //            typeMap[typeof(int?)] = DbType.Int32;
    //            typeMap[typeof(uint?)] = DbType.UInt32;
    //            typeMap[typeof(long?)] = DbType.Int64;
    //            typeMap[typeof(ulong?)] = DbType.UInt64;
    //            typeMap[typeof(float?)] = DbType.Single;
    //            typeMap[typeof(double?)] = DbType.Double;
    //            typeMap[typeof(decimal?)] = DbType.Decimal;
    //            typeMap[typeof(bool?)] = DbType.Boolean;
    //            typeMap[typeof(char?)] = DbType.StringFixedLength;
    //            typeMap[typeof(Guid?)] = DbType.Guid;
    //            typeMap[typeof(DateTime?)] = DbType.DateTime;
    //            typeMap[typeof(DateTimeOffset?)] = DbType.DateTimeOffset;
    //        }

    //        return typeMap[parameter.GetType()];
    //    }
    }
}
