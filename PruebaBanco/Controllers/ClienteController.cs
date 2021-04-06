using PruebaBanco.Clases;
using PruebaBanco.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using HttpPutAttribute = System.Web.Mvc.HttpPutAttribute;
using RouteAttribute = System.Web.Mvc.RouteAttribute;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;

namespace PruebaBanco.Controllers
{
    [RoutePrefix("api/Cliente")]
    public class ClienteController : ApiController
    {

        private readonly BancoContext context = new BancoContext();

        [HttpGet]
        public IEnumerable<Clientes>  Get() {

          
              var  clientes = context.Cliente.Select(x=>new {x.documento, x.nombres, x.telefono, x.cobrogm }).ToList();
                 var dato = clientes.Select(x => new Clientes {
                  cobrogm=   x.cobrogm, 
                  documento=   x.documento,
                  Cliente=   x.nombres,
                  Telefono=  x.telefono }).ToList();

            return dato;
        }
        
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var clientes = context.Cliente.Where(x => x.id == id).Select(x => new { x.documento, x.nombres, x.telefono, x.cobrogm, x.id }).FirstOrDefault();


            if (clientes != null)
            {


                Clientes cliente = new Clientes();

                cliente.cobrogm = clientes.cobrogm;
                cliente.documento = clientes.documento;
                cliente.Cliente = clientes.nombres;
                cliente.Telefono = clientes.telefono;

                return Ok(cliente);

            }
            else {

                return BadRequest();
            }


        }

        [HttpGet]
        public Clientes Get(string Documento)
        {
            Clientes cliente = new Clientes();
            try
            {
                  var clientes = context.Cliente.Where(x => x.documento == Documento).Select(x => new { x.documento, x.nombres, x.telefono, x.cobrogm, x.id }).FirstOrDefault();
                if (clientes != null)
                {
                    cliente.cobrogm = clientes.cobrogm;
                    cliente.documento = clientes.documento;
                    cliente.Cliente = clientes.nombres;
                    cliente.Telefono = clientes.telefono;
                
                }
             

            }
            catch (Exception)
            {

                throw;
            }

            return cliente;
        }


        [HttpPost]
        public string Post(Clientes clientes) {

            Cliente cliente = new Cliente();
            cliente.documento = clientes.documento;
            cliente.nombres = clientes.Cliente;
            cliente.telefono = clientes.Telefono;
            cliente.cobrogm = clientes.cobrogm;
            context.Cliente.Add(cliente);
            context.SaveChanges();

            return "Usuario creado con exito";
        }

        [HttpPut]
        public string Put(Clientes clientes)
        {
            var cliente = context.Cliente.Where(x => x.documento == clientes.documento).FirstOrDefault();

        
            cliente.documento = clientes.documento;
            cliente.nombres = clientes.Cliente;
            cliente.telefono = clientes.Telefono;
            cliente.cobrogm = clientes.cobrogm;
            context.Entry(cliente).State = EntityState.Modified;
            context.SaveChanges();

            return "Usuario creado con exito";
        }
    

    }
}
