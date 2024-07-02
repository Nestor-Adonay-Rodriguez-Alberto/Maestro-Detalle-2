using Entidades;
using Logica_Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI_Practica_2.Controllers
{
    public class FacturaController : Controller
    {
        // Puente Con DB:
        private readonly FacturaBL _FacturaBL;

        // Constructor
        public FacturaController(FacturaBL facturaBL)
        {
            _FacturaBL = facturaBL;
        }


        // Manda Todos Los Registros De La Tabla:
        public async Task<IActionResult> Registros_Facturas()
        {
            List<Factura> Objetos_Obtenidos = await _FacturaBL.Obtener_Todas();

            return View(Objetos_Obtenidos);
        }

        // Manda Un Objeto Encontrado De La Tabla:
        public async Task<IActionResult> Detalle_Factura(int id)
        {
            Factura Objeto_Obtenido = await _FacturaBL.Obtener_PorId(new Factura() { IdFactura = id });

            ViewData["Lista_Productos"] = await _FacturaBL.Lista_Productos();

            ViewBag.Accion = "Detalle_Factura";
            return View(Objeto_Obtenido);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Registrar_Factura()
        {
            // Contamos Los Registros:
            List<Factura> Objetos_Obtenidos = await _FacturaBL.Obtener_Todas();
            int Facturas_Registradas = Objetos_Obtenidos.Count;


            // Objeto Con Informacion de Inicio:
            Factura Objeto_Inicio = new Factura();

            DateTime fechaHoraActual = DateTime.Now;
            DateTime fechaHoraActualizada = new DateTime(
                fechaHoraActual.Year,
                fechaHoraActual.Month,
                fechaHoraActual.Day,
                fechaHoraActual.Hour,
                fechaHoraActual.Minute,
                0
            );
            Objeto_Inicio.FechaRealizada = fechaHoraActualizada;

            Objeto_Inicio.Correlativo = Facturas_Registradas + 1;

            Objeto_Inicio.Lista_DetalleFactura = new List<Detalle_Factura>();
            Objeto_Inicio.Lista_DetalleFactura.Add(new Detalle_Factura { Cantidad = 1, PrecioDel_Producto = 0 });


            ViewData["Lista_Productos"] = await _FacturaBL.Lista_Productos();

            ViewBag.Accion = "Registrar_Factura";
            return View(Objeto_Inicio);
        }

        // Recibe Un Objeto Y Lo Guarda En La Tabla:
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar_Factura(Factura factura)
        {
            // Verificar Si Un Atributo Del Detalle Biene Vasio
            if (factura.Lista_DetalleFactura.Any(x => x.IdProductoEnDetalle == 0 || x.Cantidad < 1 || x.PrecioDel_Producto < 1))
            {

                if (factura.Lista_DetalleFactura.Any(x => x.IdProductoEnDetalle == 0))
                {
                    TempData["ProductoRequerido"] = "Seleccione El Producto A LLevar.";
                }
                if (factura.Lista_DetalleFactura.Any(x => x.Cantidad < 1))
                {
                    TempData["CantidadRequerida"] = "Ingrese La Cantidad A LLevar.";
                }

                ViewData["Lista_Productos"] = await _FacturaBL.Lista_Productos();


                ViewBag.Accion = "Registrar_Factura";
                return View(factura);
            }
            else
            {
                factura.Total = factura.Lista_DetalleFactura.Sum(x => x.Cantidad * x.PrecioDel_Producto);
                await _FacturaBL.Registrar_Factura(factura);
            }

            return RedirectToAction("Registros_Facturas", "Factura");
        }


        // Manda Un Objeto Encontrado De La Tabla
        public async Task<IActionResult> Editar_Factura(int id)
        {
            Factura Objeto_Obtenido = await _FacturaBL.Obtener_PorId(new Factura() { IdFactura = id });

            ViewData["Lista_Productos"] = await _FacturaBL.Lista_Productos();

            ViewBag.Accion = "Editar_Factura";
            return View(Objeto_Obtenido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Editar_Factura(Factura factura)
        {
            // Verificar Si Un Atributo Del Detalle Biene Vasio
            if (factura.Lista_DetalleFactura.Any(x => x.IdProductoEnDetalle == 0 || x.Cantidad < 1 || x.PrecioDel_Producto < 1))
            {

                if (factura.Lista_DetalleFactura.Any(x => x.IdProductoEnDetalle == 0))
                {
                    TempData["ProductoRequerido"] = "Seleccione El Producto A LLevar.";
                }
                if (factura.Lista_DetalleFactura.Any(x => x.Cantidad < 1))
                {
                    TempData["CantidadRequerida"] = "Ingrese La Cantidad A LLevar.";
                }

                ViewData["Lista_Productos"] = await _FacturaBL.Lista_Productos();

                ViewBag.Accion = "Editar_Factura";
                return View(factura);
            }
            else
            {
                await _FacturaBL.Editar_Factura(factura);
            }

            return RedirectToAction("Registros_Facturas", "Factura");
        }

        // Manda Un Objeto Encontrado De La Tabla:
        public async Task<IActionResult> Eliminar_Factura(int id)
        {
            Factura Objeto_Obtenido = await _FacturaBL.Obtener_PorId(new Factura() { IdFactura = id });

            ViewData["Lista_Productos"] = await _FacturaBL.Lista_Productos();

            ViewBag.Accion = "Eliminar_Factura";
            return View(Objeto_Obtenido);
        }


        // Recibe Un Objeto Y Lo Modifica En La Tabla:
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Eliminar_Factura(Factura factura)
        {
            await _FacturaBL.Eliminar_Factura(factura);

            return RedirectToAction("Registros_Facturas", "Factura");
        }





        //         METODOS PARA AGREGAR O ELIMINAR DETALLES A LA FACTURA
        // **********************************************************************

        [HttpPost]
        public async Task<IActionResult> Agregar_Detalle(Factura factura, string accion)
        {
            //Agregamos Detalle A La Lista:
            factura.Lista_DetalleFactura.Add(new Detalle_Factura
            {
                Cantidad = 1,
                PrecioDel_Producto = 0
            });

            ViewData["Lista_Productos"] = await _FacturaBL.Lista_Productos();

            ViewBag.Accion = accion;
            return PartialView("_Detalle_Factura", factura.Lista_DetalleFactura);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar_Detalle(Factura factura, int index, string accion)
        {
            Detalle_Factura Objeto_Obtenido = factura.Lista_DetalleFactura[index];

            if (accion == "Editar_Factura" && Objeto_Obtenido.IdDetalleFactura > 0)
            {
                Objeto_Obtenido.IdDetalleFactura = Objeto_Obtenido.IdDetalleFactura * -1;
            }
            else
            {
                factura.Lista_DetalleFactura.RemoveAt(index);
            }

            ViewData["Lista_Productos"] = await _FacturaBL.Lista_Productos();

            ViewBag.Accion = accion;
            return PartialView("_Detalle_Factura", factura.Lista_DetalleFactura);
        }


    }
}
