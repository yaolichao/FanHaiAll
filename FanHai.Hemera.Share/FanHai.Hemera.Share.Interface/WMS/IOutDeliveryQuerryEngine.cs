using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    public interface IOutDeliveryQuerryEngine
    {
        /// <summary>
        /// 根据查询条件获取外向交货明细数据
        /// </summary>
        /// <param name="ZMMTYP"></param>
        /// <returns></returns>
        DataSet OutDeliveryQuerry(DataSet dsParams);
        /// <summary>
        /// 根据查询条件获取质检明细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataSet GetQCItem(string VBELN, string POSNR);
    }
}
