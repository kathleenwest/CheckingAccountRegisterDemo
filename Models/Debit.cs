using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
namespace Models
{
    /// <summary>
    /// Describes Debit Transaction objects
    /// </summary>
    [XmlType(TypeName = "debit")]
    public class Debit : Transaction
    {
        /// <summary>
        /// Type of debit being applied to the account. These enum values can be used
        /// as the description for the transaction, unless the value is “Unknown”, in
        /// which case the user should type in a value.
        /// </summary>
        [XmlElement(ElementName = "debittype")]
        public DebitTypeEnum DebitType { get; set; }

        /// <summary>
        /// Check number, if applicable
        /// </summary>
        [XmlElement(ElementName = "checkno")]
        public int CheckNo { get; set; }

        /// <summary>
        /// Fee amount, if any, for this transaction
        /// </summary>
        [XmlElement(ElementName = "fee")]
        public decimal Fee { get; set; }

    } // end of class
} // end of namespace
