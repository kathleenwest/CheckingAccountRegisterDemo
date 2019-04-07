using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Models
{
    /// <summary>
    /// Class Describes a Credit Transaction
    /// </summary>
    [XmlType(TypeName = "credit")]
    public class Credit : Transaction
    {
        /// <summary>
        /// Type of credit being applied to the account. 
        /// These enum values can be  used as the description 
        /// for the transaction, unless the value is “Unknown”,
        /// in which case the user should type in a value.
        /// </summary>
        [XmlElement(ElementName = "credittype")]
        public CreditTypeEnum CreditType { get; set; }

    } // end of class
} // end of namespace
