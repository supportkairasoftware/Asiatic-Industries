using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using AsiaticIndustries.Core.Models.ViewModel;
using Newtonsoft.Json;

namespace AsiaticIndustries.Core.Infrastructure
{
    public class Common
    {
        public enum SetValue
        {
            CurrentTime = 1, LoggedInUserId
        }

        public enum OrderStatus
        {
            Pending=1,Approved,Dispatched
        }

        public enum InquiryStatus
        {
            Open=1,Closed
        }

        public enum SearchOperator
        {
            EqualTo = 1, NotEqualTo, BeginsWith, EndsWith, Contains, DoesNotContains, GreaterThan, LessThan
        }

        public enum UserRoles
        {
            Admin = 0,
            Representative
        }

        public enum ProductField
        {
            CurrentQuantity = 0, ReorderLevel
        }

        public static PasswordDetail CreatePassword(string password)
        {
            // Pass a logRounds parameter to GenerateSalt to explicitly specify the
            // amount of resources required to check the password. The work factor
            // increases exponentially, so each increment is twice as much work. If
            // omitted, a default of 10 is used.
            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);
            string hashedPasssword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            PasswordDetail passwordDetail = new PasswordDetail();
            passwordDetail.PasswordSalt = salt;
            passwordDetail.Password = hashedPasssword;

            return passwordDetail;
        }

        public static bool PasswordsMatch(string password, string passwordSalt, string oldHashedPassword)
        {
            // Pass a logRounds parameter to GenerateSalt to explicitly specify the
            // amount of resources required to check the password. The work factor
            // increases exponentially, so each increment is twice as much work. If
            // omitted, a default of 10 is used.

            string hashed;
            try
            {
                hashed = BCrypt.Net.BCrypt.HashPassword(password, passwordSalt);
            }
            catch (Exception)
            {
                return false;
            }

            return oldHashedPassword == hashed;
        }

        public static string SerializeObject<T>(T objectData)
        {
            string defaultJson = JsonConvert.SerializeObject(objectData);
            return defaultJson;
        }

        public string SerializePartialObject<T>(T objectData)
        {
            string defaultJson = JsonConvert.SerializeObject(objectData, Formatting.Indented,
                                  new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return defaultJson;
        }

        public static T DeserializeObject<T>(string json)
        {
            T obj = default(T);
            obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    string columnName = column.ColumnName.Replace(" ", "");
                    if (pro.Name == columnName)
                        //pro.SetValue(obj.ToString(), dr[column.ColumnName].ToString(), null);
                        pro.SetValue(obj, Convert.ChangeType(dr[column.ColumnName], pro.PropertyType), null);
                    else
                        continue;
                }
            }
            return obj;
        }

        public static bool ValidateRangeFormat(string value, int field)
        {
            try
            {
                var temp = Convert.ToInt64(value);
                if (field == (int)ProductField.CurrentQuantity)
                {
                    if (temp >= -99999 && temp <= 99999)
                        return true;
                }
                if (field == (int)ProductField.ReorderLevel)
                {
                    if (temp >= 0 && temp <= 99999)
                        return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string MessageWithTitle(string title, string messaage)
        {
            return string.Format("<div><h4>{0}</h4><p>{1}</p></div>", title, messaage);
        }


        public static void TransferObject(object source, object target)
        {
            var sourceType = source.GetType();
            var targetType = target.GetType();

            var sourceParameter = Expression.Parameter(typeof(object), "source");
            var targetParameter = Expression.Parameter(typeof(object), "target");

            var sourceVariable = Expression.Variable(sourceType, "castedSource");
            var targetVariable = Expression.Variable(targetType, "castedTarget");

            var expressions = new List<Expression>();

            expressions.Add(Expression.Assign(sourceVariable, Expression.Convert(sourceParameter, sourceType)));
            expressions.Add(Expression.Assign(targetVariable, Expression.Convert(targetParameter, targetType)));

            foreach (var property in sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanRead)
                    continue;

                var targetProperty = targetType.GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                if (targetProperty != null
                        && targetProperty.CanWrite
                        && targetProperty.PropertyType.IsAssignableFrom(property.PropertyType))
                {
                    expressions.Add(
                        Expression.Assign(
                            Expression.Property(targetVariable, targetProperty),
                            Expression.Convert(
                                Expression.Property(sourceVariable, property), targetProperty.PropertyType)));
                }
            }

            var lambda =
                Expression.Lambda<Action<object, object>>(
                    Expression.Block(new[] { sourceVariable, targetVariable }, expressions),
                    new[] { sourceParameter, targetParameter });

            var del = lambda.Compile();

            del(source, target);
        }
    }
}
