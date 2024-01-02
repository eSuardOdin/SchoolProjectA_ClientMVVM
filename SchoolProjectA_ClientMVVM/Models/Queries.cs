using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Principal;
namespace SchoolProjectA_ClientMVVM.Models
{
    public static class Queries
    {


        /*
         * 
         * MONIS
         * 
         */
        /// <summary>
        /// Trie les monis pour voir s'il en existe un avec le login du champ de login
        /// </summary>
        /// <param name="loginTxt">La textBox à checker</param>
        /// <returns>Un moni, null si le pseudo n'existe pas</returns>
        public static async Task<Moni> GetMoni(string loginTxt)
        {
            using HttpClient client = new HttpClient();
            Moni moni = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage res = await client.GetAsync("http://raspberry:5000/moni"); //http://192.168.30.10:5000/moni
                if (res.IsSuccessStatusCode)
                {
                    List<Moni> monis = await res.Content.ReadFromJsonAsync<List<Moni>>();
                    moni = monis.Find(x => x.MoniLogin == loginTxt);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return moni;
        }

        /// <summary>
        /// Check Moni password against password parameter
        /// </summary>
        /// <param name="moni">Moni to check</param>
        /// <returns>The Moni if check ok</returns>
        public static async Task<Moni> CheckMoni(Moni moni, string pwd)
        {
            using HttpClient client = new HttpClient();
            Moni? checkedMoni = null;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage res = await client.GetAsync($"http://raspberry:5000/moni/{moni.MoniId}?moniPwd={pwd}");
                if (res.IsSuccessStatusCode)
                {
                    //List<Moni> monis = await res.Content.ReadFromJsonAsync<List<Moni>>();
                    checkedMoni = await res.Content.ReadFromJsonAsync<Moni>();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return checkedMoni;
        }

        /// <summary>
        /// Moni POST
        /// </summary>
        /// <param name="moni">Moni to post to API</param>
        /// <returns>POSTed Moni (null if error)</returns>
        public static async Task<Moni> PostMoni(Moni moni)
        {
            using HttpClient client = new HttpClient();
            try
            {
                string jsonData = JsonConvert.SerializeObject(moni);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // POST
                HttpResponseMessage res = await client.PostAsync("http://raspberry:5000/moni", content);

                if (res.IsSuccessStatusCode)
                {
                    return moni;
                }
                else
                {
                    var errorResponse = await res.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(errorResponse);
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return null;
        }


        

        /*
         * 
         * BANK ACCOUNTS
         * 
         */

        /// <summary>
        /// GET all Moni Accounts
        /// </summary>
        /// <param name="id">Moni Id</param>
        /// <returns>The accounts or null</returns>
        public static async Task<List<BankAccount>> GetMoniAccounts(int id)
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage res = await client.GetAsync($"http://raspberry:5000/moni/{id}/accounts");
                if (res.IsSuccessStatusCode)
                {
                    List<BankAccount> accounts = await res.Content.ReadFromJsonAsync<List<BankAccount>>();
                    return accounts;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return null;
        }


        /// <summary>
        /// Post a bank account in order to get the api to insert it in DB
        /// </summary>
        /// <param name="account">The account to insert</param>
        /// <returns>Account inserted or null if error</returns>
        public static async Task<BankAccount> PostBankAccount(BankAccount account)
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                string accountData = JsonConvert.SerializeObject(account);
                var content = new StringContent(accountData, Encoding.UTF8, "application/json");
                HttpResponseMessage res = await client.PostAsync($"http://raspberry:5000/account", content);
                if (res.IsSuccessStatusCode)
                {
                    string responseData = await res.Content.ReadAsStringAsync();
                    BankAccount createdAccount = JsonConvert.DeserializeObject<BankAccount>(responseData);
                    return createdAccount;
                }
                else
                {
                    var errorResponse = await res.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(errorResponse);
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return null;
        }


        /// <summary>
        /// Delete a bank account
        /// </summary>
        /// <param name="accountId">The if of account to delete</param>
        /// <returns>The deleted account</returns>
        public static async Task<string> DeleteAccount(int accountId)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpResponseMessage res = await client.DeleteAsync($"http://raspberry:5000/account/{accountId}");
                if (res.IsSuccessStatusCode)
                {
                    //BankAccount deletedAccount = await res.Content.ReadFromJsonAsync<BankAccount>();
                    var response = await res.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(response);
                    return response;
                }
                else
                {
                    var errorResponse = await res.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(errorResponse);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return null;
        }
        /*
         * 
         * TRANSACTIONS
         * 
         */

        public static async Task<List<Transaction>> GetAccountTransactions(int accountId)
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage res = await client.GetAsync($"http://raspberry:5000/account/{accountId}/transactions");
                if (res.IsSuccessStatusCode)
                {
                    List<Transaction> transactions = await res.Content.ReadFromJsonAsync<List<Transaction>>();
                    return transactions;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return null;
        }

        /// <summary>
        /// Post a transaction in database
        /// </summary>
        /// <param name="transac"></param>
        /// <returns></returns>
        public static async Task<Transaction> PostTransaction(TransactionDTO transac)
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                string transacData = JsonConvert.SerializeObject(transac);
                var content = new StringContent(transacData, Encoding.UTF8, "application/json");
                HttpResponseMessage res = await client.PostAsync($"http://raspberry:5000/transaction", content);
                if (res.IsSuccessStatusCode)
                {
                    string responseData = await res.Content.ReadAsStringAsync();
                    Transaction createdTransac = JsonConvert.DeserializeObject<Transaction>(responseData);
                    return createdTransac;
                }
                else
                {
                    var errorResponse = await res.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(errorResponse);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return null;
        }


        /// <summary>
        /// Delete transaction from database
        /// </summary>
        /// <param name="transacId">Id of the transaction</param>
        /// <returns>The response or null if failed</returns>
        public static async Task<string> DeleteTransaction(int transacId)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpResponseMessage res = await client.DeleteAsync($"http://raspberry:5000/transaction/DeleteTransaction?transactionId={transacId}");
                if (res.IsSuccessStatusCode)
                {
                    var response = await res.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(response);
                    return response;
                }
                else
                {
                    var errorResponse = await res.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine(errorResponse);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return null;
        }



        /*
         * 
         * TAGS
         * 
         */

        public static async Task<List<Tag>> GetMoniTags(int id)
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage res = await client.GetAsync($"http://raspberry:5000/moni/{id}/tags");
                if (res.IsSuccessStatusCode)
                {
                    List<Tag> tags = await res.Content.ReadFromJsonAsync<List<Tag>>();
                    return tags;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return null;
        }

        /*public static async Task<List<Transaction>> GetTagTransactions(int moniId, int tagId)
        {
            List<Transaction> transactionsTagged = new List<Transaction>();
            // Let's go for awfulness
            // Get all moni accounts
            List<BankAccount>? bankAccounts = await Queries.GetMoniAccounts(moniId);
            if(bankAccounts != null)
            {
                // Check all accounts transaction
                foreach(BankAccount bankAccount in bankAccounts)
                {
                    List<Transaction>? transactions = await Queries.GetAccountTransactions(bankAccount.BankAccountId);
                    if (transactions != null)
                    {
                        foreach(Transaction t in transactions)
                        {
                            System.Diagnostics.Debug.WriteLine(t.)
                        }
                    }
                }
            }
        }*/

    }
}
