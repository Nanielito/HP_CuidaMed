using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios_Objetos {

    public class Sesion {

        public int      id_sesion     { get; set; }

        public string   sesion        { get; set; }

        public DateTime fechaInicio   { get; set; }

        public DateTime fechaUltimaOp { get; set; }

        public bool     activo        { get; set; }
    }

    public class Sec_Sesion {

        public string[] datos;

        public Sec_Sesion(int id_sesion, string sesion, DateTime fechaInicio, DateTime fechaUltimaOp, bool activo) {
            this.datos = new string[] {
                Utilidades.FuncionesGlobales.validar_tostr(id_sesion.ToString(), Utilidades.FuncionesGlobales.encrypt.AES),
                Utilidades.FuncionesGlobales.validar_tostr(sesion, Utilidades.FuncionesGlobales.encrypt.SHA256),
                Utilidades.FuncionesGlobales.validar_tostr(fechaInicio.ToString()),
                Utilidades.FuncionesGlobales.validar_tostr(fechaUltimaOp.ToString()),
                Utilidades.FuncionesGlobales.validar_tostr(activo.ToString())
            };
        }
    }
}