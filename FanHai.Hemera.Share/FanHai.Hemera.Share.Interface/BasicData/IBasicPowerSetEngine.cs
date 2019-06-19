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
    public interface IBasicPowerSetEngine
    {
        /// <summary>
        /// 获得档位设置的数据
        /// </summary>
        /// <returns></returns>
        DataSet GetPowerSetData(Hashtable hstable);
        /// <summary>
        /// 保存分档数据
        /// </summary>
        /// <param name="dsPowerSet"></param>
        /// <returns></returns>
        DataSet SavePowerSetData(DataSet dsPowerSet);
        /// <summary>
        /// 获得基础数据信息
        /// </summary>
        /// <param name="strFilter"></param>
        /// <returns></returns>
        DataSet GetBasicPowerSetEngine_CommonData(string strFilter);
        /// <summary>
        /// 判断分档规则是否重复
        /// </summary>
        /// <param name="dtInsertPowerSetData"></param>
        /// <returns></returns>
        DataSet IsExistPowerSetData(DataTable dtInsertPowerSetData);
        /// <summary>
        /// 判断子分档规则是否重复
        /// </summary>
        /// <param name="dtInsertPowerDtlData"></param>
        /// <returns></returns>
        DataSet IsExistPowerDtlData(DataTable dtInsertPowerDtlData);
        /// <summary>
        /// 获得包装数据中的分档数据
        /// </summary>
        /// <param name="hstable"></param>
        /// <returns></returns>
        DataSet GetPowerSetDtl(Hashtable hstable);

        DataSet GetPowerLevelByLotNum(Hashtable hsParams);
        DataSet IsExistPowerDtlColorData(DataTable dtInsertPowerDtlColorData);
    }
}
