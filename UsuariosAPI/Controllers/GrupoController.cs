using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UsuariosAPI.Controllers {

    public class GrupoController : ApiController {

        // GET: usuariosAPI/grupo/0
        [Route("usuariosAPI/grupo/{id_grupo:int?}")]
        [HttpGet]
        public IEnumerable<Usuarios_Objetos.Grupo> GetGrupo(int id_grupo = 0) {
            return new Usuarios_DB.ServicioGrupo().buscarGrupo(id_grupo);
        }

        // GET: usuariosAPIsec/grupo/0
        [Route("usuariosAPIsec/grupo/{id_grupo:int?}")]
        [HttpGet]
        public IEnumerable<Usuarios_Objetos.Sec_Grupo> GetSec_Grupo(int id_grupo = 0) {
            return new Usuarios_DB.ServicioGrupo().buscarSec_Grupo(id_grupo);
        }

        // POST: usuariosAPI/grupo/i
        [Route("usuariosAPI/grupo/i")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostGrupo_i(Usuarios_Objetos.Datos g) {
            return new Usuarios_DB.ServicioGrupo().crearGrupo(g.usuario_s, g.aplicacion, g.codigo, g.nombre, g.descripcion);
        }

        // POST: usuariosAPI/grupo/s
        [Route("usuariosAPI/grupo/s")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostGrupo_s(Usuarios_Objetos.Datos g) {
            return new Usuarios_DB.ServicioGrupo().crearGrupo(g.sesion, g.aplicacion, g.codigo, g.nombre, g.descripcion);
        }

        // POST: usuariosAPIsec/grupo/i
        [Route("usuariosAPIsec/grupo/i")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostSec_Grupo_i(Usuarios_Objetos.Datos g) {
            return new Usuarios_DB.ServicioGrupo().crearSec_Grupo(g.usuario_s, g.aplicacion, g.codigo, g.nombre, g.descripcion);
        }

        // POST: usuariosAPIsec/grupo/s
        [Route("usuariosAPIsec/grupo/s")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostSec_Grupo_s(Usuarios_Objetos.Datos g) {
            return new Usuarios_DB.ServicioGrupo().crearSec_Grupo(g.sesion, g.aplicacion, g.codigo, g.nombre, g.descripcion);
        }

        // PUT: usuariosAPI/grupo/i/1
        [Route("usuariosAPI/grupo/i/{id_grupo:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutGrupo_i(int id_grupo, Usuarios_Objetos.Datos g) {
            return new Usuarios_DB.ServicioGrupo().modificarGrupo(g.usuario_s, id_grupo, g.codigo, g.nombre, g.nombre, g.activo);
        }

        // PUT: usuariosAPI/grupo/s/1
        [Route("usuariosAPI/grupo/s/{id_grupo:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutGrupo_s(int id_grupo, Usuarios_Objetos.Datos g) {
            return new Usuarios_DB.ServicioGrupo().modificarSec_Grupo(g.sesion, id_grupo, g.codigo, g.nombre, g.nombre, g.activo);
        }

        // PUT: usuariosAPIsec/grupo/i/1
        [Route("usuariosAPIsec/grupo/i/{id_grupo:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutSec_Grupo_i(int id_grupo, Usuarios_Objetos.Datos g) {
            return new Usuarios_DB.ServicioGrupo().modificarGrupo(g.usuario_s, id_grupo, g.codigo, g.nombre, g.descripcion, g.activo);
        }

        // PUT: usuariosAPIsec/grupo/s/1
        [Route("usuariosAPIsec/grupo/s/{id_grupo:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutSec_Grupo_s(int id_grupo, Usuarios_Objetos.Datos g) {
            return new Usuarios_DB.ServicioGrupo().modificarSec_Grupo(g.sesion, id_grupo, g.codigo, g.nombre, g.descripcion, g.activo);
        }

        // DELETE: usuarioAPI/grupo/1
        [Route("usuariosAPI/grupo/{id_grupo:int}")]
        [HttpDelete]
        public void DeleteGrupo(int id_grupo) {

        }
    }
}