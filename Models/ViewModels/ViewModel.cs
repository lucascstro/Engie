using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Engie.Models.ViewModels
{
    public class ViewModel
    {
        public Usina Usina { get; set; }
        public ICollection<Fornecedor> Fornecedor { get; set; }
    }
}