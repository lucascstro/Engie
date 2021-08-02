using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Engie.Data;

namespace Engie.Models
{
    public class Fornecedor
    {

        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }

        private readonly Context _context = new Context();
        public List<Fornecedor> PegarTodos()
        {
            return _context.Fornecedor.OrderBy(x => x.Nome).ToList();
        }
    }
}