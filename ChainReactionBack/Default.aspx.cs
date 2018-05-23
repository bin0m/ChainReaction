using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChainReactionBack.Models;
using RDotNet;
using System.Diagnostics;
using System.Data.SqlClient;

namespace ChainReactionBack
{
    public partial class _Default : Page
    {
        public int rubETH = 1;
        public int rubBIT = 1;
        public int rubSMRT = 1;
        public int PriceBTC = 1;
        public int smartcitycoin_price = 1;
        public int PriceETH = 1;

        public List<string> recResults = new List<string>();
        public List<forGraph> forGraphs = new List<forGraph>();

        protected void Page_Load(object sender, EventArgs e)
        {

            //Process.Start("");
            CurrencyLoad();
            DataStatsLoad();

        }

        private void DataStatsLoad()
        {
            var connectionString = "Server=tcp:chainreaction.database.windows.net,1433;Initial Catalog=chainreaction;Persist Security Info=False;User ID=hacker;Password=kotbumnahpro4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

            // Create and open the connection in a using block. This
            // ensures that all resources will be closed and disposed
            // when the code exits.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("SELECT * from dbo.recResults", connection))
                {

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            recResults.Add(reader[1].ToString());
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                    }
                }
                using (SqlCommand command = new SqlCommand("SELECT * from dbo.forGraph", connection))
                {

                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            var forGraph = new forGraph();
                            forGraph.Balance = reader.GetDouble(1);
                            forGraph.Time = reader.GetDouble(2);
                            forGraph.Changes = reader.GetDouble(3);
                            forGraph.DynamicProfit = reader.GetDouble(4);
                            forGraphs.Add(forGraph);
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        private void CurrencyLoad()
        {
            string url = "https://api.coinmarketcap.com/v2/ticker/?limit=10"; ;
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            WebResponse myResponse = myRequest.GetResponse();
            StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            myResponse.Close();
            string btc = "Bitcoin";
            int indexOfBTC = result.IndexOf(btc);
            result = result.Substring(indexOfBTC, 1100);
            int indexOfPrice = result.IndexOf("price");
            int finish = indexOfPrice + 8;
            string price = result.Substring(finish, 5);
            price = price.Substring(0, 4);
            // цены криптоактива в долларах 
            PriceBTC = Int32.Parse(price);
            string eth = "Ethereum";
            int indexOfETH = result.IndexOf(eth);
            result = result.Substring(716, 320);
            indexOfPrice = result.IndexOf("price");
            finish = indexOfPrice + 8;
            price = result.Substring(finish, 5);
            price = price.Substring(0, 3);
            // цены криптоактива в долларах 
            PriceETH = Int32.Parse(price);
            string smartcitycoin = "Smartcity_coin";
            // цены криптоактива в долларах 
            smartcitycoin_price = 167;
            int dollarprice = 60;
            // цены криптоактивов в рублях 
            rubETH = dollarprice * PriceETH;
            rubBIT = dollarprice * PriceBTC;
            rubSMRT = dollarprice * smartcitycoin_price;
        }

        private void SeedDatabase()
        {
            using (var db = new ChainReactionContext())
            {
                var user1 = new User
                {
                    FirstName = "Василий",
                    LastName = "Пупкин",
                    Email = "vasya.pupkin@mail.ru",
                    Role = "employer",
                    Wallet = "0xa7e305c29c740af7cb47088ef0f9b88e58f46ca710617c7a45e51224f1ea934c"
                };
                db.Users.Add(user1);

                var product = new Product
                {
                    Name = "Стоянка",
                    Price = 60,
                    User = user1
                };
                db.Products.Add(product);

                db.SaveChanges();

            }
        }
    }
}