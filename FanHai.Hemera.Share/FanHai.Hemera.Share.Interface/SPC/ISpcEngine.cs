using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace FanHai.Hemera.Share.Interface
{
    public interface ISpcEngine
    {
        /// <summary>
        /// Get params from spc_parameter table
        /// </summary>
        /// <returns></returns>
        //DataSet GetParams();

        //DataSet SearchParam(DataSet dataset);

        //DataSet SaveParams(DataSet dataset);
        DataSet GetTableAvData(string strControlCode, string strCol_Key);//Q.0031
        DataSet GetTableData(string strControlCode, string strCol_Key);//Q.003
        /// <summary>
        /// 保存抽检界面数据保存时，spc相应操作。
        /// </summary>
        /// <param name="dataset">包含SPC数据的数据集对象。</param>
        /// <returns>true：保存成功。false:保存失败。</returns>
        DataSet SaveParamData(DataSet dataset);

        DataSet DeleteRParamData(string edcInskey);

        DataSet DeleteParamData(string transactionKey);

        DataSet SearchParamValue(DataTable dataTable);
        DataSet GetStandardAndPSigma(DataTable dataTable,string avg);

        DataSet SearchParamValueForXbar(DataTable dataTable);
        /// <summary>
        /// SPC控制图数据源 单点移动极差
        /// </summary>
        /// <param name="dataTable">查询条件</param>
        /// <returns>返回单点移动极差的数据</returns>
        DataSet SearchParamValueForMr(DataTable dataTable);
        /// <summary>
        /// 修改数据中的异常点
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        DataSet ModifyPoints(DataSet dataSet);
        DataSet ModifyPointsForMr(DataSet dataSet);
        /// <summary>
        /// 剔除数据中的异常点
        /// </summary>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        DataSet DeletePoints(DataSet dataSet);
        /// <summary>
        /// 获得SPC控制点明细信息
        /// </summary>
        /// <param name="edcInskey"></param>
        /// <returns></returns>
        DataSet GetPointInformation(string edcInskey);
        /// <summary>
        /// 获得SPC编辑点信息
        /// </summary>
        /// <param name="edccolkey"></param>
        /// <returns></returns>
        DataSet GetEditInformation(string edccolkey);

        DataSet GetPointDetailInformation(string pointkey);

        DataSet SearchPChartData(DataTable dataTable);
        /// <summary>
        /// 获得SPC控制计划基本数据的绑定信息
        /// </summary>
        /// <returns></returns>
        DataSet GetSPControlData();
        /// <summary>
        /// 获得SPC控制计划主信息
        /// </summary>
        /// <returns></returns>
        DataSet GetSPControlGridData();
        /// <summary>
        /// 新增/修改 SPC控制计划资料
        /// </summary>
        /// <param name="dsSpcControl"></param>
        /// <returns></returns>
        DataSet SaveSpcControlPlan(DataSet dsSpcControl);
        /// <summary>
        /// SPC管控规则删除或更新
        /// </summary>
        /// <param name="arrLst"></param>
        /// <returns></returns>
        DataSet UpdateControlPlan(List<string> arrLst);
    }
}
