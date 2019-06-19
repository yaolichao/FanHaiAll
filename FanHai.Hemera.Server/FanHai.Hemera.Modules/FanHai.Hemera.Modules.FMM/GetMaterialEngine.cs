using System;
using System.Collections.Generic;
using System.Text;
using SunTech.MES.CommonLibrary;
using SunTech.MES.Interface;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SunTech.MES.Module.SystemManagement
{
    public class GetMaterialEngine : AbstractEngine,IGetMaterialsEngine
    {
        private Database database = null;

        #region Initialize
        /// <summary>
        /// initialize
        /// </summary>
        public override void Initialize() { }
        #endregion
        public GetMaterialEngine()
        {
            database = DatabaseFactory.CreateDatabase();
        }
        public DataSet AddMaterialInformation(DataSet dataSet)
        {
            DataSet reqDS = new DataSet();
            if (null == dataSet)
            {
                List<string> salCommandList = new List<string>();
                
            }
            return reqDS;
        }

        public DataSet QueryMaterialInformation(DataSet dataSet)
        {
            DataSet reqDS = new DataSet();

            return reqDS;
        }

        public DataSet UpdateMaterialInformation(DataSet dataSet)
        {
            DataSet reqDS = new DataSet();

            return reqDS;
        }

        public DataSet DeleteMaterialInformation(DataSet dataSet)
        {
            DataSet reqDS = new DataSet();

            return reqDS;
        }
    }
}
