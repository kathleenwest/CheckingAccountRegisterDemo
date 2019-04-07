using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;
 
namespace Models
{
    /// <summary>
    /// Describes the TransactionList
    /// Enuemerable List of type Transaction objects
    /// objects of this type can hold items of type Transaction, Debit and Credit
    /// Represents the checking register as a whole
    /// Balance of the account is not stored as a physical entity. 
    /// Rather, it is calculated on the fly based on the contents of the TransactionList
    /// </summary>
    [XmlRoot(ElementName = "transactions")]
    public class TransactionList : List<Transaction>
    {

        #region Implement XML Serialization and Deserialization 

        /// <summary>
        /// Load
        /// Loads data fom an xml file into memory
        /// </summary>
        /// <param name="filename">(string) name of file to load xml</param>
        /// <returns></returns>
        public static TransactionList Load(string filename)
        {
            TransactionList list = new TransactionList();
            XmlSerializer ser = new XmlSerializer(typeof(Models.TransactionList));
            using (StreamReader reader = new StreamReader(filename))
            {
                if (reader != null)
                {
                    list = ser.Deserialize(reader) as Models.TransactionList;
                }
            }

            // Sort the list per requirements
            list.Sort();

            return list;

        } // end of Load method

        /// <summary>
        /// Save
        /// Saves and writes the data to XML file
        /// </summary>
        /// <param name="filename">(string) name of file to write data</param>
        public void Save(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(Models.TransactionList));
            using (StreamWriter writer = new StreamWriter(filename))
            {
                ser.Serialize(writer, this);
            }
        } // end of Save method

        #endregion Implement XML Serialization and Deserialization 

    } // end of class
} // end of namespace
