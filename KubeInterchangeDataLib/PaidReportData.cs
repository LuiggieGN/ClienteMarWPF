using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterchangeDataLib
{
    /// <summary>
    /// Class to rappresent info of tickets
    /// </summary>
    public class PaidReceiptInfo
    {
        /// <summary>
        /// Receipt ID
        /// </summary>
        public string ReceiptID = null;
        /// <summary>
        /// Paid amount
        /// </summary>
        public decimal PaidAmount = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        public PaidReceiptInfo(string id, decimal amount)
        {
            ReceiptID = id;
            PaidAmount = amount;
        }

        /// <summary>
        /// Override for ToString().
        /// </summary>
        /// <returns>Dump string</returns>
        public override string ToString()
        {
            string s = string.Empty;

            s += DumpHelper.DumpField("ReceiptID", ReceiptID);
            s += " ";
            s += DumpHelper.DumpField("PaidAmount", PaidAmount);
            return s;
        }
    }

    /// <summary>
    /// List of paid receipts.
    /// </summary>
    public class PaidReceiptList : List<PaidReceiptInfo>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PaidReceiptList()
            : base()
        {
        }

        /// <summary>
        /// Add a new element
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        public void Add(string id, decimal amount)
        {
            this.Add(new PaidReceiptInfo(id, amount));
        }

        /// <summary>
        /// Override for ToString().
        /// </summary>
        /// <returns>Dump string</returns>
        public override string ToString()
        {
            string s = string.Empty;
            foreach (PaidReceiptInfo info in this)
            {
                s += info.ToString();
                s += "\n";
            }
            return s;
        }
    }
    
    public class PaidReportData
    {
        /// <summary>
        /// Print date time.
        /// </summary>
        public DateTime PrintDate = DateTime.MinValue;
        /// <summary>
        /// Name of the terminal operator.
        /// </summary>
        public string UserName = null;
        /// <summary>
        /// Terminal ID.
        /// </summary>
        public string TerminalID = null;
        /// <summary>
        /// Shop ID.
        /// </summary>
        public string ShopID = null;
        /// <summary>
        /// Paid receipt list
        /// </summary>
        public PaidReceiptList PaidList = new PaidReceiptList();
        /// <summary>
        /// Total paid
        /// </summary>
        public decimal TotalPaid = 0;


        /// <summary>
        /// Default constructor for empty receipt.
        /// </summary>
        public PaidReportData()
        {
        }

        /// <summary>
        /// Override for ToString().
        /// </summary>
        /// <returns>Dump string</returns>
        public override string ToString()
        {
            string s = string.Empty;

            s += DumpHelper.DumpField("PrintDate", PrintDate);
            s += " ";
            s += DumpHelper.DumpField("UserName", UserName);
            s += " ";
            s += DumpHelper.DumpField("TerminalID", TerminalID);
            s += "\n";
            s += DumpHelper.DumpField("ShopID", ShopID);
            s += " ";
            s += DumpHelper.DumpField("TotalPaid", TotalPaid);
            s += " ";
            s += DumpHelper.DumpField("PaidList", PaidList.ToString());
            return s;
        }
    }
}
