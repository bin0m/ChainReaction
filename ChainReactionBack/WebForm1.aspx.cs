﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using 

namespace ChainReactionBack
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public int rubETH=1;
        public int rubBIT = 1;
        public int rubSMRT = 1;
        public int PriceBTC = 1;
        public int smartcitycoin_price = 1;
        public int PriceETH = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            var usertype = "";

            REngine.SetEnvironmentVariables();
            REngine engine = REngine.GetInstance();

            #region Подключаем пакеты R, если их нет, то устанавливаем и подключаем
            try { engine.Evaluate("library(tidyr)"); }
            catch (EvaluationException e)
            {
                engine.Evaluate("install.package(tidyr)");
                engine.Evaluate("library(tidyr)");
            }
            try { engine.Evaluate("library(recommenderlab)"); }
            catch (EvaluationException e)
            {
                engine.Evaluate("install.package(recommenderlab)");
                engine.Evaluate("library(recommenderlab)");
            }
            try { engine.Evaluate("library(dplyr)"); }
            catch (EvaluationException e)
            {
                engine.Evaluate("install.package(dplyr)");
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
                engine.Evaluate("users < -sqlQuery(myconn, 'SELECT * FROM users'");
                engine.Evaluate("transactions < -sqlQuery(myconn, 'SELECT * FROM transactions')");
                engine.Evaluate(" products < -sqlQuery(myconn, 'SELECT * FROM products')");

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
    }
}