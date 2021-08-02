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
using Engie.Models.ViewModel;

namespace Engie.Controllers
{
    public class UsinasController : Controller
    {
        private Context db = new Context();

        // GET: Usinas
        public ActionResult Index()
        {
            Usina usina = new Usina();
            var todasUsinas = usina.PegarTodos();
            var fornecedor = db.Fornecedor.ToList();

            List<Usina> UsinaCompleta = new List<Usina>();
            List<Fornecedor> fornecedores = new List<Fornecedor>();
            foreach (var item in todasUsinas)
            {
                foreach (var item2 in fornecedor)
                {
                    if (item.IdFornecedor == item2.Id)
                    {
                        var retFornecedor = fornecedor.Where(x=>x.Id==item.IdFornecedor).ToList();
                        UsinaCompleta.Add(new Usina { Id = item.Id, Ativo = item.Ativo, Uc = item.Uc, IdFornecedor = item.IdFornecedor, Fornecedor = retFornecedor});
                    }
                }
            }

            return View(UsinaCompleta);
        }

        // GET: Usinas/Details/5
        public ActionResult Details(int? id)
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

        // GET: Usinas/Create
        public ActionResult Create()
        {
            Fornecedor fornecedor = new Fornecedor();
            Usina usina = new Usina();
            usina.Fornecedor = usina.TodosFornecedores();
            ViewData["Fornecedor"] = db.Fornecedor;

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
                db.Usina.Add(usina);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
            var retFornecedor = db.Fornecedor.ToList().Where(x => x.Id == id).ToList();
            usinaID.Fornecedor = retFornecedor;           

            ViewData["Fornecedor"] = new Fornecedor().PegarTodos();

            if (usinaID == null)
            {
                return HttpNotFound();
            }
            foreach (var item in retFornecedor) {
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
                db.Entry(usina).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
