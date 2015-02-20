using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UsuariosAPI.Controllers {

    public class PerfilController : ApiController {

        // GET: usuariosAPI/perfil/0/1
        [Route("usuariosAPI/perfil/{id_perfil:int?}/{id_idioma:int?}")]
        [HttpGet]
        public IEnumerable<Usuarios_Objetos.Perfil> GetPerfil(int id_perfil = 0, int id_idioma = 1) {
            return new Usuarios_DB.ServicioPerfil().buscarPerfil(id_perfil, id_idioma);
        }

        // GET: usuariosAPIsec/perfil/0/1
        [Route("usuariosAPIsec/perfil/{id_perfil:int?}/{id_idioma:int?}")]
        [HttpGet]
        public IEnumerable<Usuarios_Objetos.Sec_Perfil> GetSec_Perfil(int id_perfil = 0, int id_idioma = 1) {
            return new Usuarios_DB.ServicioPerfil().buscarSec_Perfil(id_perfil, id_idioma);
        }

        // POST: usuariosAPI/perfil/i
        [Route("usuariosAPI/perfil/i")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostPerfil_i(Usuarios_Objetos.Datos p) {
            return new Usuarios_DB.ServicioPerfil().crearPerfil(p.usuario, p.codigo, p.nombre, p.descripcion);
        }

        // POST: usuariosAPI/perfil/s
        [Route("usuariosAPI/perfil/s")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostPerfil_s(Usuarios_Objetos.Datos p) {
            return new Usuarios_DB.ServicioPerfil().crearPerfil(p.sesion, p.codigo, p.nombre, p.descripcion);
        }

        // POST: usuariosAPIsec/perfil/i
        [Route("usuariosAPIsec/perfil/i")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostSec_Perfil_i(Usuarios_Objetos.Datos p) {
            return new Usuarios_DB.ServicioPerfil().crearSec_Perfil(p.usuario, p.codigo, p.nombre, p.descripcion);
        }

        // POST: usuariosAPIsec/perfil/s
        [Route("usuariosAPIsec/perfil/s")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostSec_Perfil_s(Usuarios_Objetos.Datos p) {
            return new Usuarios_DB.ServicioPerfil().crearSec_Perfil(p.sesion, p.codigo, p.nombre, p.descripcion);
        }

        // PUT: usuariosAPI/perfil/i/1/1/1
        [Route("usuariosAPI/perfil/i/{id_perfil:int}/{id_idioma:int}/{id_perfil_t:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutPerfil_i(int id_perfil, int id_idioma, int id_perfil_t, Usuarios_Objetos.Datos p) {
            return new Usuarios_DB.ServicioPerfil().modificarPerfil(p.usuario, id_perfil, id_idioma, id_perfil_t, p.codigo, p.nombre, p.descripcion, p.activo);
        }

        // PUT: usuariosAPI/perfil/s/1/1/1
        [Route("usuariosAPI/perfil/s/{id_perfil:int}/{id_idioma:int}/{id_perfil_t:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutPerfil_s(int id_perfil, int id_idioma, int id_perfil_t, Usuarios_Objetos.Datos p) {
            return new Usuarios_DB.ServicioPerfil().modificarPerfil(p.sesion, id_perfil, id_idioma, id_perfil_t, p.codigo, p.nombre, p.descripcion, p.activo);
        }

        // PUT: usuariosAPIsec/perfil/i/1/1/1
        [Route("usuariosAPIsec/perfil/i/{id_perfil:int}/{id_idioma:int}/{id_perfil_t:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutSec_Perfil_i(int id_perfil, int id_idioma, int id_perfil_t, Usuarios_Objetos.Datos p) {
            return new Usuarios_DB.ServicioPerfil().modificarPerfil(p.usuario, id_perfil, id_idioma, id_perfil_t, p.codigo, p.nombre, p.descripcion, p.activo);
        }

        // PUT: usuariosAPIsec/perfil/s/1/1/1
        [Route("usuariosAPIsec/perfil/s/{id_perfil:int}/{id_idioma:int}/{id_perfil_t:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutSec_Perfil_s(int id_perfil, int id_idioma, int id_perfil_t, Usuarios_Objetos.Datos p) {
            return new Usuarios_DB.ServicioPerfil().modificarPerfil(p.sesion, id_perfil, id_idioma, id_perfil_t, p.codigo, p.nombre, p.descripcion, p.activo);
        }

        // DELETE: usuarioAPI/perfil/1
        [Route("usuariosAPI/perfil/{id_perfil:int}")]
        [HttpDelete]
        public void DeleteAplicacion(int id_aplicacion) {

        }
    }
}