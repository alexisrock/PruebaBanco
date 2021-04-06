using PruebaBanco.Clases;
using PruebaBanco.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PruebaBanco.Controllers
{
    [RoutePrefix("api/Cuenta")]
    public class CuentaController : ApiController
    {


        private BancoContext db = new BancoContext();

        [HttpGet]
        public IEnumerable<CuentasBanco> Get() {

            var cuentas = db.CuentaCliente.Select(x=> new { x.Cliente, x.Banco, x.saldo, x.id, x.numcuenta}).ToList();

            var dato = cuentas.Select(x => new CuentasBanco
            {
                Cliente = x.Cliente != null ? Convert.ToInt32(x.Cliente) : 0,
                Banco = x.Banco != null ? Convert.ToInt32(x.Banco) : 0,
                Numcuenta = x.numcuenta != null ? x.numcuenta : "",
                Saldo = x.saldo != null ? Convert.ToInt32(x.saldo.Value.ToString()) : 0,
                id = x.id != null ? x.id : 0
            }).ToList();

            return dato;
        }

        [Route("{id:int}/CuentasClientes")]
        [HttpGet]
        public IEnumerable<CuentasBanco> Get(int id)
        {

            var cuenta = db.CuentaCliente.Where(x=> x.Cliente==id).Select(x => new  CuentasBanco {             
                Numcuenta = x.numcuenta != null ? x.numcuenta : "",
                Saldo = (long)(x.saldo != null ? x.saldo : 0),
                id = x.id != null ? x.id : 0
            }).ToList();

   
            return cuenta;
        }

       
        [HttpGet]
        public CuentasBanco Get(int id, int? cuent)
        {

            var cuenta = db.CuentaCliente.Where(x => x.Cliente == id && x.id==cuent).Select(x => new CuentasBanco
            {
                Numcuenta = x.numcuenta != null ? x.numcuenta : "",
                Saldo = (long)(x.saldo != null ? x.saldo : 0),
                id = x.id != null ? x.id : 0
            }).FirstOrDefault();


            return cuenta;
        }



        [HttpPost]
        public string Post(int cliente, int valor) {

            using (DbContextTransaction tn = db.Database.BeginTransaction()) {
                try
                {
                    Random rdn = new Random();
                    CuentaCliente cuentacliente = new CuentaCliente();
                    cuentacliente.Banco = 1;
                    cuentacliente.Cliente = cliente;
                    cuentacliente.saldo = valor;
                    cuentacliente.numcuenta = rdn.Next().ToString();
                    db.CuentaCliente.Add(cuentacliente);
                    db.SaveChanges();
                    tn.Commit();
                    return cuentacliente.numcuenta;
                }
                catch (Exception ex)
                {
                    return "Error: " + ex.Message;
                    tn.Rollback();
                   
                }
            }           
             
        }

        [HttpPut]
        public string Transaccion(int idcuenta, int valor, int operaccion) {
        

            switch(operaccion)
            {
                case 1: Consignar(idcuenta, valor);
                    return "Transaccion exitosa";
                    break;
                case 2: Retirar(idcuenta, valor);
                    return "Transaccion exitosa";
                    break;


            }
           

            return "Cambio Exitoso";
        }



        public  void Consignar(int idcuenta, int valor)
        {
            using (DbContextTransaction tn = db.Database.BeginTransaction()) {
                try
                {
                    CuentaCliente cuentacliente = db.CuentaCliente.Where(x => x.id == idcuenta).FirstOrDefault();
                    cuentacliente.saldo += valor;
                    db.Entry(cuentacliente).State = EntityState.Modified;                    
                    transaccion transac = new transaccion();
                    transac.idcliente = cuentacliente.Cliente;
                    transac.idcuenta = idcuenta;
                    transac.fecha = DateTime.Now;
                    transac.valor = valor;
                    transac.operacion = 1;
                    db.transaccion.Add(transac);
                    db.SaveChanges();
                    tn.Commit();
                }
                catch (Exception ex)
                {

                    throw;
                }
            
            }




            


        }

        public void Retirar(int idcuenta, int valor)
        {
            using (DbContextTransaction tn = db.Database.BeginTransaction())
            {
                try
                {
                    CuentaCliente cuentacliente = db.CuentaCliente.Where(x => x.id == idcuenta).FirstOrDefault();
                    cuentacliente.saldo -= valor;
                    db.Entry(cuentacliente).State = EntityState.Modified;
                    transaccion transac = new transaccion();
                    transac.idcliente = cuentacliente.Cliente;
                    transac.idcuenta = idcuenta;
                    transac.fecha = DateTime.Now;
                    transac.valor = valor;
                    transac.operacion = 2;
                    db.transaccion.Add(transac);
                    db.SaveChanges();
                    tn.Commit();
                }
                catch (Exception ex)
                {

                    throw;
                }

            }





        }

        }
    }
