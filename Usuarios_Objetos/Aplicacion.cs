using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios_Objetos {

    public class Aplicacion {

        public int    id_aplicacion   { get; set; }

        public string codigo          { get; set; }

        public int    id_aplicacion_t { get; set; }

        public string aplicacion      { get; set; }

        public string descripcion     { get; set; }

        public int    id_idioma       { get; set; }

        public int    id_usuario      { get; set; }

        public bool   activo          { get; set; }
    }

    public class Sec_Aplicacion {

        public string[] datos;

        public Sec_Aplicacion(int id_aplicacion, string codigo, int id_aplicacion_t, string aplicacion, string descripcion, int id_idioma, int id_usuario, bool activo) {
            this.datos = new string[] {
                Utilidades.FuncionesGlobales.validar_tostr(id_aplicacion.ToString(), Utilidades.FuncionesGlobales.encrypt.AES),
                Utilidades.FuncionesGlobales.validar_tostr(codigo),
                Utilidades.FuncionesGlobales.validar_tostr(id_aplicacion_t.ToString(), Utilidades.FuncionesGlobales.encrypt.AES),
                Utilidades.FuncionesGlobales.validar_tostr(aplicacion),
                Utilidades.FuncionesGlobales.validar_tostr(descripcion),
                Utilidades.FuncionesGlobales.validar_tostr(id_idioma.ToString(), Utilidades.FuncionesGlobales.encrypt.AES),
                Utilidades.FuncionesGlobales.validar_tostr(id_usuario.ToString(), Utilidades.FuncionesGlobales.encrypt.AES),
                Utilidades.FuncionesGlobales.validar_tostr(activo.ToString())
            };
        }
    }
}