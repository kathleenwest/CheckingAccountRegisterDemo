using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Models;
using System.Web;

namespace CheckingAccountDemo.Controllers
{
    /// <summary>
    /// Transactions Controller
    /// Web API for Transactions Service
    /// </summary>
    public class TransactionsController : ApiController
    {
        /// <summary>
        /// Stores filepath where server stores data locally
        /// </summary>
        private string FilePath
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/App_Data/transactions.xml");
            }
        }
             
        /// <summary>
        /// Adds a new Debit object to the checking register
        /// </summary>
        /// <param name="debit">(Debit) debit object to add</param>
        [HttpPost()]
        public Debit AddDebit([FromBody]Debit debit)
        {
            TransactionList data = TransactionList.Load(FilePath);

            if (debit == null)
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            else
            {
                data.Add(debit);
                data.Save(FilePath);
            }

            // Return the Debit object
            return debit;

        } // end of method
              
        /// <summary>
        /// Adds a new Credit object to the checking register
        /// </summary>
        /// <param name="credit">(Credit) credit object to add</param>
        [HttpPost()]
        public Credit AddCredit([FromBody]Credit credit)
        {
            TransactionList data = TransactionList.Load(FilePath);

            if (credit == null)
            {              
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            else
            {
                data.Add(credit);
                data.Save(FilePath);
            }

            // Return the object that was passed
            return credit;

        } // end of method

        /// <summary>
        /// Returns a list of all transactions in the register 
        /// in sorted order
        /// </summary>
        /// <returns>(TransactionList) a Transaction List type object 
        /// (list that holds transaction objects)</returns>
        public TransactionList GetAllTransactions()
        {
            TransactionList data = TransactionList.Load(FilePath);

            // Check to see if we have results
            if (data.Count > 0)
            {
                // Sort the Results
                data.Sort();
                // Return the Results
                return data;
            }
            else
            {
                return null;
                //throw new HttpResponseException(HttpStatusCode.NoContent);
            }

        } // end of method

        /// <summary>
        /// Gets a list of transactions that are between the given 
        /// dates(inclusively). Items should be sorted according 
        /// to defined rules.
        /// </summary>
        /// <param name="start">(DateTime) start time period</param>
        /// <param name="end">(DateTime) end time period</param>
        /// <returns>(TransactionList) a Transaction List type object 
        /// (list that holds transaction objects)</returns>
        public TransactionList GetTransactionsByDateRange(DateTime start, DateTime end)
        {
            TransactionList data = TransactionList.Load(FilePath);

            // Pick items from the list that meet the criteria
            IEnumerable<Transaction> dataPicked = data.Where(s => (s.Date >= start) && (s.Date <= end));

            // make a new list for output
            TransactionList dataResults = new TransactionList();

            // Put the enuerable items in a Transaction List
            // The casting to TransactionList did not work
            foreach(Transaction item in dataPicked)
            {
                dataResults.Add(item);
            }
          
            // Check to see if we have results
            if (dataResults.Count > 0)
            {
                // Sort the Results
                dataResults.Sort();
                // Return the Results
                return dataResults;
            }
            else
            {
                return null;
                //throw new HttpResponseException(HttpStatusCode.NoContent);
            }

        } // end of method

        /// <summary>
        /// Gets a list of credit transactions that are of the given type.
        /// Items should be sorted according to defined rules.
        /// </summary>
        /// <param name="creditType">(CreditTypeEnum) type of credit transaction</param>
        /// <returns>(TransactionList) a Transaction List type object 
        /// (list that holds transaction objects)</returns>
        public TransactionList GetCreditsByType(CreditTypeEnum creditType)
        {
            TransactionList data = TransactionList.Load(FilePath);

            // Pick items from the list that meet the criteria of being a Credit
            IEnumerable<Transaction> dataCredits = data.Where(s => (s is Credit));

            // Pick items from the list that meet the criteria of matching creditType
            IEnumerable<Transaction> dataPicked = dataCredits.Where(s => (s as Credit).CreditType == creditType);

            // make a new list for output
            TransactionList dataResults = new TransactionList();

            // Put the enuerable items in a Transaction List
            // The casting to TransactionList did not work
            foreach (Transaction item in dataPicked)
            {
                dataResults.Add(item);
            }

            // Check to see if we have results
            if (dataResults.Count > 0)
            {
                // Sort the Results
                dataResults.Sort();
                // Return the Results
                return dataResults;
            }
            else
            {
                return null;
                //throw new HttpResponseException(HttpStatusCode.NoContent);
            }

        } // end of method

        /// <summary>
        /// Gets a list of debit transactions that are of the given type.
        /// Items should be sorted according to defined rules.
        /// </summary>
        /// <param name="debitType">(DebitTypeEnum) type of debit transaction</param>
        /// <returns>(TransactionList) a Transaction List type object 
        /// (list that holds transaction objects)</returns>
        public TransactionList GetDebitsByType(DebitTypeEnum debitType)
        {
            TransactionList data = TransactionList.Load(FilePath);

            // Pick items from the list that meet the criteria of being a Debit
            IEnumerable<Transaction> dataDebits = data.Where(s => (s is Debit));

            // Pick items from the list that meet the criteria of matching debitType
            IEnumerable<Transaction> dataPicked = dataDebits.Where(s => (s as Debit).DebitType == debitType);

            // make a new list for output
            TransactionList dataResults = new TransactionList();

            // Put the enuerable items in a Transaction List
            // The casting to TransactionList did not work
            foreach (Transaction item in dataPicked)
            {
                dataResults.Add(item);
            }

            // Check to see if we have results
            if (dataResults.Count > 0)
            {
                // Sort the Results
                dataResults.Sort();
                // Return the Results
                return dataResults;
            }
            else
            {
                return null;
                //throw new HttpResponseException(HttpStatusCode.NoContent);
            }

        } // end of method

    } // end of class
} // end of namespace
