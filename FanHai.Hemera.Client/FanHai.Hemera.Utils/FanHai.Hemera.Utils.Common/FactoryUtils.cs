//----------------------------------------------------------------------------------
// Copyright (c) FanHai
//----------------------------------------------------------------------------------
// =================================================================================
// 修改人               修改时间              说明
// ---------------------------------------------------------------------------------
// Peter.Zhang          2012-03-19            新建
// =================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FanHai.Hemera.Share.Interface;
using FanHai.Gui.Core;
namespace FanHai.Hemera.Utils.Common
{
    /// <summary>
    /// 同工厂数据相关的工具类。
    /// </summary>
    /// add by peter 2012-03-19
    public static class FactoryUtils
    {
        /// <summary>
        /// 根据线别名称获取线别所属的车间。
        /// </summary>
        /// <param name="lines">使用逗号分开的线别名称。</param>
        /// <returns>包含工厂车间信息的数据集对象【LOCATION_KEY，LOCATION_NAME】。
        /// </returns>
        public static DataTable GetFactoryRoomByLines(string lines)
        {
            DataSet dsReturn = new DataSet();
            string errorMsg = string.Empty;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateILocationEngine().GetFactoryRoomByLines(lines);
                    
                    errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (errorMsg != string.Empty)
                    {
                        throw new Exception(errorMsg);
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn.Tables[0];
        }

        /// <summary>
        /// 根据线上仓数据获取线上仓所属的车间。
        /// </summary>
        /// <param name="stores">使用逗号分开的线上仓名称。</param>
        /// <returns>
        /// 包含工厂车间信息的数据集对象。【LOCATION_KEY，LOCATION_NAME】。
        /// </returns>
        /// add by peter 2012-03-19
        public static DataTable GetFactoryRoomByStores(string stores)
        {
            DataSet dsReturn = new DataSet();
            string errorMsg = string.Empty;
            try
            {
                IServerObjFactory serverFactory = CallRemotingService.GetRemoteObject();
                if (null != serverFactory)
                {
                    dsReturn = serverFactory.CreateILocationEngine().GetFactoryRoomByStores(stores);
                    errorMsg = FanHai.Hemera.Share.Common.ReturnMessageUtils.GetServerReturnMessage(dsReturn);
                    if (errorMsg != string.Empty)
                    {
                        throw new Exception(errorMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CallRemotingService.UnregisterChannel();
            }
            return dsReturn.Tables[0];
        }
       
    }
}
