using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace InterchangeDataLib
{
    /// <summary>
    /// Class to rappresent a receipt. Receipt contains a Header with more details (legs).
    /// </summary>
    public class ReceiptData
    {
        /// <summary>
        /// Receipt ID (and barcode).
        /// </summary>
        public string ReceiptID = null;
        /// <summary>
        /// Total stake.
        /// </summary>
        public decimal Amount = 0;
        /// <summary>
        /// Final price.
        /// </summary>
        public decimal FinalPrice = 0;
        /// <summary>
        /// Possible winning.
        /// </summary>
        public decimal PossibleReturn = 0;
        /// <summary>
        /// Bet place date.
        /// </summary>
        public DateTime PlaceDate = DateTime.MinValue;
        /// <summary>
        /// Company info printed in the receipt.
        /// </summary>
        //public string CompanyInfo = null;
        /// <summary>
        /// Name of the terminal operator.
        /// </summary>
        public string UserName = null;
        /// <summary>
        /// Terminal ID.
        /// </summary>
        public string TerminalID = null;
        /// <summary>
        /// Bet type (single / double / etc).
        /// </summary>
        public string BetType = null;
        /// <summary>
        /// Bet legs.
        /// </summary>
        public ReceiptLegDataList Legs = null;
        /// <summary>
        /// Company logo in. 
        /// </summary>
        //public byte[] LogoRawImage = null;

        /// <summary>
        /// Default constructor for empty receipt.
        /// </summary>
        public ReceiptData()
        {
        }

        /// <summary>
        /// Number of legs in the receipt.
        /// </summary>
        public int LegsCount
        {
            get
            {
                return (Legs == null) ? 0 : Legs.Count;
            }
        }

        /// <summary>
        /// Add a leg to the receipt.
        /// </summary>
        /// <param name="leg"></param>
        public void AddLeg(ReceiptLegData leg)
        {
            if (Legs == null)
            {
                Legs = new ReceiptLegDataList();
            }
            Legs.Add(leg);
        }

        /// <summary>
        /// Create and add a leg to the receipt.
        /// </summary>
        /// <param name="dtEvent"></param>
        /// <param name="sEventDescription"></param>
        /// <param name="sMarketDescription"></param>
        /// <param name="sSelectionDescription"></param>
        /// <param name="dPrice"></param>
        public void AddLeg(DateTime dtEvent, string sEventDescription, string sMarketDescription, string sSelectionDescription, decimal dPrice)
        {
            ReceiptLegData leg = new ReceiptLegData(dtEvent, sEventDescription, sMarketDescription, sSelectionDescription, dPrice);
            AddLeg(leg);
        }

        /// <summary>
        /// Override for ToString().
        /// </summary>
        /// <returns>Dump string</returns>
        public override string ToString()
        {
            string s = string.Empty;

            s += "Header:";
            s += " ";
            s += DumpHelper.DumpField("ReceiptID", ReceiptID);
            s += " ";
            s += DumpHelper.DumpField("UserName", UserName);
            s += " ";
            s += DumpHelper.DumpField("TerminalID", TerminalID);
            s += " ";
            s += DumpHelper.DumpField("BetType", BetType);
            s += "\n";
            s += DumpHelper.DumpField("Amount", Amount);
            s += " ";
            s += DumpHelper.DumpField("FinalPrice", FinalPrice);
            s += " ";
            s += DumpHelper.DumpField("PossibleReturn", PossibleReturn);
            s += "\n";
            s += DumpHelper.DumpField("LegsCount", LegsCount);
            s += "\n";
            foreach (ReceiptLegData leg in Legs)
            {
                s += leg.ToString();
            }
            return s;
        }
    }

    /// <summary>
    /// Class to rappresent a receipt leg (detail).
    /// </summary>
    public class ReceiptLegData
    {
        /// <summary>
        /// When the event will start
        /// </summary>
        public DateTime EventDate = DateTime.MinValue;
        /// <summary>
        /// Description of the event
        /// </summary>
        public string EventDescription = null;
        /// <summary>
        /// Type of bet (for example 1X2, Odd/Even, etc)
        /// </summary>
        public string MarketDescription = null;
        /// <summary>
        /// Type of bet (for example 1 or X or 2 etc)
        /// </summary>
        public string SelectionDescription = null;
        /// <summary>
        /// Price (Odds) in decimal format.
        /// </summary>
        public decimal Price = 0;

        /// <summary>
        /// Default constructor for empty leg
        /// </summary>
        public ReceiptLegData()
        {
        }

        /// <summary>
        /// Constructor for a fill leg
        /// </summary>
        /// <param name="dtEvent"></param>
        /// <param name="sEventDescription"></param>
        /// <param name="sMarketDescription"></param>
        /// <param name="sSelectionDescription"></param>
        /// <param name="dPrice"></param>
        public ReceiptLegData(DateTime dtEvent, string sEventDescription, string sMarketDescription, string sSelectionDescription, decimal dPrice)
        {
            EventDate = dtEvent;
            EventDescription = sEventDescription;
            MarketDescription = sMarketDescription;
            SelectionDescription = sSelectionDescription;
            Price = dPrice;
        }

        /// <summary>
        /// Override for ToString().
        /// </summary>
        /// <returns>Dump string</returns>
        public override string ToString()
        {
            string s = string.Empty;

            s += "Leg:";
            s += " ";
            s += DumpHelper.DumpField("date", EventDate);
            s += " ";
            s += DumpHelper.DumpField("event", EventDescription);
            s += " ";
            s += DumpHelper.DumpField("market", MarketDescription);
            s += " ";
            s += DumpHelper.DumpField("selection", SelectionDescription);
            s += " ";
            s += DumpHelper.DumpField("price", Price);
            s += "\n";
            return s;
        }
    }

    /// <summary>
    /// Class for a List of ReceiptLegData
    /// </summary>
    public class ReceiptLegDataList : List<ReceiptLegData>
    {
    }
}
