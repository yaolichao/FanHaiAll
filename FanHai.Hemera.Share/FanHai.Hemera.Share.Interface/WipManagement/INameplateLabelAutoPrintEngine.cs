using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FanHai.Hemera.Share.Interface
{
    public interface INameplateLabelAutoPrintEngine
    {
        /// <summary>
        /// 获取名牌自动打印的信息
        /// </summary>
        /// <returns></returns>
        DataSet getInfoForNamepalteLabelAutoPrint(string lotNum);


        /// <summary>
        /// 获取名牌自动打印的信息体现功率档位所对应的五大参数 yibin.fei 2017.10.26
        /// </summary>
        /// <param name="lotNum"></param>
        /// <param name="AFTERPOWER"></param>
        /// <returns></returns>
        DataSet getInfoForNamepalteLabelAutoPrintForPowerShow(string lotNum, string AfterPower);

        /// <summary>
        /// 根据prodid获取长宽高
        /// </summary>
        /// <param name="prodId"></param>
        /// <returns></returns>
        DataSet getSizeForQTX(string prodId);
        /// <summary>
        /// 获取所有模板信息
        /// </summary>
        /// <returns></returns>
        DataSet getInfoForTemplate();
        /// <summary>
        /// 新增模板
        /// </summary>
        /// <returns></returns>
        bool addTemplate(string prodId,string template,string editor,string editTime);
        /// <summary>
        /// 更新模板信息
        /// </summary>
        /// <param name="prodId"></param>
        /// <param name="template"></param>
        /// <param name="editor"></param>
        /// <param name="editTime"></param>
        /// <returns></returns>
        bool updateTemplate(string prodId, string template, string editor, string editTime);
        /// <summary>
        /// 根据产品id获取模板
        /// </summary>
        /// <param name="prodId"></param>
        /// <returns></returns>
        DataSet getTemplateByProdId(string prodId);
    }
}
