using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    /// <summary>
    /// Enumeration for Credit Type Transactions
    /// </summary>
    public enum CreditTypeEnum
    {
        Unknown = 0,
        Check = 1,
        Cash = 2,
        MoneyOrder = 3,
        Wire = 4,
        AutomaticDeposit = 5
    } // end of enum

} // end of namespace
