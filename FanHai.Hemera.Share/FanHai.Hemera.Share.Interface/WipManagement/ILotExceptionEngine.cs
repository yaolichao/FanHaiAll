using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections;

namespace FanHai.Hemera.Share.Interface
{
    public interface ILotExceptionEngine
    {
        /// <summary>
        ///  获取不良信息跟踪信息
        /// </summary>
        /// <param name="lotNo"></param>
        /// <param name="sTime"></param>
        /// <param name="eTime"></param>
        /// <returns></returns>
        DataSet GetLotExceptionData(string lotNo,string sTime,string eTime);
        /// <summary>
        /// 插入不良信息跟踪信息
        /// </summary>
        /// <param name="data"></param>
        DataSet InsertLotExceptionDetail(DataSet data);
        /// <summary>
        /// 更新不良信息跟踪信息
        /// </summary>
        /// <param name="data"></param>
        DataSet UpdateLotExceptionDatail(DataSet data);
        /// <summary>
        /// 删除不良信息跟踪信息
        /// </summary>
        /// <param name="id"></param>
        DataSet DeleteLotExceptionDetail(string id);
        /// <summary>
        /// 根据key获取从扩展熟悉表中获取value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        DataSet GetExtendParam(string key);

        /// <summary>
        /// 获取所以工序
        /// </summary>
        /// <returns></returns>
        DataSet GetAllOperations();
        /// <summary>
        /// 获取工厂
        /// </summary>
        /// <returns></returns>
        DataSet GetAllFab();

        /// <summary>
        /// 获取批次信息，批号，创建时间，partType
        /// </summary>
        /// <returns></returns>
        DataSet GetLotInfo(string lotNo);


    }
}
