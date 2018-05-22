using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChainReactionBack.Models;

namespace ChainReactionBack
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SeedDatabase();
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