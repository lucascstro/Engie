using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Engie.Models
{
    public class Fornecedor
    {

        public int Id { get; set; }
        [Display(Name = "Fornecedor")]      
        public string Nome { get; set; }
    }
}