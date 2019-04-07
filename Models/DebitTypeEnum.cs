using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    /// <summary>
    /// Enumeration for Debit Type Transactions
    /// </summary>
    public enum DebitTypeEnum
    {
        Unknown = 0,
        Check = 1,
        Cash = 2,
        ATM = 3,
        AutomaticPayment = 4,
        debitCard = 5,
        FundsTransfer = 6

    } // end of enum

} // end of namespace
