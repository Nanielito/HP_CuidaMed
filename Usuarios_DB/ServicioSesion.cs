using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios_DB {

    /// <summary>
    /// Class to manage methods related to an application entity.
    /// </summary>
    public class ServicioSesion {

        /// <summary>
        /// The ue field represents an entity model for the UsuarioEntidades database.
        /// </summary>
        private UsuariosEntidades ue = null;

        /// <summary>
        /// The vResultado field represents an object parameter which is used when a stored procedures is invoked.
        /// </summary>
        private ObjectParameter vResultado = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServicioSesion"/> class and creates a database instance (UsuariosEntidades).
        /// </summary>
        public ServicioSesion() {
            ue = new UsuariosEntidades(
                Utilidades.AES.desencriptar(
                    System.Configuration.ConfigurationManager.ConnectionStrings["UsuariosEntidades"].ConnectionString
                )
            );
        }

        #region Normal

        public Usuarios_Objetos.Respuesta autenticarUsuario(string usuario, string contrasena) {
            Usuarios_Objetos.Respuesta res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Autenticar(
                    Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(usuario), contrasena),
                    Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(contrasena)),
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    res.mensaje = "OK";
                    res.objeto = ue.Tb_Sesion.Where(ts => ts.id_sesion == id)
                        .Select(s => new {
                            sesion        = s.sesion,
                            fechaInicio   = s.fechaInicio,
                            fechaUltimaOp = s.fechaUltimaOp
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sesion { 
                            sesion        = Utilidades.Hashing.SHA_256(e.sesion),
                            fechaInicio   = e.fechaInicio,
                            fechaUltimaOp = e.fechaUltimaOp
                        }).FirstOrDefault();
                }
                else
                    res.mensaje = vResultado.Value.ToString();

                return res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        public Usuarios_Objetos.Respuesta reautenticarUsuario(string sesion) {
            Usuarios_Objetos.Respuesta res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Reautenticar(
                    sesion,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    res.mensaje = "OK";
                    res.objeto = ue.Tb_Sesion.Where(ts => ts.id_sesion == id)
                        .Select(s => new {
                            sesion = s.sesion,
                            fechaInicio = s.fechaInicio,
                            fechaUltimaOp = s.fechaUltimaOp
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sesion {
                            sesion = Utilidades.Hashing.SHA_256(e.sesion),
                            fechaInicio = e.fechaInicio,
                            fechaUltimaOp = e.fechaUltimaOp
                        }).FirstOrDefault();
                }
                else
                    res.mensaje = vResultado.Value.ToString();

                return res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        #endregion

        #region Secure

        public Usuarios_Objetos.Respuesta autenticarSec_Usuario(string usuario, string contrasena) {
            Usuarios_Objetos.Respuesta res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Autenticar(
                    Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(usuario), contrasena),
                    Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(contrasena)),
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    res.mensaje = "OK";
                    res.objeto = ue.Tb_Sesion.Where(ts => ts.id_sesion == id)
                        .Select(s => new {
                            sesion        = s.sesion,
                            fechaInicio   = s.fechaInicio,
                            fechaUltimaOp = s.fechaUltimaOp
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Sesion(
                            0,
                            e.sesion,
                            e.fechaInicio,
                            e.fechaUltimaOp,
                            true
                        )).FirstOrDefault();
                }
                else
                    res.mensaje = vResultado.Value.ToString();

                return res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        public Usuarios_Objetos.Respuesta reautenticarSec_Usuario(string sesion) {
            Usuarios_Objetos.Respuesta res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Reautenticar(
                    sesion,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    res.mensaje = "OK";
                    res.objeto = ue.Tb_Sesion.Where(ts => ts.id_sesion == id)
                        .Select(s => new {
                            sesion = s.sesion,
                            fechaInicio = s.fechaInicio,
                            fechaUltimaOp = s.fechaUltimaOp
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Sesion(
                            0,
                            e.sesion,
                            e.fechaInicio,
                            e.fechaUltimaOp,
                            true
                        )).FirstOrDefault();
                }
                else
                    res.mensaje = vResultado.Value.ToString();

                return res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        #endregion
    }
}