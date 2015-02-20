using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios_Objetos {

    public class Perfil {

        public int    id_perfil   { get; set; }

        public string codigo      { get; set; }

        public int    id_perfil_t { get; set; }

        public string perfil      { get; set; }

        public string descripcion { get; set; }

        public int    id_idioma   { get; set; }

        public bool   activo      { get; set; }
    }

    public class Sec_Perfil {

        public string[] datos;

        public Sec_Perfil(int id_perfil, string codigo, int id_perfil_t, string perfil, string descripcion, int id_idioma, bool activo) {
            this.datos = new string[] {
                Utilidades.FuncionesGlobales.validar_tostr(id_perfil.ToString(), Utilidades.FuncionesGlobales.encrypt.AES),
                Utilidades.FuncionesGlobales.validar_tostr(codigo),
                Utilidades.FuncionesGlobales.validar_tostr(id_perfil_t.ToString(), Utilidades.FuncionesGlobales.encrypt.AES),
                Utilidades.FuncionesGlobales.validar_tostr(perfil),
                Utilidades.FuncionesGlobales.validar_tostr(descripcion),
                Utilidades.FuncionesGlobales.validar_tostr(id_idioma.ToString(), Utilidades.FuncionesGlobales.encrypt.AES),
                Utilidades.FuncionesGlobales.validar_tostr(activo.ToString())
            };
        }
    }
}