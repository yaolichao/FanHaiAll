using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;

/// <summary>
///DateTimeHelper 的摘要说明
/// </summary>
public sealed class DateTimeHelper
{
    /// <summary>
    /// 今年第几周,年的第一周从年的第一天开始，到指定周的下一个首日结束。
    /// </summary>
    /// <param name="datetime"></param>
    /// <param name="dayOfWeek"></param>
    /// <returns></returns>
    public static int GetWeekOfYearFirstDay(DateTime datetime,DayOfWeek dayOfWeek)
    {
        int weekYear = new System.Globalization.GregorianCalendar()
                            .GetWeekOfYear(datetime, 
                                           System.Globalization.CalendarWeekRule.FirstDay, 
                                           dayOfWeek);
        return weekYear;
    }
    /// <summary>  
    /// 得到本周第一天(以星期天为第一天)  
    /// </summary>  
    /// <param name="datetime"></param>  
    /// <returns></returns>  
    public static DateTime GetWeekFirstDaySun(DateTime datetime)
    {
        //星期天为第一天  
        int weeknow = Convert.ToInt32(datetime.DayOfWeek);
        int daydiff = (-1) * weeknow;

        //本周第一天  
        string FirstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
        return Convert.ToDateTime(FirstDay);
    }

    /// <summary>  
    /// 得到本周第一天(以星期一为第一天)  
    /// </summary>  
    /// <param name="datetime"></param>  
    /// <returns></returns>  
    public static DateTime GetWeekFirstDayMon(DateTime datetime)
    {
        //星期一为第一天  
        int weeknow = Convert.ToInt32(datetime.DayOfWeek);

        //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。  
        weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
        int daydiff = (-1) * weeknow;

        //本周第一天  
        string FirstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
        return Convert.ToDateTime(FirstDay);
    }

    /// <summary>  
    /// 得到本周最后一天(以星期六为最后一天)  
    /// </summary>  
    /// <param name="datetime"></param>  
    /// <returns></returns>  
    public static DateTime GetWeekLastDaySat(DateTime datetime)
    {
        //星期六为最后一天  
        int weeknow = Convert.ToInt32(datetime.DayOfWeek);
        int daydiff = (7 - weeknow) - 1;

        //本周最后一天  
        string LastDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
        return Convert.ToDateTime(LastDay);
    }

    /// <summary>  
    /// 得到本周最后一天(以星期天为最后一天)  
    /// </summary>  
    /// <param name="datetime"></param>  
    /// <returns></returns>  
    public static DateTime GetWeekLastDaySun(DateTime datetime)
    {
        //星期天为最后一天  
        int weeknow = Convert.ToInt32(datetime.DayOfWeek);
        weeknow = (weeknow == 0 ? 7 : weeknow);
        int daydiff = (7 - weeknow);

        //本周最后一天  
        string LastDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
        return Convert.ToDateTime(LastDay);
    }  
}
