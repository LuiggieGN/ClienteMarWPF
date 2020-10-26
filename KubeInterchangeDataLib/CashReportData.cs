using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterchangeDataLib
{
    /// <summary>
    /// Class to rappresent info of tickets
    /// </summary>
    public class TicketSummaryInfo
    {
        /// <summary>
        /// Number of tickets
        /// </summary>
        public int Counter = 0;
        /// <summary>
        /// Total amount of the tickets
        /// </summary>
        public decimal Amount = 0;

        /// <summary>
        /// Override for ToString().
        /// </summary>
        /// <returns>Dump string</returns>
        public override string ToString()
        {
            string s = string.Empty;

            s += DumpHelper.DumpField("Counter", Counter);
            s += " ";
            s += DumpHelper.DumpField("Amount", Amount);
            return s;
        }
    }

    /// <summary>
    /// Class to rappresent data for the cash report
    /// </summary>
    public class CashReportData
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
        /// Cash at open.
        /// </summary>
        public decimal CashOpen = 0;
        /// <summary>
        /// Placed bets.
        /// </summary>
        public TicketSummaryInfo PlacedBet = new TicketSummaryInfo();
        /// <summary>
        /// Cancelled bets.
        /// </summary>
        public TicketSummaryInfo CancelledBet = new TicketSummaryInfo();
        /// <summary>
        /// Paid bets.
        /// </summary>
        public TicketSummaryInfo PaidBet = new TicketSummaryInfo();
        /// <summary>
        /// Cash at close
        /// </summary>
        public decimal CashClose = 0;


        /// <summary>
        /// Default constructor for empty receipt.
        /// </summary>
        public CashReportData()
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
            s += DumpHelper.DumpField("CashOpen", CashOpen);
            s += "\n";
            s += DumpHelper.DumpField("PlacedBet", PlacedBet.ToString());
            s += "\n";
            s += DumpHelper.DumpField("CancelledBet", CancelledBet.ToString());
            s += "\n";
            s += DumpHelper.DumpField("PaidBet", PaidBet.ToString());
            s += "\n";
            s += DumpHelper.DumpField("CashClose", CashClose);
            s += "\n";
            return s;
        }
    }
}
