using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cripto.Models;

namespace CriptoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly CryptoContext db;

        public QueryController(CryptoContext context)
        {
            db = context;
        }

        [HttpGet("0")]
        public ActionResult Query0(int ValorActual = 50)
        {
            // Ejemplo de método en controlador
            var list = db.Moneda.ToListAsync();

            return Ok(new
            {
                ValorActual = "Parámetros para usar cuando sea posible",
                Descripcion = "Ejemplo en MODO NO ASYNC - DEBE SER ASÍNCRONOS",
                Valores = list,
            });
        }
        [HttpGet("1")]
        public async Task<ActionResult> Query1()
        {
            var list=db.Moneda.Where(m => m.Actual >= 50).OrderBy(m => m.MonedaId);

            return Ok(new
            {
                Valores = list,
            });

        }

        [HttpGet("2")]
        public async Task<ActionResult> Query2()
        {
            var list=db.Contrato.GroupBy(c => c.CarteraId).Select(c => new{
                CarteraId=c.Key,
                Cantidad=c.Count()
            }).Where(c => c.Cantidad > 2).ToListAsync();

            return Ok(new
            {
                Valores = list,
            });

        }

        [HttpGet("/3/")]
        public async Task<ActionResult> Query3()
        {
            var list=db.Cartera.GroupBy(c => c.Exchange).Select(c => new{
                CarteraId=c.Key,
                Cantidad=c.Count()
            }).OrderByDescending(a =>a.Cantidad);

            return Ok(new
            {

                Valores = list,
            });

        }
    }
}
