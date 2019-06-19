using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Interface
{
    /// <summary>
    /// 托盘信息查询操作接口。
    /// </summary>
    public interface IWarehouseEngine
    {
        /// <summary>
        /// 查询包含托盘号的字符串。
        /// </summary>
        /// </param>
        /// <returns>包含托盘信息的数据集。</returns>
        DataSet SearchPallet(string dsPalltNo);
        DataSet SearchPelletInfToList(string PalletNo, string work_order,string part_number);

        DataSet CreateRkKoPo(DataSet dsIn);

        DataSet GetBwart(string p);

        DataSet GetRkKoInformation(string rkNum, string Werks, string WorkOrder, string Status, string StartTime, string EndTime);

        DataSet GetKoPoByRkNumber(string rknum);

        DataSet RfcToDeleteRk(string rkNum,string werks);

        DataSet GetKoPoByRkNumberForPrint(string koRkNumber);

        DataSet InsertPrintInf(string number, string name);

        DataSet EditRkKoPo(DataSet dsIn);

        DataSet GetPo(string rkNum,string pallet_no);

        //获取全部工单号
        DataSet GetWorkNumber();
        //根据工单获取工单对应的信息
        DataSet GetPorWorkOrderInfByWorkNo(string workNumber);
        //根据托号获取托对应的真是工单(混工单成托)
        DataSet GetPalletWorkorder(string paNo, string work_order);
        //获取保税手册号
        string GetBonded(string bonded);
    }
}
