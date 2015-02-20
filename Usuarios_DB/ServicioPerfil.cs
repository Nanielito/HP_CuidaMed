using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios_DB {

    /// <summary>
    /// Class to manage the methods related to a profile entity.
    /// </summary>
    public class ServicioPerfil {

        /// <summary>
        /// The ue field represents an entity model for the UsuarioEntidades database.
        /// </summary>
        UsuariosEntidades ue = null;

        /// <summary>
        /// The vResultado field represents an object parameter which is used when a stored procedures is invoked.
        /// </summary>
        ObjectParameter vResultado = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServicioPerfil"/> class and creates a database instance (UsuariosEntidades).
        /// </summary>
        public ServicioPerfil() {
            ue = new UsuariosEntidades(
                Utilidades.AES.desencriptar(
                    System.Configuration.ConfigurationManager.ConnectionStrings["UsuariosEntidades"].ConnectionString
                )
            );
        }

        #region Normal

        /// <summary>
        /// Gets information about an user profile or all users profiles registered in the database.
        /// </summary>
        /// <param name="id_perfil">
        /// An integer identifier related to a profile. If set to a value less than or equal to <c>0</c> the method searchs all profiles.
        /// </param>
        /// <param name="id_idioma">
        /// An integer identifier related to a language.
        /// </param>
        /// <returns>
        /// A list that contains all information of the profile/profiles registered in the database.
        /// </returns>
        public List<Usuarios_Objetos.Perfil> buscarPerfil(int id_perfil, int id_idioma) {
            try {
                if (id_perfil <= 0)
                    return ue.Tb_Perfil_T.Where(ta => ta.id_idioma == id_idioma)
                        .Select(s => new Usuarios_Objetos.Perfil {
                            id_perfil   = s.id_perfil,
                            codigo      = s.Tb_Perfil.codigo,
                            id_perfil_t = s.id_perfil_T,
                            perfil      = s.perfil,
                            descripcion = s.descripcion,
                            id_idioma   = s.id_idioma,
                            activo      = s.activo
                        }).ToList();
                else
                    return ue.Tb_Perfil_T.Where(ta => ta.id_idioma == id_idioma && ta.id_perfil == id_perfil)
                        .Select(s => new Usuarios_Objetos.Perfil {
                            id_perfil   = s.id_perfil,
                            codigo      = s.Tb_Perfil.codigo,
                            id_perfil_t = s.id_perfil_T,
                            perfil      = s.perfil,
                            descripcion = s.descripcion,
                            id_idioma   = s.id_idioma,
                            activo      = s.activo
                        }).ToList();
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates a profile in the database.
        /// </summary>
        /// <param name="id_usuarioCreador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the code of the profile.
        /// </param>
        /// <param name="perfil">
        /// A string that represents the name of the profile.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the description of the profile.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information related to the request status and and the created record.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearPerfil(int id_usuarioCreador, string codigo, string perfil, string descripcion = null) {
            Usuarios_Objetos.Respuesta res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Perfil_Crear(
                    id_usuarioCreador,
                    codigo,
                    perfil,
                    descripcion,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    res.mensaje = "OK";
                    res.objeto = ue.Tb_Perfil_T.Where(tp => tp.id_perfil == id)
                        .Select(s => new Usuarios_Objetos.Perfil {
                            id_perfil = s.id_perfil,
                            codigo = s.Tb_Perfil.codigo,
                            perfil = s.perfil,
                            activo = s.activo
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
        /// Creates a profile in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the code of the profile.
        /// </param>
        /// <param name="perfil">
        /// A string that represents the name of the profile.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the description of the profile.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information related to the request status and and the created record.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearPerfil(string sesion, string codigo, string perfil, string descripcion = null) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);
                
                return this.crearPerfil(id, codigo, perfil, descripcion);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates a profile in the database.
        /// </summary>
        /// <param name="id_usuarioModificador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_perfil">
        /// An integer identifier related to the profile that should be updated.
        /// </param>
        /// <param name="id_idioma">
        /// An integer identifier related to a language.
        /// </param>
        /// <param name="id_perfil_t">
        /// An integer identifier related to a profile and a language.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the new code of the profile.
        /// </param>
        /// <param name="perfil">
        /// A string that represents the new name of the profile.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the new description of the profile.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the profile. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarPerfil(int id_usuarioModificador, int id_perfil, int id_idioma, int id_perfil_t, string codigo, string perfil, string descripcion, bool activo) {
            Usuarios_Objetos.Respuesta res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Perfil_Modificar(
                    id_usuarioModificador,
                    id_idioma,
                    id_perfil,
                    id_perfil_t,
                    codigo,
                    perfil,
                    descripcion,
                    activo,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    res.mensaje = "OK";
                    res.objeto  = ue.Tb_Perfil_T.Where(tp => tp.id_perfil == id)
                        .Select(s => new Usuarios_Objetos.Perfil {
                            id_perfil   = s.id_perfil,
                            codigo      = s.Tb_Perfil.codigo,
                            id_perfil_t = s.id_perfil_T,
                            perfil      = s.perfil,
                            descripcion = s.descripcion,
                            id_idioma   = s.id_idioma,
                            activo      = s.activo
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
        /// Updates a profile in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_perfil">
        /// An integer identifier related to the profile that should be updated.
        /// </param>
        /// <param name="id_idioma">
        /// An integer identifier related to a language.
        /// </param>
        /// <param name="id_perfil_t">
        /// An integer identifier related to a profile and a language.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the new code of the profile.
        /// </param>
        /// <param name="perfil">
        /// A string that represents the new name of the profile.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the new description of the profile.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the profile. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarPerfil(string sesion, int id_perfil, int id_idioma, int id_perfil_t, string codigo, string perfil, string descripcion, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);

                return this.modificarPerfil(id, id_perfil, id_idioma, id_perfil_t, codigo, perfil, descripcion, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        #endregion

        #region Secure

        /// <summary>
        /// Gets information about an user profile or all users profiles registered in the database.
        /// </summary>
        /// <param name="id_perfil">
        /// An integer identifier related to a profile. If set to a value less than or equal to <c>0</c> the method searchs all profiles.
        /// </param>
        /// <param name="id_idioma">
        /// An integer identifier related to a language.
        /// </param>
        /// <returns>
        /// A list that contains all information of the profile/profiles registered in the database, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_perfil), codigo, Enc(id_perfil_t), perfil, descripcion, Enc(id_idioma), activo]
        /// </returns>
        public List<Usuarios_Objetos.Sec_Perfil> buscarSec_Perfil(int id_perfil, int id_idioma) {
            try {
                if (id_perfil <= 0)
                    return ue.Tb_Perfil_T.Where(ta => ta.id_idioma == id_idioma)
                        .Select(s => new {
                            id_perfil   = s.id_perfil,
                            codigo      = s.Tb_Perfil.codigo,
                            id_perfil_t = s.id_perfil_T,
                            perfil      = s.perfil,
                            descripcion = s.descripcion,
                            id_idioma   = s.id_idioma,
                            activo      = s.activo
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Perfil(
                            e.id_perfil,
                            e.codigo,
                            e.id_perfil_t,
                            e.perfil,
                            e.descripcion,
                            e.id_idioma,
                            e.activo
                        )).ToList();
                else
                    return ue.Tb_Perfil_T.Where(ta => ta.id_idioma == id_idioma && ta.id_perfil == id_perfil)
                        .Select(s => new {
                            id_perfil   = s.id_perfil,
                            codigo      = s.Tb_Perfil.codigo,
                            id_perfil_t = s.id_perfil_T,
                            perfil      = s.perfil,
                            descripcion = s.descripcion,
                            id_idioma   = s.id_idioma,
                            activo      = s.activo
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Perfil(
                            e.id_perfil,
                            e.codigo,
                            e.id_perfil_t,
                            e.perfil,
                            e.descripcion,
                            e.id_idioma,
                            e.activo
                        )).ToList();
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates a profile in the database.
        /// </summary>
        /// <param name="id_usuarioCreador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the code of the profile.
        /// </param>
        /// <param name="perfil">
        /// A string that represents the name of the profile.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the description of the profile.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and and the record created, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_perfil), codigo, Enc(id_perfil_t), perfil, descripcion, Enc(id_idioma), activo]
        /// </returns>
        public Usuarios_Objetos.Respuesta crearSec_Perfil(int id_usuarioCreador, string codigo, string perfil, string descripcion = null) {
            Usuarios_Objetos.Respuesta res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Perfil_Crear(
                    id_usuarioCreador,
                    codigo,
                    perfil,
                    descripcion,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    res.mensaje = "OK";
                    res.objeto  = ue.Tb_Perfil_T.Where(tp => tp.id_perfil == id)
                        .Select(s => new {
                            id_perfil   = s.id_perfil,
                            codigo      = s.Tb_Perfil.codigo,
                            perfil      = s.perfil,
                            descripcion = s.descripcion,
                            activo      = s.activo
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Perfil(
                            e.id_perfil,
                            e.codigo,
                            0,
                            e.perfil,
                            e.descripcion,
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
        /// Creates a profile in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the code of the profile.
        /// </param>
        /// <param name="perfil">
        /// A string that represents the name of the profile.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the description of the profile.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and and the record created, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_perfil), codigo, Enc(id_perfil_t), perfil, descripcion, Enc(id_idioma), activo]
        /// </returns>
        public Usuarios_Objetos.Respuesta crearSec_Perfil(string sesion, string codigo, string perfil, string descripcion = null) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);
                
                return this.crearSec_Perfil(id, codigo, perfil, descripcion);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates a profile in the database.
        /// </summary>
        /// <param name="id_usuarioModificador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_perfil">
        /// An integer identifier related to the profile that should be updated.
        /// </param>
        /// <param name="id_idioma">
        /// An integer identifier related to a language.
        /// </param>
        /// <param name="id_perfil_t">
        /// An integer identifier related to a profile and a language.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the new code of the profile.
        /// </param>
        /// <param name="perfil">
        /// A string that represents the new name of the profile.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the new description of the profile.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the profile. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_perfil), codigo, Enc(id_perfil_t), perfil, descripcion, Enc(id_idioma), activo]
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarSec_Perfil(int id_usuarioModificador, int id_perfil, int id_idioma, int id_perfil_t, string codigo, string perfil, string descripcion, bool activo) {
            Usuarios_Objetos.Respuesta res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Perfil_Modificar(
                    id_usuarioModificador,
                    id_idioma,
                    id_perfil,
                    id_perfil_t,
                    codigo,
                    perfil,
                    descripcion,
                    activo,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    res.mensaje = "OK";
                    res.objeto  = ue.Tb_Perfil_T.Where(tp => tp.id_perfil == id)
                        .Select(s => new {
                            id_perfil   = s.id_perfil,
                            codigo      = s.Tb_Perfil.codigo,
                            id_perfil_t = s.id_perfil_T,
                            perfil      = s.perfil,
                            descripcion = s.descripcion,
                            id_idioma   = s.id_idioma,
                            activo      = s.activo
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Perfil(
                            e.id_perfil,
                            e.codigo,
                            e.id_perfil_t,
                            e.perfil,
                            e.descripcion,
                            e.id_idioma,
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
        /// Updates a profile in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_perfil">
        /// An integer identifier related to the profile that should be updated.
        /// </param>
        /// <param name="id_idioma">
        /// An integer identifier related to a language.
        /// </param>
        /// <param name="id_perfil_t">
        /// An integer identifier related to a profile and a language.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the new code of the profile.
        /// </param>
        /// <param name="perfil">
        /// A string that represents the new name of the profile.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the new description of the profile.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the profile. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_perfil), codigo, Enc(id_perfil_t), perfil, descripcion, Enc(id_idioma), activo]
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarSec_Perfil(string sesion, int id_perfil, int id_idioma, int id_perfil_t, string codigo, string perfil, string descripcion, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);
                
                return this.modificarSec_Perfil(id, id_perfil, id_idioma, id_perfil_t, codigo, perfil, descripcion, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        #endregion
    }
}