using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChainReactionBack.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("First Name")]
        [StringLength(128)]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [StringLength(128)]
        public string LastName { get; set; }

        [StringLength(256)]
        public string Wallet { get; set; }

        [DisplayName("Role")]
        [StringLength(128)]
        public string Role { get; set; }

        // Relation one-to-many
        public ICollection<Product> Products { get; set; }

        // Relation one-to-many
        public ICollection<SmartContract> SmartContracts { get; set; }

        // Relation one-to-many
        public ICollection<Transaction> FromTransactions { get; set; }
        public ICollection<Transaction> ToTransactions { get; set; }


    }
}