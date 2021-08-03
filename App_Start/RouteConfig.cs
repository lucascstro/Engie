using Engie.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Engie
{
    public class RouteConfig
    {
        static private Context db = new Context();
        public static void RegisterRoutes(RouteCollection routes)
        {   
            //verifica se o banco ja tem REGISTRO DE FORNECEDORES adicionados, se vazio adiciona os fornecedores.
            db.VerificaFornecedor();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Usinas", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
