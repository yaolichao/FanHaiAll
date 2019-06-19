
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 数据库操作接口。
    /// </summary>
    public interface IBasicTestRuleEngine
    {
        /// <summary>
        /// 获得测试规则主要信息
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        /// owner genchille.yang 2012-10-31 10:00:01
         DataSet GetTestRuleMainData(Hashtable hstable);
        /// <summary>
        /// 获得测试规则明细信息
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        /// owner genchille.yang 2012-10-31 10:05:27
         DataSet GetTestRuleDeatilData(Hashtable hstable);
        /// <summary>
        /// 保存测试规则相关数据
        /// </summary>
        /// <param name="dsSave"></param>
        /// <returns></returns>
         DataSet SaveTestRuleAllData(DataSet dsSave);
        /// <summary>
        /// 获得打印数据
        /// </summary>
        /// <returns></returns>
         DataSet GetPrintData();
 
    }
}
