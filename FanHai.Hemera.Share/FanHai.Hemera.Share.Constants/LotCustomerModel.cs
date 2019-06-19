using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    /// <summary>
    /// 批次工作站作业明细的参数类。
    /// </summary>
    [Serializable]
    public class LotCustomerModel
    {

        private string shiftKey;
        private string operationName;
        private string equipmentKey;
        private string equipmentName;
        private string lineKey;
        private string lineName;
        private string moduleGrade;
        private string customerType;
        private string virtualCustomerNumber;
        private string trayText;
        private string trayValue;
        private string lotNumber;
        private int packageNumber;
        private string color;
        private int number;
        private string workOrderNo;
        private string patrNumber;
        private string subPowerlevel;
        private string gradeName;
        private string psKey;
        private string isFlip;
        private string isPack;

        public string IsFlip
        {
            get { return isFlip; }
            set { isFlip = value; }
        }

        public string OperationName
        {
            get { return operationName; }
            set { operationName = value; }
        }

        public string EquipmentKey
        {
            get { return equipmentKey; }
            set { equipmentKey = value; }
        }

        public string EquipmentName
        {
            get { return equipmentName; }
            set { equipmentName = value; }
        }

        public string LineKey
        {
            get { return lineKey; }
            set { lineKey = value; }
        }

        public string LineName
        {
            get { return lineName; }
            set { lineName = value; }
        }

        public string ModuleGrade
        {
            get { return moduleGrade; }
            set { moduleGrade = value; }
        }

        public string CustomerType
        {
            get { return customerType; }
            set { customerType = value; }
        }

        public string VirtualCustomerNumber
        {
            get { return virtualCustomerNumber; }
            set { virtualCustomerNumber = value; }
        }

        public string TrayText
        {
            get { return trayText; }
            set { trayText = value; }
        }

        public string TrayValue
        {
            get { return trayValue; }
            set { trayValue = value; }
        }

        public string LotNumber
        {
            get { return lotNumber; }
            set { lotNumber = value; }
        }

        public int PackageNumber
        {
            get { return packageNumber; }
            set { packageNumber = value; }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        public string WorkOrderNo
        {
            get { return workOrderNo; }
            set { workOrderNo = value; }
        }

        public string PatrNumber
        {
            get { return patrNumber; }
            set { patrNumber = value; }
        }

        public string GradeName
        {
            get { return gradeName; }
            set { gradeName = value; }
        }

        public string PsKey
        {
            get { return psKey; }
            set { psKey = value; }
        }

        public string SubPowerlevel
        {
            get { return subPowerlevel; }
            set { subPowerlevel = value; }
        }

        public string ShiftKey
        {
            get { return shiftKey; }
            set { shiftKey = value; }
        }

        public string IsPack
        {
            get { return isPack; }
            set { isPack = value; }
        }
        /// <summary>
        /// 构造函数。
        /// </summary>
        public LotCustomerModel()
        {
        }
    }
}
