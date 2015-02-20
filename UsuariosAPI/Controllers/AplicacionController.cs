using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UsuariosAPI.Controllers {

    public class AplicacionController : ApiController {

        // GET: usuariosAPI/aplicacion/0/1
        [Route("usuariosAPI/aplicacion/{id_aplicacion:int?}/{id_idioma:int?}")]
        [HttpGet]
        public IEnumerable<Usuarios_Objetos.Aplicacion> GetAplicacion(int id_aplicacion = 0, int id_idioma = 1) {
            return new Usuarios_DB.ServicioAplicacion().buscarAplicacion(id_aplicacion, id_idioma);
        }

        // GET: usuariosAPIsec/aplicacion/0/1
        [Route("usuariosAPIsec/aplicacion/{id_aplicacion:int?}/{id_idioma:int?}")]
        [HttpGet]
        public IEnumerable<Usuarios_Objetos.Sec_Aplicacion> GetSec_Aplicacion(int id_aplicacion = 0, int id_idioma = 1) {
            return new Usuarios_DB.ServicioAplicacion().buscarSec_Aplicacion(id_aplicacion, id_idioma);
        }

        // POST: usuariosAPI/aplicacion/i
        [Route("usuariosAPI/aplicacion/i")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostAplicacion_i(Usuarios_Objetos.Datos a) {
            return new Usuarios_DB.ServicioAplicacion().crearAplicacion(a.usuario_s, a.codigo, a.nombre, a.descripcion);
        }

        // POST: usuariosAPI/aplicacion/s
        [Route("usuariosAPI/aplicacion/s")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostAplicacion_s(Usuarios_Objetos.Datos a) {
            return new Usuarios_DB.ServicioAplicacion().crearAplicacion(a.sesion, a.codigo, a.nombre, a.descripcion);
        }

        // POST: usuariosAPIsec/aplicacion/i
        [Route("usuariosAPIsec/aplicacion/i")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostSec_Aplicacion_i(Usuarios_Objetos.Datos a) {
            return new Usuarios_DB.ServicioAplicacion().crearSec_Aplicacion(a.usuario_s, a.codigo, a.nombre, a.descripcion);
        }

        // POST: usuariosAPIsec/aplicacion/s
        [Route("usuariosAPIsec/aplicacion/s")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostSec_Aplicacion_s(Usuarios_Objetos.Datos a) {
            return new Usuarios_DB.ServicioAplicacion().crearSec_Aplicacion(a.sesion, a.codigo, a.nombre, a.descripcion);
        }

        // PUT: usuariosAPI/aplicacion/i/1/1/1
        [Route("usuariosAPI/aplicacion/i/{id_aplicacion:int}/{id_idioma:int}/{id_aplicacion_t:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutAplicacion_i(int id_aplicacion, int id_idioma, int id_aplicacion_t, Usuarios_Objetos.Datos a) {
            return new Usuarios_DB.ServicioAplicacion().modificarAplicacion(a.usuario_s, id_aplicacion, id_idioma, id_aplicacion_t, a.codigo, a.nombre, a.descripcion, a.activo);
        }

        // PUT: usuariosAPI/aplicacion/s/1/1/1
        [Route("usuariosAPI/aplicacion/s/{id_aplicacion:int}/{id_idioma:int}/{id_aplicacion_t:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutAplicacion_s(int id_aplicacion, int id_idioma, int id_aplicacion_t, Usuarios_Objetos.Datos a) {
            return new Usuarios_DB.ServicioAplicacion().modificarAplicacion(a.sesion, id_aplicacion, id_idioma, id_aplicacion_t, a.codigo, a.nombre, a.descripcion, a.activo);
        }

        // PUT: usuariosAPIsec/aplicacion/i/1/1/1
        [Route("usuariosAPIsec/aplicacion/i/{id_aplicacion:int}/{id_idioma:int}/{id_aplicacion_t:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutSec_Aplicacion_i(int id_aplicacion, int id_idioma, int id_aplicacion_t, Usuarios_Objetos.Datos a) {
            return new Usuarios_DB.ServicioAplicacion().modificarAplicacion(a.usuario_s, id_aplicacion, id_idioma, id_aplicacion_t, a.codigo, a.nombre, a.descripcion, a.activo);
        }

        // PUT: usuariosAPIsec/aplicacion/s/1/1/1
        [Route("usuariosAPIsec/aplicacion/s/{id_aplicacion:int}/{id_idioma:int}/{id_aplicacion_t:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutSec_Aplicacion_s(int id_aplicacion, int id_idioma, int id_aplicacion_t, Usuarios_Objetos.Datos a) {
            return new Usuarios_DB.ServicioAplicacion().modificarAplicacion(a.sesion, id_aplicacion, id_idioma, id_aplicacion_t, a.codigo, a.nombre, a.descripcion, a.activo);
        }

        // DELETE: usuarioAPI/aplicacion/1
        [Route("usuariosAPI/aplicacion/{id_aplicacion:int}")]
        [HttpDelete]
        public void DeleteAplicacion(int id_aplicacion) {

        }
    }
}