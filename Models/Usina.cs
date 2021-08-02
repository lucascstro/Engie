using Engie.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Engie.Models
{
    public class Usina
    {
        public int Id { get; set; }
        [Required]
        public string Uc { get; set; }
        [Required]
        public bool Ativo { get; set; }
        [Required]
        public int IdFornecedor { get; set; }       
        public List<Fornecedor> Fornecedor { get; set; }

        private readonly Context _context = new Context();
        public List<Fornecedor> TodosFornecedores()
        {            
            return _context.Fornecedor.OrderBy(x => x.Id).ToList();
        }
        public List<Usina> PegarTodos()
        {
            return _context.Usina.OrderBy(x => x.IdFornecedor).ToList();
        }
    }
}