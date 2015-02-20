using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios_Objetos {

    public class Grupo {

        public int    id_grupo      { get; set; }

        public string codigo        { get; set; }

        public string grupo         { get; set; }

        public string descripcion   { get; set; }

        public int    id_aplicacion { get; set; }

        public bool   activo        { get; set; }
    }

    public class Sec_Grupo {

        public string[] datos;

        public Sec_Grupo(int id_grupo, string codigo, string grupo, string descripcion, int id_aplicacion, bool activo) {
            this.datos = new string[] {
                Utilidades.FuncionesGlobales.validar_tostr(id_grupo.ToString(), Utilidades.FuncionesGlobales.encrypt.AES),
                Utilidades.FuncionesGlobales.validar_tostr(codigo),
                Utilidades.FuncionesGlobales.validar_tostr(grupo),
                Utilidades.FuncionesGlobales.validar_tostr(descripcion),
                Utilidades.FuncionesGlobales.validar_tostr(id_aplicacion.ToString(), Utilidades.FuncionesGlobales.encrypt.AES),
                Utilidades.FuncionesGlobales.validar_tostr(activo.ToString())
            };
        }
    }
}