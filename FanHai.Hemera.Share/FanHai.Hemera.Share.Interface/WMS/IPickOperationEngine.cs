using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.Common;

namespace FanHai.Hemera.Share.Interface
{
    public interface IPickOperationEngine
    {

        string SavePickData(DataSet ds, string NO);
        /// <summary>
        /// 保存出库存信息。
        /// </summary>
        /// <param name="ds">需要保存的数据 。</param>
        /// /// <param name="NO">出库单号前缀 。</param>
        /// <returns>出库单号。</returns>
        string GetOutBNO(string Prefix, string tabName, string field);
        /// <summary>
        /// 分配单号。
        /// </summary>
        /// <param name="Prefix">单号前缀 </param>
        /// <param name="tabName">表名 </param>
        /// /// /// <param name="field">  字段名</param>
        /// <returns>单号。</returns>
        DataSet ImportPickdata(string _outbandno);
        /// <summary>
        /// 导入保存的出库单信息。
        /// </summary>
        /// <param name="_outbandno">出库单号</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        /// 
        DataSet GetCabinetData(string _CabinetNo, out string msg);
        /// <summary>
        /// 查询柜号相对应的信息。
        /// </summary>
        /// <param name="_CabinetData">柜号</param>
        /// <returns>包含执行结果的数据集对象。</returns>

        void UpdateSapVbeln(string sapVbeln, string OutBNO);
        /// <summary>
        /// 将生成的外向交货单号保存到数据库。
        /// </summary>
        /// <param name="sapVbeln">外向交货单号</param>
        /// <param name="OutBNO">出库单号</param>        
        /// <returns>包含执行结果的数据集对象。</returns>
        /// 
        bool DeleteOutBand(string _orderNO, int flag);
        /// <summary>
        /// 删除出库单。
        /// </summary>
        /// <param name="_orderNO">单号</param>
        /// <param name="flag"> 0 表示出库单，1表示外向交货单</param>
        /// <returns>执行结果。</returns>
        /// 

        DataSet GetCRRelation();
        /// <summary>
        ///  获取SAP中特性的字段名称与WMS中字段名称的对应关系。
        /// </summary>
        /// <returns>包含执行结果的数据集对象。</returns>

        DataSet GetPickedInfo(string _outbandno, string _Sapvbeln);
        /// <summary>
        /// 读取已经确认的出库信息。
        /// </summary>
        /// <param name="_outbandno">出库单号</param>
        /// /// <param name="_Sapvbeln">外向交货单号</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        /// 
        bool UpateCarInfo(string _ci,string _Stype,string _sno,string _ono,string _vno);
        // <summary>
        /// 更新车辆信息
        /// </summary>
        /// <param name="_ci">CI号</param>
        ///  <param name="_Stype">运输类型</param>
        ///  <param name="_sno">车辆牌号</param>
        ///  <param name="_ono">出库单号</param>
        ///  <param name="_vno">外向交货单号</param> 
        /// <returns>包含执行结果的数据集对象。</returns>
        DataSet GetCarInfo(string _ono);        // <summary>
        /// 获取车辆信息        
        ///  <param name="_ono">出库单号</param>      
        /// <returns>包含执行结果的数据集对象。</returns>

    }
}
