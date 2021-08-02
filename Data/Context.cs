using Engie.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Engie.Data
{
    public class Context : DbContext
    {
        public Context() : base("EngieBD")
        { }

        //DBset Entidade Usina
        public DbSet<Usina> Usina { get; set; }

        //DBset Entidade Fornecedor
        public DbSet<Fornecedor> Fornecedor { get; set; }
        
        //metodo para verificar necessidade de popular tabela Fornecedores
        public void VerificaFornecedor()
        {
            try
            {   //verifica se tem forncedores cadastrados, se não tiver poupula tabela 
                if (Fornecedor.ToList().Count == 0)
                {
                    var ListFornecedor = new List<Fornecedor>{
                    new Fornecedor{Id=1,Nome="SOLARIAN" },
                    new Fornecedor{Id=2,Nome="FUTURA" },
                    new Fornecedor{Id=3,Nome="CENTRAL GERADORA FAZENDA MODELO" },
                    new Fornecedor{Id=4,Nome="NOVA MUNDO" },
                    new Fornecedor{Id=5,Nome="SOLARE" },
                    new Fornecedor{Id=6,Nome="UNISOL" }
                };
                    //Pega lista de Fornecedores e salva no banco.
                    ListFornecedor.ForEach(x => Fornecedor.Add(x));
                    SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao inserir os fornecedores! - Erro:" + e.Message.ToString()); ;
            }
        }
     
    }
}