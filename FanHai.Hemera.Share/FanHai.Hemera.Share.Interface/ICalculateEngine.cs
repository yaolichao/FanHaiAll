using System;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    public interface ICalculateEngine
    {
        /// <summary>
        /// Get calculate result via input param
        /// </summary>
        /// <param name="dataSet">Param dataSet</param>
        /// <returns>Result dataSet</returns>
        DataSet ExecuteCalculate(DataSet dataSet);

        /// <summary>
        /// Cheeck calculate result range
        /// </summary>
        /// <param name="paramKey">Param's key</param>
        /// <param name="paramValue">Param's value</param>
        /// <param name="validValue">Valid flag value</param>
        void CheckValueRange(string paramKey, double? paramValue, ref int validValue);
    }
}
