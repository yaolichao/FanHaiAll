//----------------------------------------------------------------------------------
// Copyright (c) CHINT
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-09-13            重构 迁移到SQL Server数据库
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SolarViewer.Hemera.Utils;
using SolarViewer.Hemera.Share.Constants;
using System.Data.Common;
using System.Collections;
using SolarViewer.Hemera.Utils.StaticFuncs;
using SolarViewer.Hemera.Utils.DatabaseHelper;
using SolarViewer.Hemera.Modules.WipJob;
using SolarViewer.Hemera.Modules.FMM;
using SolarViewer.Hemera.Modules.EMS;

namespace SolarViewer.Hemera.Modules.Wip
{
    public partial class WipEngine 
    {
        /// <summary>
        /// 批次自动进站。
        /// </summary>
        /// <param name="lotKey">批次主键。</param>
        /// <param name="equipmentKey">设备主键。</param>
        /// <param name="shiftName">班次名称。</param>
        /// <returns>包含执行结果的数据集对象。</returns>
        private DataSet AutoTrackIn(string lotKey, string equipmentKey, string shiftName)
        {
            //getLotDetail 
            Hashtable mainDataHashTable = new Hashtable();
            DataTable mainDataTable = new DataTable();
            DataSet dataSet = new DataSet();
            DataSet dsReturn = new DataSet();
            DataSet dsLots = new DataSet();
            string operationKey = "", lineKey = "", strLineName = "", operateLineName = "";
            string strToUser = "", editor = "", editTimeZone = "", childLineName = "";
            try
            {
                if (!string.IsNullOrEmpty(lotKey))
                {

                    dsLots = GetLotsInfo(lotKey);
                    if (dsLots != null && dsLots.Tables.Count > 0 && dsLots.Tables[0].Rows.Count > 0)
                    {
                        editor = dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_EDITOR].ToString();
                        strToUser = editor;
                        editTimeZone = dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_EDIT_TIMEZONE].ToString();
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_TYPE, "0");
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_PIECE_KEY, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_KEY].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY, ACTIVITY_FIELD_VALUES.FIELD_ACTIVITY_TRACKIN);
                        mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_LOT_KEY, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_KEY].ToString());
                        mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_LOT_NUMBER, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LOT_NUMBER].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_ACTIVITY_COMMENT, string.Empty);
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDITOR, editor);
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIMEZONE_KEY, editTimeZone);
                        //mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_EDIT_TIME, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_EDIT_TIME].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STATE_FLAG, "2");
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_REWORK_FLAG, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_IS_REWORKED].ToString());

                        lineKey = dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_PRODUCTION_LINE_KEY].ToString();
                        strLineName = dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_LINE_NAME].ToString();
                        operateLineName = dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_OPR_LINE].ToString();
                        //childLineName = dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CHILD_LINE].ToString();

                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_LINE_KEY, lineKey);
                        mainDataHashTable.Add(POR_LOT_FIELDS.FIELD_LINE_NAME, strLineName);
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_SHIFT_NAME, shiftName);
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_WORK_ORDER_KEY, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_WORK_ORDER_KEY].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_STEP_KEY, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_CUR_STEP_VER_KEY].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_QUANTITY_IN, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_QUANTITY].ToString());
                        operationKey = dsLots.Tables[0].Rows[0][POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY].ToString();

                        //----------------
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPERATOR, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_OPERATOR].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_OPR_LINE].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_LINE_PRE, dsLots.Tables[0].Rows[0][POR_LOT_FIELDS.FIELD_OPR_LINE].ToString());
                        mainDataHashTable.Add(WIP_TRANSACTION_FIELDS.FIELD_OPR_COMPUTER, "SERVER");

                        mainDataTable.TableName = TRANS_TABLES.TABLE_PARAM;
                        mainDataTable = SolarViewer.Hemera.Share.Common.CommonUtils.ParseToDataTable(mainDataHashTable);
                        dataSet.Tables.Add(mainDataTable);
                    }
                }
                if (equipmentKey == string.Empty)
                {
                    //find equipment
                    OperationEquipments operationequipment = new OperationEquipments();
                    //edit by rayna 2011-4-25 search equipment by child line
                    //DataSet dsEquipment = operationequipment.GetOperationEquipment(operationKey, operateLineName);
                    DataSet dsEquipment = operationequipment.GetOperationEquipment(operationKey, childLineName);
                    if (dsEquipment != null && dsEquipment.Tables.Contains(EMS_OPERATION_EQUIPMENT_FIELDS.DATABASE_TABLE_NAME))
                    {
                        if (dsEquipment.Tables[EMS_OPERATION_EQUIPMENT_FIELDS.DATABASE_TABLE_NAME].Rows.Count > 0)
                        {
                            equipmentKey = dsEquipment.Tables[EMS_OPERATION_EQUIPMENT_FIELDS.DATABASE_TABLE_NAME].Rows[0]["EQUIPMENT_KEY"].ToString();
                        }
                    }
                }
                Hashtable equHashData = new Hashtable();
                DataTable equDataTable = new DataTable();
                equHashData.Add(EMS_EQUIPMENTS_FIELDS.FIELD_EQUIPMENT_KEY, equipmentKey);
                equHashData.Add(POR_ROUTE_OPERATION_VER_FIELDS.FIELD_ROUTE_OPERATION_VER_KEY, operationKey);
                equDataTable = SolarViewer.Hemera.Share.Common.CommonUtils.ParseToDataTable(equHashData);
                equDataTable.TableName = EMS_EQUIPMENTS_FIELDS.DATABASE_TABLE_NAME;
                dataSet.Tables.Add(equDataTable);
                dsReturn = TrackInLot(dataSet);
                int code = 0;
                string errorMsg = SolarViewer.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn, ref code);

                if (errorMsg != string.Empty)
                {
                    RecordErrorMessage("批次进站异常", errorMsg, strToUser, "EMSGOUT", editor, editTimeZone, lotKey, "LOT");
                }
            }
            catch (Exception ex)
            {
                SolarViewer.Hemera.Share.Common.ReturnMessageUtils.AddServerReturnMessage(dsReturn, ex.Message);
                RecordErrorMessage("自动进站异常", ex.Message, strToUser, "EMSGOUT", editor, editTimeZone, lotKey, "JOB");
            }
            return dsReturn;
        }
    }
}
