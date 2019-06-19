using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FanHai.Hemera.Share.Common
{
    /// <summary>
    /// 数学公式计算类。
    /// </summary>
    public class Calc
    {
        private string _token; /*全局变量*/
        private string _inputString = string.Empty;
        private IList<string> _inputStringList = new List<string>();
        private int _index = 0;

        /// <summary>
        /// 计算用字符串表示的数学公式。
        /// </summary>
        /// <param name="input">用字符串表示的数学公式。</param>
        /// <returns>计算结果。</returns>
        public double CalcMath(string input)
        {
            if (string.IsNullOrEmpty(input)) throw new ArgumentNullException("input", "输入的数学公式字符串不能为空");
            this._inputString = input.Trim();
            this._inputStringList = new List<string>();
            this._index = 0;
            double result;


            #region 字符串解释
            Regex reg = new Regex("\\+|\\-|\\*|/|\\(|\\)|\\[|\\]|\\{|\\}", RegexOptions.Singleline);
            string[] strList = reg.Split(this._inputString);
            MatchCollection mc = reg.Matches(this._inputString);
            if (mc.Count > 0)
            {
                for (int i = 0; i < strList.Length; i++)
                {
                    if (!string.IsNullOrEmpty(strList[i]))
                        this._inputStringList.Add(strList[i]);
                    if (i < mc.Count)
                    {
                        Match mat = mc[i];
                        if (mat.Success)
                        {
                            string s = mat.Value;
                            if (!string.IsNullOrEmpty(s))
                                this._inputStringList.Add(s);
                        }
                    }
                }
            }
            #endregion


            this._token = this.GetNext(); /*载入第一个单元*/
            result = this.Low(); /*进行计算*/
            return result;
        }

        /*对当前的标志进行匹配*/
        private void MatchMarker(string expectedToken)
        {
            if (this._token == expectedToken)
            {
                this._token = this.GetNext();/*匹配成功，获取下一个*/
            }
            else
            {
                throw new ArgumentException("运算符不匹配", "token");
            }
        }
        /*用于计算表达式中的+、-运算*/
        private double Low()
        {
            double result = this.Mid();
            while (this._token == "+" || this._token == "-")
                if (this._token == "+")
                {
                    this.MatchMarker("+");
                    result += this.Mid();
                }
                else if (this._token == "-")
                {
                    this.MatchMarker("-");
                    result -= this.Mid();
                }
            return result;
        }
        /*用于计算表达式中的*、/运算*/
        private double Mid()
        {
            double div;
            double result = this.High();
            while (this._token == "*" || this._token == "/")
                if (this._token == "*")
                {
                    this.MatchMarker("*");
                    div = this.High();
                    result *= div;
                }
                else if (this._token == "/")
                {
                    this.MatchMarker("/");
                    div = this.High();
                    if (div == 0) div = 1;
                    result /= div;
                }
            return result;
        }
        /*用于计算表达式中带{}的运算*/
        private double High()
        {
            double result = this.High2();
            if (this._token == "{")
            {
                this.MatchMarker("{");
                result = this.Low(); /*递归计算表达式*/
                this.MatchMarker("}");
            }
            return result;
        }
        /*用于计算表达式中带[]的运算*/
        private double High2()
        {
            double result = this.High3();
            if (this._token == "[")
            {
                this.MatchMarker("[");
                result = this.Low(); /*递归计算表达式*/
                this.MatchMarker("]");
            }
            return result;
        }
        /*用于计算表达式中带()的运算*/
        private double High3()
        {
            double result = this.MathInput();
            if (this._token == "(")
            {
                this.MatchMarker("(");
                result = this.Low(); /*递归计算表达式*/
                this.MatchMarker(")");
            }
            return result;
        }
        /*获取计算数值*/
        private double MathInput()
        {
            double result = 0;
            if (double.TryParse(this._token, out result))
            {
                this._token = this.GetNext();
            }
            return result;
        }

        private string GetNext()
        {
            string ret = string.Empty;
            if (this._index < this._inputStringList.Count)
            {
                ret = this._inputStringList[this._index];
                this._index++;
            }
            return ret;
        }
    }
}
