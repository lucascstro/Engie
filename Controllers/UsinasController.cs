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
            int fornParam = Convert.ToInt32(Request.QueryString["fornecedorDrop"]);
            int ativoParam = Convert.ToInt32(Request.QueryString["AtivoDrop"]);

            Usina usina = new Usina();
            var todasUsinas = usina.PegarTodos();
            var fornecedor = db.Fornecedor.ToList();
            List<SelectListItem> estadoList = new List<SelectListItem>();
            List<SelectListItem> fornecedorList = new List<SelectListItem>();
            List<SelectListItem> ucList = new List<SelectListItem>();
            estadoList.Add(new SelectListItem { Text = "Todos", Value = "0" });
            estadoList.Add(new SelectListItem { Text = "Sim", Value = "1" });
            estadoList.Add(new SelectListItem { Text = "Não", Value = "2" });
            fornecedorList.Add(new SelectListItem { Text = "Todos", Value = "0" });

            foreach (var item in fornecedor)
            {
                fornecedorList.Add(new SelectListItem { Text = item.Nome, Value = item.Id.ToString() });
            }
            foreach (var item in todasUsinas)
            {
                ucList.Add(new SelectListItem { Text = item.Uc, Value = item.Id.ToString() });
            }
            List<Usina> UsinaCompleta = new List<Usina>();
            List<Fornecedor> fornecedores = new List<Fornecedor>();

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

            ViewBag.fornecedorDrop = fornecedorList.ToList();
            ViewBag.UcDrop = ucList.ToList();
            ViewBag.AtivoDrop = estadoList.ToList();
            return View(UsinaCompleta);
        }

        // GET: Usinas/Create
        public ActionResult Create()
        {
            Fornecedor fornecedor = new Fornecedor();
            Usina usina = new Usina();
            usina.Fornecedor = usina.TodosFornecedores();
            ViewData["Fornecedor"] = db.Fornecedor;
            ViewData["Erro"] = "";
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
            if (ModelState.IsValid)
            {
                //verifica uc e fornecedor cadastrada                                                
                var cadastrado = db.Usina.Where(x => x.Uc == usina.Uc && x.IdFornecedor == usina.IdFornecedor).ToList();
                if (cadastrado.Count() == 0)
                {
                    db.Usina.Add(usina);
                    db.SaveChanges();
                    ModelState.AddModelError("CustomError", "Castro Realizado!");
                }
                else
                {
                    string errorMessage = String.Format("Cadastro existente!");
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }

            usina.Fornecedor = usina.TodosFornecedores();
            ViewData["Fornecedor"] = db.Fornecedor;

            return View(usina);
        }

        // GET: Usinas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usina usinaID = db.Usina.Find(id);
            var retFornecedor = db.Fornecedor.ToList();
            var retFornecedoriD = retFornecedor.Where(x => x.Id == id).ToList();
            usinaID.Fornecedor = retFornecedoriD;

            ViewData["Fornecedor"] = retFornecedor.ToList();

            if (usinaID == null)
            {
                return HttpNotFound();
            }
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
            if (ModelState.IsValid)
            {
                var cadastrado = db.Usina.Where(x => x.Uc == usina.Uc && x.IdFornecedor == usina.IdFornecedor && x.Ativo == usina.Ativo).ToList();
                if (cadastrado.Count() == 0)
                {
                    db.Entry(usina).State = EntityState.Modified;
                    db.SaveChanges();
                    ModelState.AddModelError("CustomError", "Edição Realizada!");
                }
                else
                {
                    string errorMessage = String.Format("Cadastro existente!");
                    ModelState.AddModelError(string.Empty, errorMessage);
                }
            }

            usina.Fornecedor = usina.TodosFornecedores();
            ViewData["Fornecedor"] = db.Fornecedor;

            return View(usina);
        }

        // GET: Usinas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usina usina = db.Usina.Find(id);
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
            Usina usina = db.Usina.Find(id);
            db.Usina.Remove(usina);
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
