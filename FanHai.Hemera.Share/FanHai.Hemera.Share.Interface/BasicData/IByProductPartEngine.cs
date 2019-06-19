using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    public interface IByProductPartEngine
    {
        DataSet GetByFourParameters(string partId, string partType, string partModule, string partClass, ref PagingQueryConfig pconfig);
        
        DataSet GetByPartId(string partid);
        
        DataSet CheckPart(string partId);

        DataSet GetByPartId();

        DataSet GetInfFromPorPartAndProductPart(string name,string partNum, DataTable dtGvlist);

        DataSet Delete(string partId);
    }
}
