/*
<FileInfo>
  <Author>Alfred.Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using FanHai.Hemera.Share.Interface;
using FanHai.Hemera.Share.Constants;
using FanHai.Hemera.Utils;

using Microsoft.Practices.EnterpriseLibrary.Data;

namespace FanHai.Hemera.Modules.EDC
{
    /// <summary>
    /// 
    /// </summary>
    public class CalculateEngine : AbstractEngine, ICalculateEngine
    {
        private Database db; //数据库对象。
        /// <summary>
        /// 构造函数。
        /// </summary>
        public CalculateEngine()
        {
            db = DatabaseFactory.CreateDatabase();
        }

        #region Initialize
        /// <summary>
        /// initialize
        /// </summary>
        public override void Initialize() { }
        #endregion

        #region Get calculate result via input param
        /// <summary>
        /// Get calculate result via input param
        /// </summary>
        /// <param name="dataSet">Param dataSet</param>
        /// <returns>Result dataSet</returns>
        public DataSet ExecuteCalculate(DataSet dataSet)
        {
            try
            {
                //Prepare data
                PrepareData(dataSet);

                //Eexcute calculate
                ExecCalculate();

                //Return backfill data 
                return dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Analysis and prepare data
        /// <summary>
        /// Analysis and prepare data
        /// </summary>
        /// <param name="dataSet">dataSet for need deal with data</param>
        private void PrepareData(DataSet dataSet)
        {
            try
            {
                #region Analysis data and get mutuality info
                //Analysis data and get mutuality info
                if (dataSet.Tables.Contains(TRANS_TABLES.TABLE_PARAM))
                {
                    paramTable = dataSet.Tables[TRANS_TABLES.TABLE_PARAM];

                    foreach (DataRow dataRow in paramTable.Rows)
                    {
                        if (paramTable.Columns.Contains(POR_LOT_FIELDS.FIELD_LOT_KEY))
                        {
                            lotKey = dataRow[POR_LOT_FIELDS.FIELD_LOT_KEY].ToString();
                        }

                        if (paramTable.Columns.Contains(EDC_SP_FIELDS.FIELD_SAMPLING_SIZE))
                        {
                            sampCount = Convert.ToInt32(dataRow[EDC_SP_FIELDS.FIELD_SAMPLING_SIZE]);
                        }

                        if (paramTable.Columns.Contains(EDC_SP_FIELDS.FIELD_UNIT_SAMPLING_SIZE))
                        {
                            unitCount = Convert.ToInt32(dataRow[EDC_SP_FIELDS.FIELD_UNIT_SAMPLING_SIZE]);
                        }
                    }
                }

                if (dataSet.Tables.Contains(EDC_COLLECTION_DATA_FIELDS.DATABASE_TABLE_NAME))
                {
                    backTable = dataSet.Tables[EDC_COLLECTION_DATA_FIELDS.DATABASE_TABLE_NAME];

                    foreach (DataRow dataRow in backTable.Rows)
                    {
                        if (backTable.Columns.Contains(EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE))
                        {
                            if (backTable.Columns.Contains(EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY))
                            {
                                if (dataRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE].ToString() == string.Empty)
                                {
                                    if (backFillParam.Count < 1)
                                    {
                                        backFillParam.Add(dataRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY].ToString());
                                    }
                                    else
                                    {
                                        foreach (string param in backFillParam)
                                        {
                                            if (param == dataRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY].ToString())
                                            {
                                                IsExist = true;
                                                continue;
                                            }
                                        }

                                        if (!IsExist)
                                        {
                                            backFillParam.Add(dataRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY].ToString());
                                        }

                                        IsExist = false;
                                    }
                                }
                                else
                                {
                                    if (currCalcParam.Count < 1)
                                    {
                                        currCalcParam.Add(dataRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY].ToString());
                                    }
                                    else
                                    {
                                        foreach (string param in currCalcParam)
                                        {
                                            if (param == dataRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY].ToString())
                                            {
                                                IsExist = true;
                                                continue;
                                            }
                                        }

                                        if (!IsExist)
                                        {
                                            currCalcParam.Add(dataRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY].ToString());
                                        }

                                        IsExist = false;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Execulate calculate
        /// <summary>
        /// Execulate calculate
        /// </summary>
        private void ExecCalculate()
        {
            try
            {
                foreach (string backParam in backFillParam)
                {
                    int validValue = 0;
                    double currValue = 0.0d;
                    double? calcValue = null;
                    GetCalculateType(backParam, ref calcType);
                    List<string> calcParam = GetAllCalculateParam(backParam);
                    if (calcParam.Count < 1)
                    {
                        throw new Exception("${res:FanHai.Hemera.Modules.AutoCalc.NotConfigCalcParam}");
                    }

                    switch (calcType)
                    {
                        case COMMON_SYMBOL.CALC_MINUS:
                            CalcMinus(calcParam, backParam, currValue, calcValue, validValue);
                            break;
                        case COMMON_SYMBOL.CALC_DIVIDE:
                            CalcDivide(calcParam, backParam, currValue, calcValue, validValue);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Calculate minus
        /// <summary>
        /// Calculate minus
        /// </summary>
        /// <param name="calcParam">List for join calculate param's key</param>
        /// <param name="backParam">Require backfill param's key</param>
        /// <param name="currValue">Current join calculate value</param>
        /// <param name="calcValue">Calculate result value</param>
        /// <param name="validValue">Validate flag value</param>
        private void CalcMinus(List<string> calcParam, string backParam, double currValue, double? calcValue, int validValue)
        {
            string currParam = string.Empty;
            foreach (string param in currCalcParam)
            {
                if (calcParam.Contains(param))
                {
                    currParam = param;
                    calcParam.Remove(currParam);
                }
            }

            if (calcParam.Count > 1)
            {
                throw new Exception("${res:FanHai.Hemera.Modules.AutoCalc.ConfigParamError}");
            }

            #region Get previous step info
            //Get previous step info
            DataTable prevTable = GetPreviousTable(lotKey, calcParam[0]);
            #endregion

            if (prevTable.Rows.Count < 1)
            {
                throw new Exception("${res:FanHai.Hemera.Modules.AutoCalc.NoWeighing}");
            }


            #region Calculate data and backfill mutuality data
            //Calculate data and backfill mutuality data

            foreach (DataRow prevRow in prevTable.Rows)
            {
                currValue = 0.0d;
                double prevValue = 0.0d;
                foreach (DataRow backRow in backTable.Rows)
                {
                    if (backRow[EDC_COLLECTION_DATA_FIELDS.FIELD_SP_SAMP_SEQ].ToString() ==
                        prevRow[EDC_COLLECTION_DATA_FIELDS.FIELD_SP_SAMP_SEQ].ToString() &&
                        backRow[EDC_COLLECTION_DATA_FIELDS.FIELD_SP_UNIT_SEQ].ToString() ==
                        prevRow[EDC_COLLECTION_DATA_FIELDS.FIELD_SP_UNIT_SEQ].ToString())
                    {
                        if (prevRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY].ToString() == calcParam[0])
                        {
                            prevValue = Convert.ToDouble(prevRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE]);
                        }
                        if (backRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY].ToString() == currParam)
                        {
                            currValue = Convert.ToDouble(backRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE]);
                        }

                        if (prevValue != 0.0d && currValue != 0.0d)
                        {
                            break;
                        }
                    }
                }

                if (prevValue == 0.0d)
                    continue;

                foreach (DataRow backRow in backTable.Rows)
                {
                    if (backRow[EDC_COLLECTION_DATA_FIELDS.FIELD_SP_SAMP_SEQ].ToString() ==
                        prevRow[EDC_COLLECTION_DATA_FIELDS.FIELD_SP_SAMP_SEQ].ToString() &&
                        backRow[EDC_COLLECTION_DATA_FIELDS.FIELD_SP_UNIT_SEQ].ToString() ==
                        prevRow[EDC_COLLECTION_DATA_FIELDS.FIELD_SP_UNIT_SEQ].ToString() &&
                        backRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY].ToString() == backParam &&
                        backRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE].ToString() == string.Empty)
                    {

                        calcValue = prevValue - currValue;
                        backRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE] = calcValue;
                        CheckValueRange(backParam, calcValue, ref validValue);
                        backRow[EDC_COLLECTION_DATA_FIELDS.FIELD_VALID_FLAG] = validValue;

                        if (validValue == 2)
                        {
                            foreach (DataRow dataRow in paramTable.Rows)
                            {
                                dataRow[EDC_COLLECTION_DATA_FIELDS.FIELD_VALID_FLAG] = 1;
                            }
                        }
                    }
                }
            }
            #endregion
        }
        #endregion

        #region Calculate divide
        /// <summary>
        /// Calculate divide
        /// </summary>
        /// <param name="calcParam">List for join calculate param's key</param>
        /// <param name="backParam">Require backfill param's key</param>
        /// <param name="currValue">current join calculate value</param>
        /// <param name="calcValue">Calculate result value</param>
        /// <param name="validValue">Validate flag value</param>
        private void CalcDivide(List<string> calcParam, string backParam, double currValue, double? calcValue, int validValue)
        {
            foreach (string param in calcParam)
            {
                if (!currCalcParam.Contains(param))
                {
                    throw new Exception("${res:FanHai.Hemera.Modules.AutoCalc.ConfigParamError}");
                }

                DataView dv = new DataView(backTable);
                dv.RowFilter = EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY + " = '" + param + "'";
                DataTable dvTable = dv.ToTable();

                foreach (DataRow dvRow in dvTable.Rows)
                {
                    double? maxValue = null;
                    double? minValue = null;

                    DataRow[] listRow = backTable.Select(EDC_COLLECTION_DATA_FIELDS.FIELD_SP_SAMP_SEQ + " = '" +
                                                             dvRow[EDC_COLLECTION_DATA_FIELDS.FIELD_SP_SAMP_SEQ].ToString() + "' AND " +
                                                         EDC_COLLECTION_DATA_FIELDS.FIELD_SP_UNIT_SEQ + " = '" +
                                                             dvRow[EDC_COLLECTION_DATA_FIELDS.FIELD_SP_UNIT_SEQ].ToString() + "'");

                    foreach (DataRow backRow in listRow)
                    {
                        if (calcParam.Contains(backRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY].ToString()))
                        {
                            currValue = Convert.ToDouble(backRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE]);

                            if (maxValue == null || currValue > maxValue)
                            {
                                maxValue = currValue;
                            }

                            if (minValue == null || currValue < minValue)
                            {
                                minValue = currValue;
                            }
                        }
                    }

                    foreach (DataRow backRow in listRow)
                    {
                        if (backRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_KEY].ToString() == backParam)
                        {
                            calcValue = (maxValue - minValue) / (maxValue + minValue);
                            backRow[EDC_COLLECTION_DATA_FIELDS.FIELD_PARAM_VALUE] = calcValue;
                            CheckValueRange(backParam, calcValue, ref validValue);
                            backRow[EDC_COLLECTION_DATA_FIELDS.FIELD_VALID_FLAG] = validValue;

                            if (validValue == 2)
                            {
                                foreach (DataRow dataRow in paramTable.Rows)
                                {
                                    dataRow[EDC_COLLECTION_DATA_FIELDS.FIELD_VALID_FLAG] = 1;
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Get calculate type via param key
        /// <summary>
        /// Get calculate type via param key
        /// </summary>
        /// <param name="paramKey">Param's key</param>
        /// <param name="calcType">Calculate type such as + - * /</param>
        private void GetCalculateType(string paramKey, ref string calcType)
        {
            string sqlCommand = string.Empty;

            try
            {
                sqlCommand = string.Format(@"SELECT CALCULATE_TYPE FROM BASE_PARAMETER WHERE PARAM_KEY = '{0}'", paramKey);
                calcType = db.ExecuteScalar(CommandType.Text, sqlCommand).ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get all calculate param's key
        /// <summary>
        /// Get all calculate param's key 
        /// </summary>
        /// <param name="paramKey">Require backfill param's key</param>
        /// <returns>List for all relation param's key</returns>
        private List<string> GetAllCalculateParam(string paramKey)
        {
            string sqlCommand = string.Empty;
            List<string> paramList = new List<string>();

            try
            {
                sqlCommand = string.Format(@"SELECT T.PARAM_KEY
                                               FROM BASE_PARAMETER_DERIVTION T
                                              WHERE T.DERIVATION_KEY = '{0}'", paramKey);

                using (IDataReader dataReader = db.ExecuteReader(CommandType.Text, sqlCommand))
                {
                    while (dataReader.Read())
                    {
                        paramList.Add(dataReader[0].ToString());
                    }

                    dataReader.Close();
                }

                return paramList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get previous step info
        /// <summary>
        /// Get previous step info
        /// </summary>
        /// <param name="lotKey">Lot key</param>
        /// <param name="stepKey">Step key</param>
        /// <returns>DataTable for previous step info</returns>
        private DataTable GetPreviousTable(string lotKey, string paramKey)
        {
            try
            {
                string sqlCommand = string.Empty;

                sqlCommand = string.Format(@"SELECT TOP {2} *
                                             FROM (SELECT A.*
                                                   FROM EDC_COLLECTION_DATA A, EDC_MAIN_INS B
                                                   WHERE A.EDC_INS_KEY = B.EDC_INS_KEY
                                                   AND A.PARAM_KEY = '{0}'
                                                   AND B.LOT_KEY = '{1}') T
                                             ORDER BY T.EDIT_TIME DESC", paramKey, lotKey, sampCount * unitCount);

                return db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Cheeck calculate result range
        /// <summary>
        /// Cheeck calculate result range
        /// </summary>
        /// <param name="paramKey">Param's key</param>
        /// <param name="paramValue">Param's value</param>
        /// <param name="validValue">Valid flag value</param>
        public void CheckValueRange(string paramKey, double? paramValue, ref int validValue)
        {
            string sqlCommand = string.Empty;

            try
            {
                sqlCommand = string.Format(@"SELECT LOWER_SPEC, UPPER_SPEC, LOWER_BOUNDARY, UPPER_BOUNDARY
                                             FROM BASE_PARAMETER
                                             WHERE PARAM_KEY = '{0}'", paramKey);

                DataTable dataTable = db.ExecuteDataSet(CommandType.Text, sqlCommand).Tables[0];

                foreach (DataRow dataRow in dataTable.Rows)
                {
                    if (dataTable.Rows.Count == 0 ||
                        dataRow[BASE_PARAMETER_FIELDS.FIELD_LOWER_BOUNDARY] == DBNull.Value ||
                        dataRow[BASE_PARAMETER_FIELDS.FIELD_UPPER_BOUNDARY] == DBNull.Value ||
                        dataRow[BASE_PARAMETER_FIELDS.FIELD_LOWER_SPEC] == DBNull.Value ||
                        dataRow[BASE_PARAMETER_FIELDS.FIELD_UPPER_SPEC] == DBNull.Value)
                    {
                        return;
                    }

                    if (paramValue >= Convert.ToDouble(dataRow[BASE_PARAMETER_FIELDS.FIELD_LOWER_BOUNDARY]) &&
                        paramValue <= Convert.ToDouble(dataRow[BASE_PARAMETER_FIELDS.FIELD_UPPER_BOUNDARY]))
                    {
                        if (paramValue >= Convert.ToDouble(dataRow[BASE_PARAMETER_FIELDS.FIELD_LOWER_SPEC]) &&
                            paramValue <= Convert.ToDouble(dataRow[BASE_PARAMETER_FIELDS.FIELD_UPPER_SPEC]))
                        {
                            validValue = 0;
                        }
                        else
                        {
                            validValue = 1;
                        }
                    }
                    else
                    {
                        validValue = 2;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Define variable
        //Define variable
        string lotKey = string.Empty;
        string calcType = string.Empty;

        int sampCount = 0;
        int unitCount = 0;
        bool IsExist = false;
        DataTable backTable = null;
        DataTable paramTable = null;
        List<string> backFillParam = new List<string>();
        List<string> currCalcParam = new List<string>();
        List<string> prevCalcParam = new List<string>();
        #endregion
    }
}
