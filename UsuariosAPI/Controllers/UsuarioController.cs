using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UsuariosAPI.Controllers {

    public class UsuarioController : ApiController {

        // GET: usuariosAPI/admin
        [Route("usuariosAPI/admin")]
        [HttpGet]
        public Usuarios_Objetos.Respuesta PostAdmin() {
            return new Usuarios_DB.ServicioUsuario().crearUsuarioAdministrador();
        }

        // GET: usuariosAPI/usuario/test
        [Route("usuariosAPI/usuario/{usuario}")]
        [HttpGet]
        public bool GetExisteUsuario(string usuario) {
            return new Usuarios_DB.ServicioUsuario().existeUsuario(usuario);
        }
        
        // GET: usuariosAPI/correo/test@test.com
        [Route("usuariosAPI/correo/{correo}")]
        [HttpGet]
        public bool GetExisteCorreo(string correo) {
            return new Usuarios_DB.ServicioUsuario().existeCorreo(correo);
        }

        // GET: usuariosAPI/usuario/i/1
        [Route("usuariosAPI/usuario/i/{id_usuario:int?}")]
        [HttpGet]
        public IEnumerable<Usuarios_Objetos.Usuario> GetUsuario_i(int id_usuario = 0) {
            return new Usuarios_DB.ServicioUsuario().buscarUsuario(id_usuario);
        }

        // GET: usuariosAPI/usuario/s/1
        [Route("usuariosAPI/usuario/s/{sesion}")]
        [HttpGet]
        public IEnumerable<Usuarios_Objetos.Usuario> GetUsuario_s(string sesion) {
            return new Usuarios_DB.ServicioUsuario().buscarUsuario(sesion);
        }

        // GET: usuariosAPIsec/usuario/1
        [Route("usuariosAPIsec/usuario/{id_usuario:int?}")]
        [HttpGet]
        public IEnumerable<Usuarios_Objetos.Sec_Usuario> GetSec_Usuario(int id_usuario = 0) {
            return new Usuarios_DB.ServicioUsuario().buscarSec_Usuario(id_usuario);
        }

        // POST: usuariosAPI/usuario/i
        [Route("usuariosAPI/usuario/i")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostUsuario_i(Usuarios_Objetos.Datos u) {
            return new Usuarios_DB.ServicioUsuario().crearUsuario(u.usuario_s, u.usuario_u, u.usuario_m, u.usuario_p, u.activo);
        }

        // POST: usuariosAPI/usuario/s
        [Route("usuariosAPI/usuario/s")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostUsuario_s(Usuarios_Objetos.Datos u) {
            return new Usuarios_DB.ServicioUsuario().crearUsuario(u.sesion, u.usuario_u, u.usuario_m, u.usuario_p, u.activo);
        }

        // POST: usuariosAPIsec/usuario/i
        [Route("usuariosAPIsec/usuario/i")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostSec_Usuario_i(Usuarios_Objetos.Datos u) {
            return new Usuarios_DB.ServicioUsuario().crearSec_Usuario(u.usuario_s, u.usuario_u, u.usuario_m, u.usuario_p, u.activo);
        }

        // POST: usuariosAPIsec/usuario/s
        [Route("usuariosAPIsec/usuario/s")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostSec_Usuario_s(Usuarios_Objetos.Datos u) {
            return new Usuarios_DB.ServicioUsuario().crearSec_Usuario(u.sesion, u.usuario_u, u.usuario_m, u.usuario_p, u.activo);
        }

        // PUT: usuariosAPI/contrasena/i/1
        [Route("usuariosAPI/contrasena/i/{id_usuario:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutContrasena_i(int id_usuario, Usuarios_Objetos.Datos u) {
            return new Usuarios_DB.ServicioUsuario().modificarContrasena(u.usuario_s, id_usuario, u.usuario_p, u.usuario_np);
        }

        // PUT: usuariosAPI/contrasena/s/1
        [Route("usuariosAPI/contrasena/s/{id_usuario:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutContrasena_s(int id_usuario, Usuarios_Objetos.Datos u) {
            return new Usuarios_DB.ServicioUsuario().modificarContrasena(u.sesion, id_usuario, u.usuario_p, u.usuario_np);
        }

        // PUT: usuariosAPIsec/contrasena/i/1
        [Route("usuariosAPIsec/contrasena/i/{id_usuario:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutSec_Contrasena_i(int id_usuario, Usuarios_Objetos.Datos u) {
            return new Usuarios_DB.ServicioUsuario().modificarSec_Contrasena(u.usuario_s, id_usuario, u.usuario_p, u.usuario_np);
        }

        // PUT: usuariosAPIsec/contrasena/s/1
        [Route("usuariosAPIsec/contrasena/s/{id_usuario:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutSec_Contrasena_s(int id_usuario, Usuarios_Objetos.Datos u) {
            return new Usuarios_DB.ServicioUsuario().modificarSec_Contrasena(u.sesion, id_usuario, u.usuario_p, u.usuario_np);
        }

        // PUT: usuariosAPI/usuario/i/1
        [Route("usuariosAPI/usuario/i/{id_usuario:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutUsuario_i(int id_usuario, Usuarios_Objetos.Datos u) {
            return new Usuarios_DB.ServicioUsuario().modificarUsuario(u.usuario_s, id_usuario, u.usuario_u, u.usuario_m, u.usuario_p, u.activo);
        }

        // PUT: usuariosAPI/usuario/s/1
        [Route("usuariosAPI/usuario/s/{id_usuario:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutUsuario_s(int id_usuario, Usuarios_Objetos.Datos u) {
            return new Usuarios_DB.ServicioUsuario().modificarUsuario(u.sesion, id_usuario, u.usuario_u, u.usuario_m, u.usuario_p, u.activo);
        }

        // PUT: usuariosAPIsec/usuario/i/1
        [Route("usuariosAPIsec/usuario/i/{id_usuario:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutSec_Usuario_i(int id_usuario, Usuarios_Objetos.Datos u) {
            return new Usuarios_DB.ServicioUsuario().modificarUsuario(u.usuario_s, id_usuario, u.usuario_u, u.usuario_m, u.usuario_p, u.activo);
        }

        // PUT: usuariosAPIsec/usuario/s/1
        [Route("usuariosAPIsec/usuario/s/{id_usuario:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutSec_Usuario_s(int id_usuario, Usuarios_Objetos.Datos u) {
            return new Usuarios_DB.ServicioUsuario().modificarUsuario(u.sesion, id_usuario, u.usuario_u, u.usuario_m, u.usuario_p, u.activo);
        }

        // POST: usuariosAPI/usuario_aplicacion/i
        [Route("usuariosAPI/usuario_aplicacion/i")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostUsuario_Aplicacion_i(Usuarios_Objetos.Datos ua) {
            return new Usuarios_DB.ServicioUsuario().crearUsuario_Aplicacion(ua.usuario_s, ua.usuario, ua.aplicacion, ua.activo);
        }

        // POST: usuariosAPI/usuario_aplicacion/s
        [Route("usuariosAPI/usuario_aplicacion/s")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostUsuario_Aplicacion_s(Usuarios_Objetos.Datos ua) {
            return new Usuarios_DB.ServicioUsuario().crearUsuario_Aplicacion(ua.sesion, ua.usuario, ua.aplicacion, ua.activo);
        }

        // POST: usuariosAPIsec/usuario_aplicacion/i
        [Route("usuariosAPIsec/usuario_aplicacion/i")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostSec_Usuario_Aplicacion_i(Usuarios_Objetos.Datos ua) {
            return new Usuarios_DB.ServicioUsuario().crearSec_Usuario_Aplicacion(ua.usuario_s, ua.usuario, ua.aplicacion, ua.activo);
        }

        // POST: usuariosAPIsec/usuario_aplicacion/s
        [Route("usuariosAPIsec/usuario_aplicacion/s")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostSec_Usuario_Aplicacion_s(Usuarios_Objetos.Datos ua) {
            return new Usuarios_DB.ServicioUsuario().crearSec_Usuario_Aplicacion(ua.sesion, ua.usuario, ua.aplicacion, ua.activo);
        }

        // PUT: usuariosAPI/usuario_aplicacion/i/1
        [Route("usuariosAPI/usuario_aplicacion/i/{id_usuarioAplicacion:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutUsuario_Aplicacion_i(int id_usuarioAplicacion, Usuarios_Objetos.Datos ua) {
            return new Usuarios_DB.ServicioUsuario().modificarUsuario_Aplicacion(ua.usuario_s, id_usuarioAplicacion, ua.usuario, ua.aplicacion, ua.activo);
        }

        // PUT: usuariosAPI/usuario_aplicacion/s/1
        [Route("usuariosAPI/usuario_aplicacion/s/{id_usuarioAplicacion:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutUsuario_Aplicacion_s(int id_usuarioAplicacion, Usuarios_Objetos.Datos ua) {
            return new Usuarios_DB.ServicioUsuario().modificarUsuario_Aplicacion(ua.sesion, id_usuarioAplicacion, ua.usuario, ua.aplicacion, ua.activo);
        }

        // PUT: usuariosAPIsec/usuario_aplicacion/i/1
        [Route("usuariosAPIsec/usuario_aplicacion/i/{id_usuarioAplicacion:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutSec_Usuario_Aplicacion_i(int id_usuarioAplicacion, Usuarios_Objetos.Datos ua) {
            return new Usuarios_DB.ServicioUsuario().modificarUsuario_Aplicacion(ua.usuario_s, id_usuarioAplicacion, ua.usuario, ua.aplicacion, ua.activo);
        }

        // PUT: usuariosAPIsec/usuario_aplicacion/s/1
        [Route("usuariosAPIsec/usuario_aplicacion/s/{id_usuarioAplicacion:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutSec_Usuario_Aplicacion_s(int id_usuarioAplicacion, Usuarios_Objetos.Datos ua) {
            return new Usuarios_DB.ServicioUsuario().modificarUsuario_Aplicacion(ua.sesion, id_usuarioAplicacion, ua.usuario, ua.aplicacion, ua.activo);
        }

        // POST: usuariosAPI/usuario_grupo/i
        [Route("usuariosAPI/usuario_grupo/i")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostUsuario_Grupo_i(Usuarios_Objetos.Datos ua) {
            return new Usuarios_DB.ServicioUsuario().crearUsuario_Grupo(ua.usuario_s, ua.usuario, ua.grupo, ua.activo);
        }

        // POST: usuariosAPI/usuario_grupo/s
        [Route("usuariosAPI/usuario_grupo/s")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostUsuario_Grupo_s(Usuarios_Objetos.Datos ua) {
            return new Usuarios_DB.ServicioUsuario().crearUsuario_Grupo(ua.sesion, ua.usuario, ua.grupo, ua.activo);
        }

        // POST: usuariosAPIsec/usuario_grupo/i
        [Route("usuariosAPIsec/usuario_grupo/i")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostSec_Usuario_Grupo_i(Usuarios_Objetos.Datos ua) {
            return new Usuarios_DB.ServicioUsuario().crearSec_Usuario_Grupo(ua.usuario_s, ua.usuario, ua.grupo, ua.activo);
        }

        // POST: usuariosAPIsec/usuario_grupo/s
        [Route("usuariosAPIsec/usuario_grupo/s")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostSec_Usuario_Grupo_s(Usuarios_Objetos.Datos ua) {
            return new Usuarios_DB.ServicioUsuario().crearSec_Usuario_Grupo(ua.sesion, ua.usuario, ua.grupo, ua.activo);
        }

        // PUT: usuariosAPI/usuario_grupo/i/1
        [Route("usuariosAPI/usuario_grupo/i/{id_usuarioGrupo:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutUsuario_Grupo_i(int id_usuarioGrupo, Usuarios_Objetos.Datos ug) {
            return new Usuarios_DB.ServicioUsuario().modificarUsuario_Grupo(ug.usuario_s, id_usuarioGrupo, ug.usuario, ug.grupo, ug.activo);
        }

        // PUT: usuariosAPI/usuario_grupo/s/1
        [Route("usuariosAPI/usuario_grupo/s/{id_usuarioGrupo:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutUsuario_Grupo_s(int id_usuarioGrupo, Usuarios_Objetos.Datos ug) {
            return new Usuarios_DB.ServicioUsuario().modificarUsuario_Grupo(ug.sesion, id_usuarioGrupo, ug.usuario, ug.grupo, ug.activo);
        }

        // PUT: usuariosAPIsec/usuario_grupo/i/1
        [Route("usuariosAPIsec/usuario_grupo/i/{id_usuarioGrupo:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutSec_Usuario_Grupo_i(int id_usuarioGrupo, Usuarios_Objetos.Datos ug) {
            return new Usuarios_DB.ServicioUsuario().modificarUsuario_Grupo(ug.usuario_s, id_usuarioGrupo, ug.usuario, ug.grupo, ug.activo);
        }

        // PUT: usuariosAPIsec/usuario_grupo/s/1
        [Route("usuariosAPIsec/usuario_grupo/s/{id_usuarioGrupo:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutSec_Usuario_Grupo_s(int id_usuarioGrupo, Usuarios_Objetos.Datos ug) {
            return new Usuarios_DB.ServicioUsuario().modificarUsuario_Grupo(ug.sesion, id_usuarioGrupo, ug.usuario, ug.grupo, ug.activo);
        }

        // POST: usuariosAPI/usuario_perfil/i
        [Route("usuariosAPI/usuario_perfil/i")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostUsuario_Perfil_i(Usuarios_Objetos.Datos up) {
            return new Usuarios_DB.ServicioUsuario().crearUsuario_Perfil(up.usuario_s, up.usuario, up.perfil, up.activo);
        }

        // POST: usuariosAPI/usuario_perfil/s
        [Route("usuariosAPI/usuario_perfil/s")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostUsuario_Perfil_s(Usuarios_Objetos.Datos up) {
            return new Usuarios_DB.ServicioUsuario().crearUsuario_Perfil(up.sesion, up.usuario, up.perfil, up.activo);
        }

        // POST: usuariosAPIsec/usuario_perfil/i
        [Route("usuariosAPIsec/usuario_perfil/i")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostSec_Usuario_Perfil_i(Usuarios_Objetos.Datos up) {
            return new Usuarios_DB.ServicioUsuario().crearSec_Usuario_Perfil(up.usuario_s, up.usuario, up.perfil, up.activo);
        }

        // POST: usuariosAPIsec/usuario_perfil/s
        [Route("usuariosAPIsec/usuario_perfil/s")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostSec_Usuario_Perfil_s(Usuarios_Objetos.Datos up) {
            return new Usuarios_DB.ServicioUsuario().crearSec_Usuario_Perfil(up.sesion, up.usuario, up.perfil, up.activo);
        }

        // PUT: usuariosAPI/usuario_perfil/i/1
        [Route("usuariosAPI/usuario_perfil/i/{id_usuarioPerfil:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutUsuario_Perfil_i(int id_usuarioPerfil, Usuarios_Objetos.Datos up) {
            return new Usuarios_DB.ServicioUsuario().modificarUsuario_Perfil(up.usuario_s, id_usuarioPerfil, up.usuario, up.perfil, up.activo);
        }

        // PUT: usuariosAPI/usuario_perfil/s/1
        [Route("usuariosAPI/usuario_perfil/s/{id_usuarioPerfil:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutUsuario_Perfil_s(int id_usuarioPerfil, Usuarios_Objetos.Datos up) {
            return new Usuarios_DB.ServicioUsuario().modificarUsuario_Perfil(up.sesion, id_usuarioPerfil, up.usuario, up.perfil, up.activo);
        }

        // PUT: usuariosAPIsec/usuario_perfil/i/1
        [Route("usuariosAPIsec/usuario_perfil/i/{id_usuarioPerfil:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutSec_Usuario_Perfil_i(int id_usuarioPerfil, Usuarios_Objetos.Datos up) {
            return new Usuarios_DB.ServicioUsuario().modificarUsuario_Perfil(up.usuario_s, id_usuarioPerfil, up.usuario, up.perfil, up.activo);
        }

        // PUT: usuariosAPIsec/usuario_perfil/s/1
        [Route("usuariosAPIsec/usuario_perfil/s/{id_usuarioPerfil:int}")]
        [HttpPut]
        public Usuarios_Objetos.Respuesta PutSec_Usuario_Perfil_s(int id_usuarioPerfil, Usuarios_Objetos.Datos up) {
            return new Usuarios_DB.ServicioUsuario().modificarUsuario_Perfil(up.sesion, id_usuarioPerfil, up.usuario, up.perfil, up.activo);
        }

        // DELETE: usuarioAPI/usuario/1
        [Route("usuariosAPI/usuario/{id_usuario:int}")]
        [HttpDelete]
        public void DeleteUsuario(int id_usuario) {

        }
    }
}
