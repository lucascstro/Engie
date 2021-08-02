using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Engie.Models
{
    public class Usina
    {
        public int Id { get; set; }
        [Required]
        public string  Uc { get; set; }
        [Required]
        public bool  Ativo { get; set; }            
        public IList<Fornecedor> Fornecedor { get; set; }
    }
}