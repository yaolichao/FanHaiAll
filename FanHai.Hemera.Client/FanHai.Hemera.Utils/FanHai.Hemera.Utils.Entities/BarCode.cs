/*
<FileInfo>
  <Author>Alfred.Liu, FanHai Hemera</Author>
  <Copyright><![CDATA[
    Copyright © 2011 FanHai. All rights reserved.
 * ]]></Copyright>
</FileInfo>
*/
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace FanHai.Hemera.Utils.Entities
{
    public class BarCode
    {
        public BarCode(string batteryCellCode)
        {
            this.batteryCellCode = batteryCellCode;
        }

        public BarCode(string batteryCellCode, string batteryModuleCode)
        {
            this.batteryCellCode = batteryCellCode;
            this.batteryModuleCode = batteryModuleCode;
        }
        //public BarCode(string batteryCellCode, string batteryQty)
        //{
        //    this.batteryCellCode = batteryCellCode;
        //    this.batteryQty = batteryQty;
        //}
        public string BatteryCellCode
        {
            get { return batteryCellCode; }
            set { batteryCellCode = value; }
        }

        public string BatteryModuleCode
        {
            get { return batteryModuleCode; }
            set { batteryModuleCode = value; }
        }
        public string BatteryQty
        {
            get { return batteryQty; }
            set { batteryQty = value; }
        }
        public string Line
        {
            get { return line; }
            set { line = value; }
        }
        public string OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; }
        }
        public string StepDesc
        {
            get { return stepDesc; }
            set { stepDesc = value; }
        }
        public string ExtendItem1
        {
            get { return extendItem1; }
            set { extendItem1 = value; }
        }
        public string ExtendItem2
        {
            get { return extendItem2; }
            set { extendItem2 = value; }
        }
        public string ExtendItem3
        {
            get { return extendItem3; }
            set { extendItem3 = value; }
        }
        public string ExtendItem4
        {
            get { return extendItem4; }
            set { extendItem4 = value; }
        }
        public string ExtendItem5
        {
            get { return extendItem5; }
            set { extendItem5 = value; }
        }

        string batteryCellCode = string.Empty;
        string batteryModuleCode = string.Empty;
        string batteryQty = string.Empty;
        string line = string.Empty;
        string orderNumber = string.Empty;

        string stepDesc = string.Empty;
        string extendItem1 = string.Empty;
        string extendItem2 = string.Empty;
        string extendItem3 = string.Empty;
        string extendItem4 = string.Empty;
        string extendItem5 = string.Empty;
       
    }
}
