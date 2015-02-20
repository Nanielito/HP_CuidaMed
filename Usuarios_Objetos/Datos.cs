using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios_Objetos {

    public class Datos {

        public string nombre      { get; set; }

        public string codigo      { get; set; }

        public string descripcion { get; set; }

        public string usuario_u   { get; set; } //user

        public string usuario_m   { get; set; } //email

        public string usuario_p   { get; set; } //password

        public string usuario_np  { get; set; } //new password

        public string sesion      { get; set; }

        public int    usuario_s   { get; set; }

        public int    usuario     { get; set; }

        public int    aplicacion  { get; set; }

        public int    grupo       { get; set; }

        public int    perfil      { get; set; }

        public bool   activo      { get; set; }
    }
}