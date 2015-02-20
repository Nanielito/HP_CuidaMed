using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios_Objetos {

    public class Usuario {

        public int    id_usuario    { get; set; }

        public string usuario       { get; set; }

        public string usuarioCorreo { get; set; }

        public string contrasena    { get; set; }

        public int    id_perfil     { get; set; }

        public bool   activo        { get; set; }

        public List<Aplicacion> aplicaciones { get; set; }

        public List<Grupo>      grupos       { get; set; }
    }

    public class Sec_Usuario {

        public string[] datos;

        public List<Sec_Aplicacion> aplicaciones;

        public List<Sec_Grupo>      grupos;

        public Sec_Usuario(int id_usuario, string usuario, string usuarioCorreo, string contrasena, int id_perfil, bool activo, List<Sec_Aplicacion> aplicaciones, List<Sec_Grupo> grupos) {
            this.datos = new string[] {
                Utilidades.FuncionesGlobales.validar_tostr(id_usuario.ToString(), Utilidades.FuncionesGlobales.encrypt.AES),
                Utilidades.FuncionesGlobales.validar_tostr(usuario),
                Utilidades.FuncionesGlobales.validar_tostr(usuarioCorreo),
                Utilidades.FuncionesGlobales.validar_tostr(contrasena),
                Utilidades.FuncionesGlobales.validar_tostr(id_perfil.ToString(), Utilidades.FuncionesGlobales.encrypt.AES),
                Utilidades.FuncionesGlobales.validar_tostr(activo.ToString())
            };
            this.aplicaciones = aplicaciones;
            this.grupos       = grupos;
        }
    }
}