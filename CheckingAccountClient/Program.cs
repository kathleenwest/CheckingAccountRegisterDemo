using System;
using System.Net;
using Utilities;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Net.Http.Headers;

namespace CheckingAccountClient
{
    /// <summary>
    /// Client Console Program
    /// User interface to query and test a 
    /// web api service called "Transactions".
    /// </summary>
	public class Program
	{
        // Base Address for the API and controller
        const string BASE_ADDR = "http://localhost:18437/api/Transactions/";

        /// <summary>
        /// Main
        /// Main entry method to the program.
        /// Contains user menu of options.
        /// </summary>
        /// <param name="args">No arguments used</param>
        static void Main(string[] args)
		{
			MenuOptionsEnum choice = MenuOptionsEnum.Quit;
			do
			{
				choice = ConsoleHelpers.ReadEnum<MenuOptionsEnum>("Option: ");
				Console.Clear();
				
				if (choice != MenuOptionsEnum.Quit)
				{
					DisplayOptionTitle(choice);
				}

				switch (choice)
				{
					case MenuOptionsEnum.GetAllTransactions:
						GetAllTransactions();
						break;
					case MenuOptionsEnum.GetCreditsByType:
						GetCreditsByType();
						break;
					case MenuOptionsEnum.GetDebitsByType:
						GetDebitsByType();
						break;
					case MenuOptionsEnum.GetTransactionsByDateRange:
						GetTransactionsByDateRange();
						break;
					case MenuOptionsEnum.AddCredit:
						AddCredit();
						break;
					case MenuOptionsEnum.AddDebit:
						AddDebit();
						break;
				}

				Console.WriteLine();
				Console.Write("Press <ENTER> to continue...");
				Console.ReadLine();
				Console.Clear();

			} while (choice != MenuOptionsEnum.Quit);
		}

        /// <summary>
        /// AddDebit
        /// Processes the client's request to add a Debit
        /// type transaction object to the backend database.
        /// Prompts user for information to build the debit
        /// transaction object.
        /// </summary>
		private static void AddDebit()
		{
			try
			{
                Debit newDebit = new Debit()
                {
                    Amount = 2.5M,
                    DebitType = DebitTypeEnum.Cash,
                    Date = DateTime.Now,
                    Description = "Need some money",
                    CheckNo = 1005,
                    Fee = 1M                
                };

                // Get user input
                newDebit.Amount = ConsoleHelpers.ReadDecimal("Enter Amount: ", 0.00M, 99999.99M);
                newDebit.DebitType = ConsoleHelpers.ReadEnum<DebitTypeEnum>("Enter Debit Type: ");
                newDebit.Date = ConsoleHelpers.ReadDate("Enter Date: ",DateTime.MinValue, DateTime.MaxValue);
                newDebit.Description = ConsoleHelpers.ReadString("Enter Description: ", 0, 20);
                newDebit.CheckNo = ConsoleHelpers.ReadInt("Enter Check Number: ",0,9999);
                newDebit.Fee = ConsoleHelpers.ReadDecimal("Enter the Fee Amount: ", 0.00M, 999.99M);
                
                // Call the API
                var resultDebitAdd = ClientHelper.Post<Debit, Debit>(BASE_ADDR, Utilities.SerializationModesEnum.Xml, newDebit, "AddDebit");

                if (resultDebitAdd == null)
                {
                    Console.WriteLine("Response was null");
                }
                else if (resultDebitAdd.StatusCode == HttpStatusCode.UnsupportedMediaType)
                {
                    Console.WriteLine("No Debit Object was sent to add");
                }
                else if (!((int)resultDebitAdd.StatusCode >= 200 && (int)resultDebitAdd.StatusCode < 300))
                {
                    Console.WriteLine("Error encountered: {0}", resultDebitAdd.Error);
                }
                else
                {
                    Console.WriteLine("Added new Debit Transaction.");
                }
            }
			catch (Exception ex)
			{
				System.Reflection.MethodBase mb = System.Reflection.MethodBase.GetCurrentMethod();
				System.Diagnostics.Debug.WriteLine(ex.Message, string.Format("{0}.{1}.{2}", mb.DeclaringType.Namespace, mb.DeclaringType.Name, mb.Name));
			}
		} // end of debit

        /// <summary>
        /// AddCredit
        /// Processes the client's request to add a Credit
        /// type transaction object to the backend database.
        /// Prompts user for information to build the credit
        /// transaction object.
        /// </summary>
		private static void AddCredit()
		{
			try
			{
                Credit newCredit = new Credit()
                {
                    Amount = 1.5M,
                    CreditType = CreditTypeEnum.Cash,
                    Date = DateTime.Now,
                    Description = "Got some money"
                };

                // Get user input               
                newCredit.Amount = ConsoleHelpers.ReadDecimal("Enter Amount: ", 0.00M, 99999.99M);
                newCredit.CreditType = ConsoleHelpers.ReadEnum<CreditTypeEnum>("Enter Credit Type: ");
                newCredit.Date = ConsoleHelpers.ReadDate("Enter Date: ",DateTime.MinValue, DateTime.MaxValue);
                newCredit.Description = ConsoleHelpers.ReadString("Enter Description: ", 0, 20);
                
                var resultCreditAdd = ClientHelper.Post<Credit, Credit>(BASE_ADDR, Utilities.SerializationModesEnum.Xml, newCredit, "AddCredit");

                if (resultCreditAdd == null)
                {
                    Console.WriteLine("Response was null");
                }
                else if (resultCreditAdd.StatusCode == HttpStatusCode.UnsupportedMediaType)
                {
                    Console.WriteLine("No Credit Object was sent to add");
                }
                else if (!((int)resultCreditAdd.StatusCode >= 200 && (int)resultCreditAdd.StatusCode < 300))
                {
                    Console.WriteLine("Error encountered: {0}", resultCreditAdd.Error);
                }
                else
                {
                    Console.WriteLine("Added new Credit Transaction.");
                }

            } // end of try
            catch (Exception ex)
			{
				System.Reflection.MethodBase mb = System.Reflection.MethodBase.GetCurrentMethod();
				System.Diagnostics.Debug.WriteLine(ex.Message, string.Format("{0}.{1}.{2}", mb.DeclaringType.Namespace, mb.DeclaringType.Name, mb.Name));
			}
		} // end of AddCredit

        /// <summary>
        /// GetTransactionsByDateRange
        /// Processes the client's request to get all transaction
        /// within a specified date range. Asks the user to
        /// specify a start and end date range for the query
        /// </summary>
		private static void GetTransactionsByDateRange()
		{
			try
			{
                DateTime start = ConsoleHelpers.ReadDate("Enter Start Date: ", DateTime.MinValue, DateTime.MaxValue);
                DateTime end = ConsoleHelpers.ReadDate("Enter End Date: ", DateTime.MinValue, DateTime.MaxValue);
                Console.WriteLine("Your Query: Start: {0} End: {1}", start, end);

                var resultDateTime = ClientHelper.Get<TransactionList>(BASE_ADDR, Utilities.SerializationModesEnum.Xml, "GetTransactionsByDateRange?start={0}&end={1}", start, end);

                if (resultDateTime == null)
                {
                    Console.WriteLine("Response was null");
                }
                else if(resultDateTime.Result == null)
                {
                    // Query was successful but no items met the query
                    Console.WriteLine("Your Query has no transactions that meet your criteria.");
                }
                else if (!((int)resultDateTime.StatusCode >= 200 && (int)resultDateTime.StatusCode < 300))
                {
                    Console.WriteLine("Error encountered: {0}", resultDateTime.Error);
                }
                else
                {
                    PrintList(resultDateTime.Result);
                }
            } // end of try
			catch (Exception ex)
			{
				System.Reflection.MethodBase mb = System.Reflection.MethodBase.GetCurrentMethod();
				System.Diagnostics.Debug.WriteLine(ex.Message, string.Format("{0}.{1}.{2}", mb.DeclaringType.Namespace, mb.DeclaringType.Name, mb.Name));
			} // end of catch
		} // end of method

        /// <summary>
        /// GetDebitsByType
        /// Processes the client's request to Get
        /// all transactions by a Debit Type Enum.
        /// Asks user for the Debit Type enum to query
        /// </summary>
		private static void GetDebitsByType()
		{
			try
			{
                DebitTypeEnum debittype = ConsoleHelpers.ReadEnum<DebitTypeEnum>("Choose the Debit Type: ");
                Console.WriteLine("Debit Type to Query  {0}", debittype);

                var resultDebitType = ClientHelper.Get<TransactionList>(BASE_ADDR, Utilities.SerializationModesEnum.Xml, "GetDebitsByType?debitType={0}", debittype);

                if (resultDebitType == null)
                {
                    Console.WriteLine("Response was null");
                }
                else if (resultDebitType.Result == null)
                {
                    // Query was successful but no items met the query
                    Console.WriteLine("Your query has no transactions that meet your criteria.");
                }
                else if (!((int)resultDebitType.StatusCode >= 200 && (int)resultDebitType.StatusCode < 300))
                {
                    Console.WriteLine("Error encountered: {0}", resultDebitType.Error);
                }
                else
                {
                    PrintList(resultDebitType.Result);
                }
            } // end of try
			catch (Exception ex)
			{
				System.Reflection.MethodBase mb = System.Reflection.MethodBase.GetCurrentMethod();
				System.Diagnostics.Debug.WriteLine(ex.Message, string.Format("{0}.{1}.{2}", mb.DeclaringType.Namespace, mb.DeclaringType.Name, mb.Name));
			} // end of catch
		} // end of method

        /// <summary>
        /// GetCreditsByType
        /// Processes the client's request to Get
        /// all transactions by a Credit Type Enum.
        /// Asks user for the Credit Type enum to query
        /// </summary>
		private static void GetCreditsByType()
		{
			try
			{

                CreditTypeEnum credittype = ConsoleHelpers.ReadEnum<CreditTypeEnum>("Choose the Credit Type: ");
                Console.WriteLine("Credit Type to Query  {0}", credittype);

                var resultCreditType = ClientHelper.Get<TransactionList>(BASE_ADDR, Utilities.SerializationModesEnum.Xml, "GetCreditsByType?creditType={0}", credittype);

                if (resultCreditType == null)
                {
                    Console.WriteLine("Response was null.");
                }
                else if (resultCreditType.Result == null)
                {
                    // Query was successful but no items met the query
                    Console.WriteLine("Your query has no transactions that meet your criteria.");
                }
                else if (!((int)resultCreditType.StatusCode >= 200 && (int)resultCreditType.StatusCode < 300))
                {
                    Console.WriteLine("Error encountered: {0}", resultCreditType.Error);
                }
                else
                {
                    PrintList(resultCreditType.Result);
                }
            } // end of try
			catch (Exception ex)
			{
				System.Reflection.MethodBase mb = System.Reflection.MethodBase.GetCurrentMethod();
				System.Diagnostics.Debug.WriteLine(ex.Message, string.Format("{0}.{1}.{2}", mb.DeclaringType.Namespace, mb.DeclaringType.Name, mb.Name));
			} // end of catch
		} // end of method

        /// <summary>
        /// GetAllTransactions
        /// Processes the client request to get all the
        /// transactions available through the service
        /// </summary>
		private static void GetAllTransactions()
		{
			try
			{
                var result = ClientHelper.Get<TransactionList>(BASE_ADDR, Utilities.SerializationModesEnum.Xml, "GetAllTransactions");

                if (result == null)
                {
                    Console.WriteLine("Response was null.");
                }
                else if (result.Result == null)
                {
                    // Query was successful but no items met the query
                    Console.WriteLine("Your query has no transactions that meet your criteria.");
                }
                else if (!((int)result.StatusCode >= 200 && (int)result.StatusCode < 300))
                {
                    Console.WriteLine("Error encountered: {0}", result.Error);
                }
                else
                {
                    PrintList(result.Result);
                }

            } // end of try
			catch (Exception ex)
			{
				System.Reflection.MethodBase mb = System.Reflection.MethodBase.GetCurrentMethod();
				System.Diagnostics.Debug.WriteLine(ex.Message, string.Format("{0}.{1}.{2}", mb.DeclaringType.Namespace, mb.DeclaringType.Name, mb.Name));
            } // end of catch
		} // end of method

		/// <summary>
        /// DisplayOptionTitle
		/// Displays the screen title for the given menu choice
		/// </summary>
		/// <param name="choice">Menu choice</param>
		private static void DisplayOptionTitle(MenuOptionsEnum choice)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(choice.WordBreakMixedCase());
			ConsoleHelpers.WriteBorder('=', choice.WordBreakMixedCase().Length);
			Console.WriteLine();
			Console.ResetColor();
		}

        /// <summary>
        /// PrintList
        /// Prints the list as either a Credit or Debit
        /// object with its properties. 
        /// </summary>
        /// <param name="myList">(TransactionList) list of Credit and Debit objects</param>
        public static void PrintList(TransactionList myList)
        {
            Console.WriteLine("+ Credits, - Debits");
            Console.WriteLine(new string('=', 80));
            Console.WriteLine("  {0,-24} | {1,-20} | {2,10} | {3,-20}", "Date", "Description", "Amount", "Type");
            Console.WriteLine(new string('=', 80));
            foreach (Transaction item in myList)
            {
                // Credit Transaction
                if (item is Credit)
                {
                    Credit credit = item as Credit;
                    Console.WriteLine("+ {0:MM/dd/yyyy @ hh:mm:ss tt} | {1,-20} | {2,10:C2} | {3,-20} ", credit.Date, credit.Description, credit.Amount, credit.CreditType);
                    Console.WriteLine(new string('_', 80));
                }
                // Debit Transaction
                else if (item is Debit)
                {
                    Debit debit = item as Debit;
                    Console.WriteLine("- {0:MM/dd/yyyy @ hh:mm:ss tt} | {1,-20} | {2,10:C2} | {3,-20} ", debit.Date, debit.Description, debit.Amount, debit.DebitType);
                    Console.WriteLine("  {0, 8} {1,-4} {2,4} {3,-10:C2}", "CheckNo:", debit.CheckNo, "Fee:",debit.Fee);
                    Console.WriteLine(new string('_', 80));
                }
                else
                {
                    // General Transaction
                    Console.WriteLine("General Transaction, no credit or debit. Bad record.");
                    Console.WriteLine(new string('_', 80));
                }

            } // end of foreach

        } // end of method

    } // end of class
} // end of namespace
