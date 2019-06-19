using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
   public interface IQMSEngine
    {
        /// <summary>
        /// Get Battery Grade Color
        /// </summary>
        /// <returns></returns>
        DataSet GetBatteryGradeColor();

        /// <summary>
        /// Get Battery Thickness
        /// </summary>
        /// <returns></returns>
        DataSet GetBatteryThickness();

        /// <summary>
        /// Get Battery Type
        /// </summary>
        /// <returns></returns>
        DataSet GetBatteryType();

        /// <summary>
        /// Get Cell Type Code
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        DataSet GetCellTypeCode(DataTable dataTable);

        /// <summary>
        /// Get Base Ncell
        /// </summary>
        /// <returns></returns>
        DataSet GetBaseNcell();

        /// <summary>
        /// Get Base Efficiency
        /// </summary>
        /// <returns></returns>
        DataSet GetBaseEfficiency();

        /// <summary>
        /// Get Max Box Index
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        DataSet GetMaxBoxIndex(DataTable dataTable);

        /// <summary>
        /// Add Combineed Cell Package
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        DataSet AddCombineedCellPackage(DataTable dataTable);

        /// <summary>
        /// Add Battery Package
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        //DataSet AddBatteryPackage(DataTable dataTable);

        /// <summary>
        /// Add Dtail Information
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        DataSet AddDtailInformation(DataTable dataTable);

        /// <summary>
        /// Svae Abnormity Information
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        DataSet SaveAbnormityInformation(DataTable dataTable);

        /// <summary>
        /// Get Stop Abnormity Infor
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        DataSet GetStopAbnormityInfor(DataTable dataTable);

        /// <summary>
        /// Save Category Test Config
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        DataSet SaveCategoryTestConfig(DataTable dataTable);

        /// <summary>
        /// 获取配置信息。<see cref="QMS.BASE_CODE"/>
        /// </summary>
        /// <param name="dataTable">
        /// 包含查询条件的数据表对象。
        /// </param>
        /// <returns>包含查询结果的数据集对象。</returns>
        DataSet GetConfigInformation(DataTable dataTable);

        /// <summary>
        /// Add Barcode Serial
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        DataSet AddBarcodeSerial(DataSet dataSet);

        /// <summary>
        /// Update Barcode Serial
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        //DataSet UpdateBarcodeSerial(DataTable dataTable);
        DataSet UpdateBarcodeSerial(DataSet dataSet);

        /// <summary>
        /// Delete Abnormity Information
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        DataSet DeleteAbnormityInformation(DataTable dataTable);

        /// <summary>
        /// Get Rule Name and Description
        /// </summary>
        /// <returns></returns>
        DataSet GetRuleNameDescription(string classType,string computerName);

        /// <summary>
        /// Get Rule Test Type
        /// </summary>
        /// <returns></returns>
        DataSet GetRuleTestTypeCategory();

        /// <summary>
        /// Get Rule Of Class File
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        DataSet GetRuleOfClassFile(DataTable dataTable);

        /// <summary>
        /// Get Rule Test Type Detail
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        DataSet GetRuleTestTypeDetail(DataTable dataTable);

        /// <summary>
        /// Get Test Rule Contains Type
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        DataSet GetTestRuleContainsType(DataTable dataTable);

        /// <summary>
        /// Import Class File Data
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        DataSet ImportClassFileData(DataSet dataset);

        /// <summary>
        /// Get Class File Information
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        DataSet GetClassFileInformation(DataTable dataTable);       

        DataSet GetResultColumnNameFromConfig(string factory);
        DataSet GetLine();

        DataSet GetPacketNumber(DataTable dataTable);

        DataSet UpdateBatteryPackets(DataTable dataTable, int index, string printFlag);

        void UpdateBatteryPacket(string packetNumber);

        string GetLastDateOfResult(string factoryName, string computerName);

       // DataSet SearchWorkOrderDetailInfor(string workOrderKey);

        DataSet GetWorkOrderPacketInforByType(string workOrder,string packetType);

        DataSet GetQMSStopInfo(string computerName);
        bool UpdatePacketNumber(DataTable table);
        DataSet GetClassCodeForManualPrint(string computerName);

        DataSet CheckBarcodeValidate(DataTable dataTable);
        DataSet CheckBarcodeAndReturnData(string barcode);
        string GetColumnNameFromResultConfigTable(string factoryName);
    }
}
