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

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrencyLoad();
            ReccomendProducts();
        }

        private void ReccomendProducts()
        {
            //тут получаем тип пользователя
            var usertype = "guest";

            //StartupParameter rinit = new StartupParameter();
            //rinit.Quiet = true;
            //rinit.RHome = "C:/Program Files/R/R-3.5.0";
            //rinit.Interactive = true;
            REngine.SetEnvironmentVariables();
           // REngine engine = REngine.GetInstance(null, true, rinit);
            REngine engine = REngine.GetInstance();
            engine.Evaluate("require(tidyr)");
            #region Подключаем пакеты R, если их нет, то устанавливаем и подключаем
            try {  }
            catch (EvaluationException e)
            {
                //engine.Evaluate("install.packages('tidyr')");
                engine.Evaluate("library(tidyr)");
            }
            try { engine.Evaluate("library(recommenderlab)"); }
            catch (EvaluationException e)
            {
                engine.Evaluate("install.packages(recommenderlab)");
                engine.Evaluate("library(recommenderlab)");
            }
            try { engine.Evaluate("library(dplyr)"); }
            catch (EvaluationException e)
            {
                engine.Evaluate("install.packages(dplyr)");
                engine.Evaluate("library(dplyr)");
            }

            try { engine.Evaluate("library(RODBC)"); }
            catch (EvaluationException e)
            {
                engine.Evaluate("install.packages('RODBC')");
                engine.Evaluate("library(RODBC)");
            }
            engine.Evaluate("library(RODBC)");
            #endregion

            engine.Evaluate("connectionString <- 'Driver ={ ODBC Driver 13 for SQL Server}; Server = tcp:chainreaction.database.windows.net,1433; Database = chainreaction; Uid = hacker@chainreaction; Pwd = kotbumnahpro4$; Encrypt = yes; TrustServerCertificate = no; Connection Timeout = 30;'");


            engine.Evaluate("myconn < -odbcDriverConnect(connectionString))");

            if (usertype == "guest")
            {
                // взять из бд
                var userid_fromhtml = "34";
                //engine.Evaluate("users < -sqlQuery(myconn, 'SELECT * FROM users'");
                //engine.Evaluate("transactions < -sqlQuery(myconn, 'SELECT * FROM transactions')");
                //engine.Evaluate(" products < -sqlQuery(myconn, 'SELECT * FROM products')");
                engine.Evaluate("users < -read.csv('C:\\git\\ChainReaction\\users.csv', sep=';')");
                engine.Evaluate("transactions < -read.csv('C:\\git\\ChainReaction\\transactions.csv', sep=';')");
                engine.Evaluate("products < -read.csv('C:\\git\\ChainReaction\\products.csv', sep=';')");
                engine.Evaluate("products$description < - as.character(products$description)");
                engine.Evaluate("products$name < - as.character(products$name)");
                engine.Evaluate(" products < -dplyr::rename(products, 'product' = 'id')");
                engine.Evaluate("userid_fromhtml=" + userid_fromhtml);
                engine.Evaluate("walletCertain = users[which(users$id == userid_fromhtml),]");
                engine.Evaluate("walletCertain = walletCertain$wallet");
                engine.Evaluate("trCertainUser = dplyr::filter(transactions, as.character(from) == as.character(walletCertain))");
                engine.Evaluate("trCertainUserPro < -left_join(trCertainUser, products, by = 'product')");
                engine.Evaluate("trCertainUserPro < -trCertainUserPro %>% dplyr::arrange(-time)");

                engine.Evaluate("if (nrow(trCertainUserPro) > 5) { trCertainUserPro =trCertainUserPro[1:5,]}");
                //достаем 5 последних товаров
                var trCertainUserPro = engine.GetSymbol("trCertainUserPro").AsVector();

                engine.Evaluate("recData < -left_join(transactions, dplyr::rename(dplyr::select(users, id, wallet), 'from' = 'wallet'))");
                engine.Evaluate("recData < -dplyr::select(recData, id, value, product)");
                engine.Evaluate("recData <- recData %>% group_by(id, product) %>% summarise_all(sum)");
                engine.Evaluate("recData < -spread(recData, key = product, value = value)");
                engine.Evaluate("recData <- as.data.frame(recData)");
                engine.Evaluate("rownames(recData) = recData$id");
                engine.Evaluate("recData = dplyr::select(recData, -id)");
                engine.Evaluate("recData = as.matrix(recData)");

                engine.Evaluate("r = as(recData, 'realRatingMatrix')");
                engine.Evaluate("r < -binarize(r, minRating = 2)");
                engine.Evaluate("recc_model < -Recommender(data = r, method = 'IBCF', parameter = list(k = 30))");
                engine.Evaluate("recc_predicted < -predict(object = recc_model, newdata = r[2,])");

                engine.Evaluate("recc_predicted @itemLabels[recc_predicted@items[[1]]]");

                engine.Evaluate("   products$product < - as.character(products$product)");
                engine.Evaluate("indexTitle = match(products$product, pred)");
                engine.Evaluate("indexTitle < -na.omit(indexTitle)");
                engine.Evaluate("pred_data=products$name[indexTitle]");


                // вытяиваем стринг вектор с предсказанием
                var predicted = engine.GetSymbol("pred_data").AsVector();
                
            }
            else if (usertype == "worker")
            {
                // взять из бд
                var userid_fromhtml = "34";
                engine.Evaluate("users < -sqlQuery(myconn, 'SELECT * FROM users1'");
                engine.Evaluate("transactions < -sqlQuery(myconn, 'SELECT * FROM transactions2')");
                engine.Evaluate(" products < -sqlQuery(myconn, 'SELECT * FROM products1')");

                engine.Evaluate("");
                engine.Evaluate("");
            }
            else if (usertype == "employer")
            {
                // взять из бд
                var userid_fromhtml = "34";
                engine.Evaluate("users < -sqlQuery(myconn, 'SELECT * FROM users1'");
                engine.Evaluate("transactions < -sqlQuery(myconn, 'SELECT * FROM transactions2')");
                engine.Evaluate(" products < -sqlQuery(myconn, 'SELECT * FROM products1')");

                engine.Evaluate("");
                engine.Evaluate("");
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