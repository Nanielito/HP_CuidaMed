using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios_DB {

    /// <summary>
    /// Class to manage methods related with a group entity.
    /// </summary>
    public class ServicioGrupo {

        /// <summary>
        /// The ue field represents an entity model for the UsuarioEntidades database.
        /// </summary>
        UsuariosEntidades ue = null;

        /// <summary>
        /// The vResultado field represents an object parameter which is used when a stored procedures is invoked.
        /// </summary>
        ObjectParameter vResultado = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServicioGrupo"/> class and creates a database instance (UsuariosEntidades).
        /// </summary>
        public ServicioGrupo() {
            ue = new UsuariosEntidades(
                Utilidades.AES.desencriptar(
                    System.Configuration.ConfigurationManager.ConnectionStrings["UsuariosEntidades"].ConnectionString
                )
            );
        }

        #region Normal

        /// <summary>
        /// Gets information about a group or all groups registered in the database.
        /// </summary>
        /// <param name="id_grupo">
        /// An integer identifier related to a group. If set to a value less than or equal to <c>0</c> the method searchs all groups.
        /// </param>
        /// <returns>
        /// A list that contains all information of the group/groups registered in the database.
        /// </returns>
        public List<Usuarios_Objetos.Grupo> buscarGrupo(int id_grupo) {
            try {
                if (id_grupo <= 0)
                    return ue.Tb_Grupo
                        .Select(s => new Usuarios_Objetos.Grupo {
                            id_grupo      = s.id_grupo,
                            codigo        = s.codigo,
                            grupo         = s.grupo,
                            descripcion   = s.descripcion,
                            id_aplicacion = s.id_aplicacion,
                            activo        = s.activo
                        }).ToList();
                else
                    return ue.Tb_Grupo.Where(ta => ta.id_grupo == id_grupo)
                        .Select(s => new Usuarios_Objetos.Grupo {
                            id_grupo      = s.id_grupo,
                            codigo        = s.codigo,
                            grupo         = s.grupo,
                            descripcion   = s.descripcion,
                            id_aplicacion = s.id_aplicacion,
                            activo        = s.activo
                        }).ToList();
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates a group in the database.
        /// </summary>
        /// <param name="id_usuarioCreador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_aplicacion">
        /// An integer identifier related to the application that should be the group owner.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the code of the grupo.
        /// </param>
        /// <param name="grupo">
        /// A string that represents the name of the grupo.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the description of the group.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record created.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearGrupo(int id_usuarioCreador, int id_aplicacion, string codigo, string grupo, string descripcion = null) {
            Usuarios_Objetos.Respuesta res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Grupo_Crear(
                    id_usuarioCreador,
                    id_aplicacion,
                    codigo,
                    grupo,
                    descripcion,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    res.mensaje = "OK";
                    res.objeto  = ue.Tb_Grupo.Where(tg => tg.id_grupo == id)
                        .Select(s => new Usuarios_Objetos.Grupo {
                            id_grupo      = s.id_grupo,
                            codigo        = s.codigo,
                            grupo         = s.grupo,
                            id_aplicacion = s.id_aplicacion,
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
        /// Creates a group in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_aplicacion">
        /// An integer identifier related to the application that should be the group owner.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the code of the grupo.
        /// </param>
        /// <param name="grupo">
        /// A string that represents the name of the grupo.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the description of the group.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record created.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearGrupo(string sesion, int id_aplicacion, string codigo, string grupo, string descripcion = null) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);

                return this.crearGrupo(id, id_aplicacion, codigo, grupo, descripcion);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates a group in the database.
        /// </summary>
        /// <param name="id_usuarioModificador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_grupo">
        /// An integer identifier related to the group that should be updated.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the new code of the group.
        /// </param>
        /// <param name="grupo">
        /// A string that represents the new name of the group.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the new description of the group.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the group. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarGrupo(int id_usuarioModificador, int id_grupo, string codigo, string grupo, string descripcion, bool activo) {
            Usuarios_Objetos.Respuesta res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Grupo_Modificar(
                    id_usuarioModificador,
                    id_grupo,
                    codigo,
                    grupo,
                    descripcion,
                    activo,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    res.mensaje = "OK";
                    res.objeto = ue.Tb_Grupo.Where(tg => tg.id_grupo == id)
                        .Select(s => new Usuarios_Objetos.Grupo {
                            id_grupo      = s.id_grupo,
                            codigo        = s.codigo,
                            grupo         = s.grupo,
                            descripcion   = s.descripcion,
                            id_aplicacion = s.id_aplicacion,
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
        /// Updates a group in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_grupo">
        /// An integer identifier related to the group that should be updated.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the new code of the group.
        /// </param>
        /// <param name="grupo">
        /// A string that represents the new name of the group.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the new description of the group.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the group. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarGrupo(string sesion, int id_grupo, string codigo, string grupo, string descripcion, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);

                return this.modificarGrupo(id, id_grupo, codigo, grupo, descripcion, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        #endregion

        #region Secure

        /// <summary>
        /// Gets information about a group or all groups registered in the database.
        /// </summary>
        /// <param name="id_grupo">
        /// An integer identifier related to a group. If set to a value less than or equal to <c>0</c> the method searchs all groups.
        /// </param>
        /// <returns>
        /// A list that contains all information of the group/groups registered in the database, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_grupo), codigo, grupo, descripcion, Enc(id_aplicacion), activo]
        /// </returns>
        public List<Usuarios_Objetos.Sec_Grupo> buscarSec_Grupo(int id_grupo) {
            try {
                if (id_grupo <= 0)
                    return ue.Tb_Grupo
                        .Select(s => new {
                            id_grupo      = s.id_grupo,
                            codigo        = s.codigo,
                            grupo         = s.grupo,
                            descripcion   = s.descripcion,
                            id_aplicacion = s.id_aplicacion,
                            activo        = s.activo
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Grupo(
                            e.id_grupo,
                            e.codigo,
                            e.grupo,
                            e.descripcion,
                            e.id_aplicacion,
                            e.activo
                        )).ToList();
                else
                    return ue.Tb_Grupo.Where(ta => ta.id_grupo == id_grupo)
                        .Select(s => new {
                            id_grupo      = s.id_grupo,
                            codigo        = s.codigo,
                            grupo         = s.grupo,
                            descripcion   = s.descripcion,
                            id_aplicacion = s.id_aplicacion,
                            activo        = s.activo
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Grupo(
                            e.id_grupo,
                            e.codigo,
                            e.grupo,
                            e.descripcion,
                            e.id_aplicacion,
                            e.activo
                        )).ToList();
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates a group in the database.
        /// </summary>
        /// <param name="id_usuarioCreador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_aplicacion">
        /// An integer identifier related to the application that should be the group owner.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the code of the grupo.
        /// </param>
        /// <param name="grupo">
        /// A string that represents the name of the grupo.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the description of the group.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record created, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_grupo), codigo, grupo, descripcion, Enc(id_aplicacion), activo]
        /// </returns>
        public Usuarios_Objetos.Respuesta crearSec_Grupo(int id_usuarioCreador, int id_aplicacion, string codigo, string grupo, string descripcion = null) {
            Usuarios_Objetos.Respuesta res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Grupo_Crear(
                    id_usuarioCreador,
                    id_aplicacion,
                    codigo,
                    grupo,
                    descripcion,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    res.mensaje = "OK";
                    res.objeto  = ue.Tb_Grupo.Where(tg => tg.id_grupo == id)
                        .Select(s => new {
                            id_grupo      = s.id_grupo,
                            codigo        = s.codigo,
                            grupo         = s.grupo,
                            descripcion   = s.descripcion,
                            id_aplicacion = s.id_aplicacion,
                            activo        = s.activo
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Grupo(
                            e.id_grupo,
                            e.codigo,
                            e.grupo,
                            e.descripcion,
                            e.id_aplicacion,
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
        /// Creates a group in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_aplicacion">
        /// An integer identifier related to the application that should be the group owner.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the code of the grupo.
        /// </param>
        /// <param name="grupo">
        /// A string that represents the name of the grupo.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the description of the group.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record created, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_grupo), codigo, grupo, descripcion, Enc(id_aplicacion), activo]
        /// </returns>
        public Usuarios_Objetos.Respuesta crearSec_Grupo(string sesion, int id_aplicacion, string codigo, string grupo, string descripcion = null) {
           int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);
                
                return this.crearSec_Grupo(id, id_aplicacion, codigo, grupo, descripcion);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates a group in the database.
        /// </summary>
        /// <param name="id_usuarioModificador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_grupo">
        /// An integer identifier related to the group that should be updated.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the new code of the group.
        /// </param>
        /// <param name="grupo">
        /// A string that represents the new name of the group.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the new description of the group.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the group. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_grupo), codigo, grupo, descripcion, Enc(id_aplicacion), activo]
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarSec_Grupo(int id_usuarioModificador, int id_grupo, string codigo, string grupo, string descripcion, bool activo) {
            Usuarios_Objetos.Respuesta res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Grupo_Modificar(
                    id_usuarioModificador,
                    id_grupo,
                    codigo,
                    grupo,
                    descripcion,
                    activo,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    res.mensaje = "OK";
                    res.objeto = ue.Tb_Grupo.Where(tg => tg.id_grupo == id)
                        .Select(s => new {
                            id_grupo      = s.id_grupo,
                            codigo        = s.codigo,
                            grupo         = s.grupo,
                            descripcion   = s.descripcion,
                            id_aplicacion = s.id_aplicacion,
                            activo        = s.activo
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Grupo(
                            e.id_grupo,
                            e.codigo,
                            e.grupo,
                            e.descripcion,
                            e.id_aplicacion,
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
        /// Updates a group in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_grupo">
        /// An integer identifier related to the group that should be updated.
        /// </param>
        /// <param name="codigo">
        /// A string that represents the new code of the group.
        /// </param>
        /// <param name="grupo">
        /// A string that represents the new name of the group.
        /// </param>
        /// <param name="descripcion">
        /// A string that represents the new description of the group.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the group. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_grupo), codigo, grupo, descripcion, Enc(id_aplicacion), activo]
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarSec_Grupo(string sesion, int id_grupo, string codigo, string grupo, string descripcion, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);
                
                return this.modificarSec_Grupo(id, id_grupo, codigo, grupo, descripcion, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        #endregion
    }
}