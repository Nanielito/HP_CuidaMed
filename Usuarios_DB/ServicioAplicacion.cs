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
    public class ServicioAplicacion {

        /// <summary>
        /// The ue field represents an entity model for the UsuarioEntidades database.
        /// </summary>
        private UsuariosEntidades ue = null;

        /// <summary>
        /// The vResultado field represents an object parameter which is used when a stored procedures is invoked.
        /// </summary>
        private ObjectParameter vResultado = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServicioAplicacion"/> class and creates a database instance (UsuariosEntidades).
        /// </summary>
        public ServicioAplicacion() {
            ue = new UsuariosEntidades(
                Utilidades.AES.desencriptar(
                    System.Configuration.ConfigurationManager.ConnectionStrings["UsuariosEntidades"].ConnectionString
                )
            );
        }

        #region Normal

        /// <summary>
        /// Gets information about an application or all applications registered in the database.
        /// </summary>
        /// <param name="id_aplicacion">
        /// An integer identifier related to an application. If set to a value less than or equal to <c>0</c> the method searchs all applications.
        /// </param>
        /// <param name="id_idioma">
        /// An integer identifier related to a language.
        /// </param>
        /// <returns>
        /// A list that contains all infotmation of the application/applications registered in the database.
        /// </returns>
        public List<Usuarios_Objetos.Aplicacion> buscarAplicacion(int id_aplicacion, int id_idioma) {
            try {
                if (id_aplicacion <= 0)
                    return ue.Tb_Aplicacion_T.Where(ta => ta.id_idioma == id_idioma)
                        .Select(s => new Usuarios_Objetos.Aplicacion {
                            id_aplicacion   = s.id_aplicacion,
                            codigo          = s.Tb_Aplicacion.codigo,
                            id_aplicacion_t = s.id_aplicacion_T,
                            aplicacion      = s.aplicacion,
                            descripcion     = s.descripcion,
                            id_idioma       = s.id_idioma,
                            activo          = s.activo
                        }).ToList();
                else
                    return ue.Tb_Aplicacion_T.Where(ta => ta.id_idioma == id_idioma && ta.id_aplicacion == id_aplicacion)
                        .Select(s => new Usuarios_Objetos.Aplicacion {
                            id_aplicacion   = s.id_aplicacion,
                            codigo          = s.Tb_Aplicacion.codigo,
                            id_aplicacion_t = s.id_aplicacion_T,
                            aplicacion      = s.aplicacion,
                            descripcion     = s.descripcion,
                            id_idioma       = s.id_idioma,
                            activo          = s.activo
                        }).ToList();
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an application in the database.
        /// </summary>
        /// <param name="id_usuarioCreador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the code of the application.
        /// </param>
        /// <param name="aplicacion">
        /// A string that represents the name of the application.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the description of the application.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record created. 
        /// </returns>
        public Usuarios_Objetos.Respuesta crearAplicacion(int id_usuarioCreador, string codigo, string aplicacion, string descripcion = null) {
            Usuarios_Objetos.Respuesta res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Aplicacion_Crear(
                    id_usuarioCreador,
                    codigo,
                    aplicacion,
                    descripcion,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    res.mensaje = "OK";
                    res.objeto  = ue.Tb_Aplicacion_T.Where(ta => ta.id_aplicacion == id)
                        .Select(s => new Usuarios_Objetos.Aplicacion {
                            id_aplicacion = s.id_aplicacion,
                            codigo        = s.Tb_Aplicacion.codigo,
                            aplicacion    = s.aplicacion,
                            activo        = s.activo
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

        /// <summary>
        /// Creates an application in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the code of the application.
        /// </param>
        /// <param name="aplicacion">
        /// A string that represents the name of the application.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the description of the application.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record created. 
        /// </returns>
        public Usuarios_Objetos.Respuesta crearAplicacion(string sesion, string codigo, string aplicacion, string descripcion = null) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);

                return this.crearAplicacion(id, codigo, aplicacion, descripcion);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an application in the database.
        /// </summary>
        /// <param name="id_usuarioModificador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_aplicacion">
        /// An integer identifier related to the application that should be updated.
        /// </param>
        /// <param name="id_idioma">
        /// An integer identifier related to a language.
        /// </param>
        /// <param name="id_aplicacion_t">
        /// An integer identifier related to an application and a language.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the new code of the application.
        /// </param>
        /// <param name="aplicacion">
        /// A string that represents the new name of the aplicacion.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the new description of the descripcion.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the application. If set to <c>true</c> the records will be active, otherwise it will inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarAplicacion(int id_usuarioModificador, int id_aplicacion, int id_idioma, int id_aplicacion_t, string codigo, string aplicacion, string descripcion, bool activo) {
            Usuarios_Objetos.Respuesta res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Aplicacion_Modificar(
                    id_usuarioModificador,
                    id_idioma,
                    id_aplicacion,
                    id_aplicacion_t,
                    codigo,
                    aplicacion,
                    descripcion, activo, vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    res.mensaje = "OK";
                    res.objeto  = ue.Tb_Aplicacion_T.Where(ta => ta.id_aplicacion == id)
                        .Select(s => new Usuarios_Objetos.Aplicacion {
                            id_aplicacion   = s.id_aplicacion,
                            codigo          = s.Tb_Aplicacion.codigo,
                            id_aplicacion_t = s.id_aplicacion_T,
                            aplicacion      = s.aplicacion,
                            descripcion     = s.descripcion,
                            id_idioma       = s.id_idioma,
                            activo          = s.activo
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

        /// <summary>
        /// Updates an application in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_aplicacion">
        /// An integer identifier related to the application that should be updated.
        /// </param>
        /// <param name="id_idioma">
        /// An integer identifier related to a language.
        /// </param>
        /// <param name="id_aplicacion_t">
        /// An integer identifier related to an application and a language.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the new code of the application.
        /// </param>
        /// <param name="aplicacion">
        /// A string that represents the new name of the aplicacion.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the new description of the descripcion.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the application. If set to <c>true</c> the records will be active, otherwise it will inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarAplicacion(string sesion, int id_aplicacion, int id_idioma, int id_aplicacion_t, string codigo, string aplicacion, string descripcion, bool activo) {
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ServicioUtilidades.buscarIdUsuario(sesion, ue); 

                return this.modificarAplicacion(id, id_aplicacion, id_idioma, id_aplicacion_t, codigo, aplicacion, descripcion, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        #endregion

        #region Secure

        /// <summary>
        /// Gets information about all applications registered in the database.
        /// </summary>
        /// <param name="id_aplicacion">
        /// An integer identifier related to an application. If set to a value less than or equal to <c>0</c> the method searchs all applications.
        /// </param>
        /// <param name="id_idioma">
        /// An integer identifier related to a language.
        /// </param>
        /// <returns>
        /// A list that contains all infotmation of the application/applications registered in the database, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_aplicacion), codigo, Enc(id_aplicacion_t), aplicacion, descripcion, Enc(id_idioma), Enc(id_usuarioCreador), activo]
        /// </returns>
        public List<Usuarios_Objetos.Sec_Aplicacion> buscarSec_Aplicacion(int id_aplicacion, int id_idioma) {
            try {
                if (id_aplicacion <= 0)
                    return ue.Tb_Aplicacion_T.Where(ta => ta.id_idioma == id_idioma)
                        .Select(s => new {
                            id_aplicacion   = s.id_aplicacion,
                            codigo          = s.Tb_Aplicacion.codigo,
                            id_aplicacion_t = s.id_aplicacion_T,
                            aplicacion      = s.aplicacion,
                            descripcion     = s.descripcion,
                            id_idioma       = s.id_idioma,
                            activo          = s.activo
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Aplicacion(
                            e.id_aplicacion,
                            e.codigo,
                            e.id_aplicacion_t,
                            e.aplicacion,
                            e.descripcion,
                            e.id_idioma,
                            0,
                            e.activo
                        )).ToList();
                else
                    return ue.Tb_Aplicacion_T.Where(ta => ta.id_idioma == id_idioma && ta.id_aplicacion == id_aplicacion)
                        .Select(s => new {
                            id_aplicacion   = s.id_aplicacion,
                            codigo          = s.Tb_Aplicacion.codigo,
                            id_aplicacion_t = s.id_aplicacion_T,
                            aplicacion      = s.aplicacion,
                            descripcion     = s.descripcion,
                            id_idioma       = s.id_idioma,
                            activo          = s.activo
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Aplicacion(
                            e.id_aplicacion,
                            e.codigo,
                            e.id_aplicacion_t,
                            e.aplicacion,
                            e.descripcion,
                            e.id_idioma,
                            0,
                            e.activo
                        )).ToList();
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }
              
        /// <summary>
        /// Creates an application in the database.
        /// </summary>
        /// <param name="id_usuarioCreador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the code of the application.
        /// </param>
        /// <param name="aplicacion">
        /// A string that represents the name of the application.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the description of the application.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record created, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_aplicacion), codigo, Enc(id_aplicacion_t), aplicacion, descripcion, Enc(id_idioma), Enc(id_usuarioCreador), activo]
        /// </returns>
        public Usuarios_Objetos.Respuesta crearSec_Aplicacion(int id_usuarioCreador, string codigo, string aplicacion, string descripcion = null) {
            Usuarios_Objetos.Respuesta res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Aplicacion_Crear(
                    id_usuarioCreador,
                    codigo,
                    aplicacion,
                    descripcion,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    res.mensaje = "OK";
                    res.objeto  = ue.Tb_Aplicacion_T.Where(ta => ta.id_aplicacion == id)
                        .Select(s => new {
                            id_aplicacion = s.id_aplicacion,
                            codigo        = s.Tb_Aplicacion.codigo,
                            aplicacion    = s.aplicacion,
                            descripcion   = s.descripcion,
                            activo        = s.activo
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Aplicacion(
                            e.id_aplicacion,
                            e.codigo,
                            0,
                            e.aplicacion,
                            e.descripcion,
                            0,
                            0,
                            e.activo
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

        /// <summary>
        /// Creates an application in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the code of the application.
        /// </param>
        /// <param name="aplicacion">
        /// A string that represents the name of the application.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the description of the application.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record created, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_aplicacion), codigo, Enc(id_aplicacion_t), aplicacion, descripcion, Enc(id_idioma), Enc(id_usuarioCreador), activo]
        /// </returns>
        public Usuarios_Objetos.Respuesta crearSec_Aplicacion(string sesion, string codigo, string aplicacion, string descripcion = null) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);

                return this.crearSec_Aplicacion(id, codigo, aplicacion, descripcion);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an application in the database.
        /// </summary>
        /// <param name="id_usuarioModificador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_aplicacion">
        /// An integer identifier related to the application that should be updated.
        /// </param>
        /// <param name="id_idioma">
        /// An integer identifier related to a language.
        /// </param>
        /// <param name="id_aplicacion_t">
        /// An integer identifier related to an application and a language.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the new code of the application.
        /// </param>
        /// <param name="aplicacion">
        /// A string that represents the new name of the aplicacion.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the new description of the descripcion.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the application. If set to <c>true</c> the records will be active, otherwise it will inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_aplicacion), codigo, Enc(id_aplicacion_t), aplicacion, descripcion, Enc(id_idioma), Enc(id_usuarioCreador), activo]
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarSec_Aplicacion(int id_usuarioModificador, int id_aplicacion, int id_idioma, int id_aplicacion_t, string codigo, string aplicacion, string descripcion, bool activo) {
            Usuarios_Objetos.Respuesta res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Aplicacion_Modificar(
                    id_usuarioModificador,
                    id_idioma,
                    id_aplicacion,
                    id_aplicacion_t,
                    codigo,
                    aplicacion,
                    descripcion,
                    activo,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    res.mensaje = "OK";
                    res.objeto  = ue.Tb_Aplicacion_T.Where(ta => ta.id_aplicacion == id)
                        .Select(s => new {
                            id_aplicacion   = s.id_aplicacion,
                            codigo          = s.Tb_Aplicacion.codigo,
                            id_aplicacion_t = s.id_aplicacion_T,
                            aplicacion      = s.aplicacion,
                            descripcion     = s.descripcion,
                            id_idioma       = s.id_idioma,
                            activo          = s.activo
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Aplicacion(
                            e.id_aplicacion,
                            e.codigo,
                            e.id_aplicacion_t,
                            e.aplicacion,
                            e.descripcion,
                            e.id_idioma,
                            0,
                            e.activo
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

        /// <summary>
        /// Updates an application in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_aplicacion">
        /// An integer identifier related to the application that should be updated.
        /// </param>
        /// <param name="id_idioma">
        /// An integer identifier related to a language.
        /// </param>
        /// <param name="id_aplicacion_t">
        /// An integer identifier related to an application and a language.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the new code of the application.
        /// </param>
        /// <param name="aplicacion">
        /// A string that represents the new name of the aplicacion.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the new description of the descripcion.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the application. If set to <c>true</c> the records will be active, otherwise it will inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_aplicacion), codigo, Enc(id_aplicacion_t), aplicacion, descripcion, Enc(id_idioma), Enc(id_usuarioCreador), activo]
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarSec_Aplicacion(string sesion, int id_aplicacion, int id_idioma, int id_aplicacion_t, string codigo, string aplicacion, string descripcion, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);

                return this.modificarSec_Aplicacion(id, id_aplicacion, id_idioma, id_aplicacion_t, codigo, aplicacion, descripcion, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        #endregion
    }
}