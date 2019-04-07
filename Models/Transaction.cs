using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Models
{
    /// <summary>
    /// Describes a Transaction object
    /// </summary>
    [KnownType(typeof(Credit))]
    [KnownType(typeof(Debit))]
    [XmlInclude(typeof(Credit))]
    [XmlInclude(typeof(Debit))]
    [XmlType(TypeName = "transaction")]
    public class Transaction: IComparable<Transaction>, IComparer<Transaction>
    {
        #region Properties

        // Date of the transaction
        [XmlElement(ElementName = "date")]
        public DateTime Date { get; set; }

        // Transaction description
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        // Transaction amount(positive value for deposits, negative for withdrawals)
        [XmlElement(ElementName = "amount")]
        public decimal Amount { get; set; }

        #endregion Properties

        #region Implement Comparison Interfaces for Sorting

        /// <summary>
        /// CompareTo
        /// Describes how an object should be compared to another
        /// for the purposes of sorting objects in our lists
        /// The sort criteria should be:
        /// Date(in ascending order)
        /// Transaction Type(Credits first, then Debits)
        /// Amount(in ascending order)
        /// Description(in ascending order)
        /// </summary>
        /// <param name="other">(Transaction) object to compare to this instance object</param>
        /// <returns>-1 if this instance precedes other, 0 if equal, 1 if other precedes this instance</returns>
        public int CompareTo(Transaction other)
        {
            // Compare Null First
            if (this == null && other == null)
            {
                return 1;
            }
            else if (this == null)
            {
                return -1;
            }
            else if (other == null)
            {
                return 1;
            }
            else
            {
                // Sort by Date First, Dates, Must be in Ascending Order
                if (this.Date.CompareTo(other.Date) == 0)
                {
                    // We are Equal Continue to Compare by Transaction Type
                    // (Credits first, then Debits)

                    if((this is Credit) && (other is Debit))
                    {
                        return -1;

                    } // end of if

                    else if ((this is Debit) && (other is Credit))
                    {
                        return 1;

                    } // end of else if

                    // Both of the objects are equal
                    else
                    {
                        // Sort by Amount(in ascending order)
                        if (this.Amount.CompareTo(other.Amount) == 0)
                        {
                            // Both Amounts are Equal
                            // Sort by Description(in ascending order)
                            return this.Description.CompareTo(other.Description);

                        } // end of if
                        else
                        {
                            return this.Amount.CompareTo(other.Amount);

                        } // end of else
                    } // end of else               
                } // end of if
                else
                {
                    // Early date is prioritized
                    return this.Date.CompareTo(other.Date);
                } // end of else
            } // end of else

        } // end of method

        /// <summary>
        /// Compare
        /// Describes how an object should be compared to another
        /// for the purposes of sorting objects in our lists
        /// The sort criteria should be:
        /// Date(in ascending order)
        /// Transaction Type(Credits first, then Debits)
        /// Amount(in ascending order)
        /// Description(in ascending order)
        /// </summary>
        /// <param name="x">(Transaction) object to be compared to y</param>
        /// <param name="y">(Transaction) object to be compared from x</param>
        /// <returns>-1 if x precedes y, 0 if equal, 1 if y precedes x</returns>
        public int Compare(Transaction x, Transaction y)
        {
            // Shortcut here
            return x.CompareTo(y);
        } // end of method

        #endregion Implement Comparison Interfaces for Sorting

    } // end of class
} // end of namespace
