using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Utils.DatabaseHelper
{
    /// <summary>
    /// 数据库操作条件的定义类。
    /// </summary>
    public struct Condition
    {
        /// <summary>
        /// 字段名称。
        /// </summary>
        public string FieldName;
        /// <summary>
        /// 字段值。
        /// </summary>
        public string FieldValue;
        /// <summary>
        /// 逻辑运算符。
        /// </summary>
        public DatabaseLogicOperator FieldLogicOperator;
        /// <summary>
        /// 比较运算符。
        /// </summary>
        public DatabaseCompareOperator FieldCompareOperator;
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="fieldLogicOperator">逻辑运算符。</param>
        /// <param name="fieldName">字段名称。</param>
        /// <param name="fieldCompareOperator">比较运算符。</param>
        /// <param name="fieldValue">字段值。</param>
        public Condition(DatabaseLogicOperator fieldLogicOperator, string fieldName, 
            DatabaseCompareOperator fieldCompareOperator, string fieldValue)
        {
            this.FieldLogicOperator = fieldLogicOperator;
            this.FieldName = fieldName;
            this.FieldCompareOperator = fieldCompareOperator;
            this.FieldValue = fieldValue;
        }
        /// <summary>
        /// 数据库操作条件字符串。
        /// </summary>
        /// <returns>返回SQL条件字符串。</returns>
        private string ConditionString()
        {
            StringBuilder sb = new StringBuilder();
            //判断逻辑运算符。
            switch (FieldLogicOperator)
            {
                case DatabaseLogicOperator.Not:
                    sb.AppendFormat(" {0}", "NOT");
                    break;
                case DatabaseLogicOperator.And:
                    sb.AppendFormat(" {0}", "AND");
                    break;
                case DatabaseLogicOperator.Or:
                    sb.AppendFormat(" {0}", "OR");
                    break;
                default:
                    sb.AppendFormat(" {0}", "AND");
                    break;
            }

            sb.AppendFormat(" {0}", FieldName);
            //判断比较字符串。
            switch (FieldCompareOperator)
            {
                case DatabaseCompareOperator.Equal:
                    sb.AppendFormat(" = {0}", FieldValue);
                    break;
                case DatabaseCompareOperator.NotEqual:
                    sb.AppendFormat(" != {0}", FieldValue);
                    break;
                case DatabaseCompareOperator.LessThan:
                    sb.AppendFormat(" < {0}", FieldValue);
                    break;
                case DatabaseCompareOperator.LessThanEqual:
                    sb.AppendFormat(" <= {0}", FieldValue);
                    break;
                case DatabaseCompareOperator.GreaterThan:
                    sb.AppendFormat(" > {0}", FieldValue);
                    break;
                case DatabaseCompareOperator.GreaterThanEqual:
                    sb.AppendFormat(" >= {0}", FieldValue);
                    break;
                case DatabaseCompareOperator.In:
                    sb.AppendFormat(" IN ({0})", FieldValue);
                    break;
                case DatabaseCompareOperator.NotIn:
                    sb.AppendFormat(" NOT IN ({0})", FieldValue);
                    break;
                case DatabaseCompareOperator.Between:
                    sb.AppendFormat(" BETWEEN {0}", FieldValue);
                    break;
                case DatabaseCompareOperator.Like:
                    sb.AppendFormat(" LIKE {0}", FieldValue);
                    break;
                case DatabaseCompareOperator.Null:
                    sb.Append(" IS NULL");
                    break;
                case DatabaseCompareOperator.NotNull:
                    sb.Append(" IS NOT NULL");
                    break;
                default:
                    sb.AppendFormat(" = {0}", FieldValue);
                    break;
            }

            return sb.ToString();
        }
        /// <summary>
        /// 返回表示数据库操作条件的字符串。
        /// </summary>
        /// <returns>数据库操作条件的字符串。</returns>
        public override string ToString()
        {
            return ConditionString();
        }
    }

    /// <summary>
    /// 数据库操作条件的集合类。
    /// </summary>
    public class Conditions
    {
        private List<Condition> conditions = new List<Condition>();
        /// <summary>
        /// 数据库操作条件集合。
        /// </summary>
        public List<Condition> ConditionList
        {
            get
            {
                return conditions;
            }
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public Conditions()
        {

        }
        /// <summary>
        /// 添加数据库操作条件。
        /// </summary>
        /// <param name="fieldLogicOperator">逻辑运算符。</param>
        /// <param name="fieldName">字段名。</param>
        /// <param name="fieldCompareOperator">比较运算符。</param>
        /// <param name="fieldValue">字段值。</param>
        public void Add(DatabaseLogicOperator fieldLogicOperator, string fieldName, 
            DatabaseCompareOperator fieldCompareOperator, string fieldValue)
        {
            conditions.Add(new Condition(fieldLogicOperator, fieldName, fieldCompareOperator, fieldValue));
        }
        /// <summary>
        /// 添加数据库操作条件。
        /// </summary>
        /// <param name="condition">数据库操作条件定义对象。</param>
        public void Add(Condition condition)
        {
            conditions.Add(condition);
        }
        /// <summary>
        /// 移除数据库操作条件。
        /// </summary>
        /// <param name="fieldLogicOperator">逻辑运算符。</param>
        /// <param name="fieldName">字段名。</param>
        /// <param name="fieldCompareOperator">比较运算符。</param>
        /// <param name="fieldValue">字段值。</param>
        /// <returns>true:成功移除，false:失败。</returns>
        public bool Remove(DatabaseLogicOperator fieldLogicOperator, string fieldName,
            DatabaseCompareOperator fieldCompareOperator, string fieldValue)
        {
            return conditions.Remove(new Condition(fieldLogicOperator, fieldName, fieldCompareOperator, fieldValue));
        }
        /// <summary>
        /// 移除数据库操作条件。
        /// </summary>
        /// <param name="condition">数据库操作条件定义对象。</param>
        /// <returns>true:成功移除，false:失败。</returns>
        public bool Remove(Condition condition)
        {
            return conditions.Remove(condition);
        }
        /// <summary>
        /// 移除所有数据库操作条件。
        /// </summary>
        public void RemoveAll()
        {
            conditions.RemoveAll(ConditionMatch);
        }
        /// <summary>
        /// 数据库操作条件匹配比较。
        /// </summary>
        /// <param name="s">数据库操作条件定义对象。</param>
        /// <returns>始终返回true.</returns>
        private bool ConditionMatch(Condition s)
        {
            return true;
        }
    }
}
