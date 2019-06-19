using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    public interface IUdaEngine
    {
        /// <summary>
        /// Add Uda SalesOrderAttr
        /// </summary>
        /// <param name="dataset">dataset for add items cloumns and values</param>
        /// <returns>dataset</returns>
        DataSet AddUdaSalesOrderAttr(DataSet dataset);

        /// <summary>
        /// Delete Uda SalesOrderAttr
        /// </summary>
        /// <param name="dataset">dataset for delete condition</param>
        /// <returns>dataset</returns>
        DataSet DeleteUdaSalesOrderAttr(DataSet dataset);

         /// <summary>
        /// Get Uda SalesOrderAttr
        /// </summary>
        /// <param name="dataset">dataset for select condition</param>
        /// <returns>dataset</returns>
        DataSet GetUdaSalesOrderAttr(DataSet dataset);

        /// <summary>
        /// Update Uda SalesOrderAttr
        /// </summary>
        /// <param name="dataset">dataset for update condition</param>
        /// <returns>dataset</returns>
        DataSet UpdateUdaSalesOrderAttr(DataSet dataset);

    }
}
