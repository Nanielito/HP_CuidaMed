using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios_DB {

    public static class ServicioUtilidades {

        public static int buscarIdUsuario(string sesion, UsuariosEntidades ue) {
            try {
                return buscarIdUsuarioPerfil(sesion, ue)[0] ?? -1;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        public static int buscarIdPerfil(string sesion, UsuariosEntidades ue) {
            try {
                return buscarIdUsuarioPerfil(sesion, ue)[1] ?? -1;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        public static N_Tupla<int?> buscarIdUsuarioPerfil(string sesion, UsuariosEntidades ue) {
            N_Tupla<int?> datos = null;

            try {
                datos = (
                    from _usuario in ue.Tb_Usuario_Perfil
                    join _sesion in ue.Tb_Sesion on _usuario.id_usuario equals _sesion.id_usuario
                    where _sesion.sesion == sesion && _sesion.activo == true
                    select new N_Tupla<int?>( new object[] { _usuario.id_usuario, _usuario.id_perfil })
                ).FirstOrDefault();

                return datos;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }
    }

    public class N_Tupla<T> {

        private int n;
        private object[] datos;

        public N_Tupla(object[] datos) {
            this.n     = datos.Length;
            this.datos = datos;
        }

        public int Count {
            get {
                return this.n;
            }
        }

        public T this[int indice] { 
            get {
                return (T)this.datos[indice];
            }
            set {
                this.datos[indice] = value;  
            }      
        }
    }
}
