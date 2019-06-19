using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Constants;

namespace FanHai.Hemera.Share.Interface
{
    public interface ILotComponentTrayEngine
    {
        /// <summary>
        /// 插入活动序列
        /// </summary>
        /// <returns></returns>
        bool InsertComponentTray(LotCustomerModel model);
        /// <summary>
        /// 获取队列信息
        /// </summary>
        /// <param name="trayName">托盘编号</param>
        /// <returns></returns>
        DataSet SelectComponentTray(string trayValue);
        /// <summary>
        /// 获取线别队列信息
        /// </summary>
        /// <param name="trayName">托盘编号</param>
        /// <returns></returns>
        DataSet SelectComponentTrayLine(string trayValue,string linekey);
        /// <summary>
        /// 产生新托盘号码
        /// </summary>
        /// <returns></returns>
        int CreateTrayNouber();
        ///<summary>
        /// 自动生成单据编号（传单据类型）
        /// </summary>
        /// <param name="strTxt"> 单据类型</param>
        /// <param name="isAdd"> 是否在数据库记录中加 1</param>
        /// <returns> 单据编号或 null</returns>
        string GetShgCod(string strTxt, bool isAdd);
        /// <summary>
        /// 更新OPC Value
        /// </summary>
        /// <param name="strAddress"></param>
        /// <param name="strValue"></param>
        /// <returns></returns>
        bool UpdateOpcValue(string strAddress, string strValue,string strDateTime);

    }
}
