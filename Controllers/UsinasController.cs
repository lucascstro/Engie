using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Engie.Data;
using Engie.Models;

namespace Engie.Controllers
{
    public class UsinasController : Controller
    {
        private Context db = new Context();

        // GET: Usinas
        public ActionResult Index()
        {
            //Recupera variaveis da URL
            int fornParam = Convert.ToInt32(Request.QueryString["fornecedorDrop"]);
            int ativoParam = Convert.ToInt32(Request.QueryString["AtivoDrop"]);

            //construtor do objeto Usina
            Usina usina = new Usina();

            //Recupera lista de usinas
            var todasUsinas = usina.PegarTodos();

            //recupera lista de fornecedores
            var fornecedor = db.Fornecedor.ToList();
            
            //Construto do dropdownlist de filtros
            List<SelectListItem> estadoList = new List<SelectListItem>();
            List<SelectListItem> fornecedorList = new List<SelectListItem>();

            //Adição de opção basicas do filtro para o dropdownlist  
            estadoList.Add(new SelectListItem { Text = "Todos", Value = "0" });
            estadoList.Add(new SelectListItem { Text = "Sim", Value = "1" });
            estadoList.Add(new SelectListItem { Text = "Não", Value = "2" });
            fornecedorList.Add(new SelectListItem { Text = "Todos", Value = "0" });

            //Adiciona registro de fornecedores ao objeto do dropdownlist
            foreach (var item in fornecedor)
            {
                fornecedorList.Add(new SelectListItem { Text = item.Nome, Value = item.Id.ToString() });
            }

            //contrutor do objeto que vai com todas as propriedades dos objetos Usinas e seu Fornecedor preenchidas para popultar table
            List<Usina> UsinaCompleta = new List<Usina>();

            //construtor do objeto auxiliador
            List<Fornecedor> fornecedores = new List<Fornecedor>();

            //populando tabela
            //filtros: Se ddl de fornecedor diferente de todos e ativo igual a todos
            if (fornParam > 0 && ativoParam == 0)
            {
                UsinaCompleta.Clear();
                       
                bool? a = ativoParam == 0 ? true : false;
                foreach (var item in todasUsinas.Where(x => x.IdFornecedor == fornParam))
                {
                    foreach (var item2 in fornecedor)
                    {
                        if (item.IdFornecedor == item2.Id)
                        {
                            var retFornecedor = fornecedor.Where(x => x.Id == item.IdFornecedor).ToList();
                            UsinaCompleta.Add(new Usina { Id = item.Id, Ativo = item.Ativo, Uc = item.Uc, IdFornecedor = item.IdFornecedor, Fornecedor = retFornecedor });
                        }
                    }
                }
            }
            //filtrps: se ddl igual a todos e ativo diferente de todos
            else if (fornParam == 0 && ativoParam > 0)
            {
                UsinaCompleta.Clear();
                //Filtros           
                bool? a = ativoParam == 1 ? true : false;
                foreach (var item in todasUsinas.Where(x => x.Ativo.Equals(a)))
                {
                    foreach (var item2 in fornecedor)
                    {
                        if (item.IdFornecedor == item2.Id)
                        {
                            var retFornecedor = fornecedor.Where(x => x.Id == item.IdFornecedor).ToList();
                            UsinaCompleta.Add(new Usina { Id = item.Id, Ativo = item.Ativo, Uc = item.Uc, IdFornecedor = item.IdFornecedor, Fornecedor = retFornecedor });
                        }
                    }
                }
            }
            //filtros: se ambos filtros estiverem diferente de todos
            else if (fornParam > 0 && ativoParam > 0)
            {
                UsinaCompleta.Clear();
                //Filtros           
                bool? a = ativoParam == 0 ? true : false;
                foreach (var item in todasUsinas.Where(x => x.IdFornecedor == fornParam && x.Ativo.Equals(a)))
                {
                    foreach (var item2 in fornecedor)
                    {
                        if (item.IdFornecedor == item2.Id)
                        {
                            var retFornecedor = fornecedor.Where(x => x.Id == item.IdFornecedor).ToList();
                            UsinaCompleta.Add(new Usina { Id = item.Id, Ativo = item.Ativo, Uc = item.Uc, IdFornecedor = item.IdFornecedor, Fornecedor = retFornecedor });
                        }
                    }
                }
            }
            //Carregamento padrao da tabela
            else 
            {
                UsinaCompleta.Clear();
                foreach (var item in todasUsinas)
                {
                    foreach (var item2 in fornecedor)
                    {
                        if (item.IdFornecedor == item2.Id)
                        {
                            var retFornecedor = fornecedor.Where(x => x.Id == item.IdFornecedor).ToList();
                            UsinaCompleta.Add(new Usina { Id = item.Id, Ativo = item.Ativo, Uc = item.Uc, IdFornecedor = item.IdFornecedor, Fornecedor = retFornecedor });
                        }
                    }
                }
            }

            //Auxiliar para o drop
            ViewBag.fornecedorDrop = fornecedorList.ToList();
            ViewBag.AtivoDrop = estadoList.ToList();

            return View(UsinaCompleta);
        }

        // GET: Usinas/Create
        public ActionResult Create()
        {
            //contrutores dos objetos
            Fornecedor fornecedor = new Fornecedor();
            Usina usina = new Usina();

            //carrega drop de fornecedores
            usina.Fornecedor = usina.TodosFornecedores();
            ViewData["Fornecedor"] = db.Fornecedor;

            ViewData["Erro"] = "";
            //seta checkbox ativo chekcado
            usina.Ativo = true;
            return View(usina);
        }

        // POST: Usinas/Create
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Uc,Ativo,IdFornecedor")] Usina usina)
        {
            //verifica se dados sao validos
            if (ModelState.IsValid)
            {
                //verifica se existe uc e fornecedor cadastradao                                               
                var cadastrado = db.Usina.Where(x => x.Uc == usina.Uc && x.IdFornecedor == usina.IdFornecedor).ToList();
                //não há registro com mesmos dados de uc e fornecedor
                if (cadastrado.Count() == 0)
                {
                    //Adiciona objeto usina no banco
                    db.Usina.Add(usina);
                    //confirmação
                    db.SaveChanges();
                    //mensagem de confirmação
                    ModelState.AddModelError("CustomError", "Castro Realizado!");
                }
                //registro igual encontrado 
                else
                {
                    //mensagem de erro
                    string errorMessage = String.Format("Cadastro existente!");
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }
            //Carrega drop fornecedores
            usina.Fornecedor = usina.TodosFornecedores();
            ViewData["Fornecedor"] = db.Fornecedor;

            return View(usina);
        }

        // GET: Usinas/Edit/5
        public ActionResult Edit(int? id)
        {
            //verifica retorno do id
            if (id == null)
            {
                //Erro
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //busca registro do id no banco
            Usina usinaID = db.Usina.Find(id);

            //verifica se existe o registro no banco
            if (usinaID == null)
            {
                return HttpNotFound();
            }

            //busca de fornecedores
            var retFornecedor = db.Fornecedor.ToList();
            //verifica fornecedor com mesmo ID do objeto Usina
            var retFornecedoriD = retFornecedor.Where(x => x.Id == id).ToList();
            //Seta o fornecedor com todas propriedades no objeto usina
            usinaID.Fornecedor = retFornecedoriD;
            
            //Auxilicar para carregar o drop de fornecedores
            ViewData["Fornecedor"] = retFornecedor.ToList();

           //Recupera nome do fornecedor para carregar drop com o nome           
            foreach (var item in retFornecedor)
            {
                ViewData["FornecedorId"] = item.Nome;
            }

            return View(usinaID);
        }

        // POST: Usinas/Edit/5
        // Para proteger-se contra ataques de excesso de postagem, ative as propriedades específicas às quais deseja se associar. 
        // Para obter mais detalhes, confira https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Uc,Ativo,IdFornecedor")] Usina usina)
        {
            //verifica se os dados sao validos
            if (ModelState.IsValid)
            {
                //verifica se existe usina com os mesmo dados cadastrado no banco
                var cadastrado = db.Usina.Where(x => x.Uc == usina.Uc && x.IdFornecedor == usina.IdFornecedor && x.Ativo == usina.Ativo).ToList();
                //se houver alteração de dados, faz edição do registro usina
                if (cadastrado.Count() == 0)
                {
                    //Informea EF da modificação do objeto
                    db.Entry(usina).State = EntityState.Modified;
                    //Salva alterações
                    db.SaveChanges();
                    //Mensagem de confirmação
                    ModelState.AddModelError("CustomError", "Edição Realizada!");
                }
                //os dados são os mesmo que estão no banco
                else
                {
                    //mensagem de cadastro existente
                    string errorMessage = String.Format("Cadastro existente!");
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }
            //Auxilicar para carregar drop de fornecedores
            usina.Fornecedor = usina.TodosFornecedores();
            ViewData["Fornecedor"] = db.Fornecedor;

            return View(usina);
        }

        // GET: Usinas/Delete/5
        public ActionResult Delete(int? id)
        {
            //verifica se possui id
            if (id == null)
            {
                //Erro 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Busca o Id do objeto no banco
            Usina usina = db.Usina.Find(id);
            //verifica se teve retorno com registro
            if (usina == null)
            {
                return HttpNotFound();
            }
            return View(usina);
        }

        // POST: Usinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //recupera registro do objeto no banco conforme id
            Usina usina = db.Usina.Find(id);
            //remove registro do banco
            db.Usina.Remove(usina);
            //confirma alterações
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
