using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UsuariosAPI.Controllers {

    public class SesionController : ApiController {
        
        // POST: usuariosAPI/sesion/autenticar
        [Route("usuariosAPI/sesion/autenticar")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostAutenticarUsuario(Usuarios_Objetos.Datos u) {
            return new Usuarios_DB.ServicioSesion().autenticarUsuario(u.usuario_u, u.usuario_p);
        }

        // POST: usuariosAPIsec/sesion/autenticar
        [Route("usuariosAPIsec/sesion/autenticar")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostAutenticarSec_Usuario(Usuarios_Objetos.Datos u) {
            return new Usuarios_DB.ServicioSesion().autenticarSec_Usuario(u.usuario_u, u.usuario_p);
        }

        // POST: usuariosAPI/sesion/reautenticar
        [Route("usuariosAPI/sesion/reautenticar")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostReautenticarUsuario(Usuarios_Objetos.Datos u) {
            return new Usuarios_DB.ServicioSesion().reautenticarUsuario(u.sesion);
        }

        // POST: usuariosAPIsec/sesion/reautenticar
        [Route("usuariosAPIsec/sesion/reautenticar")]
        [HttpPost]
        public Usuarios_Objetos.Respuesta PostReautenticarSec_Usuario(Usuarios_Objetos.Datos u) {
            return new Usuarios_DB.ServicioSesion().reautenticarSec_Usuario(u.sesion);
        } 
    }
}
