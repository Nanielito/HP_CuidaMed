using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Usuarios_DB {

    /// <summary>
    /// Class to manage methods related to an user entity.
    /// </summary>
    public class ServicioUsuario {

        /// <summary>
        /// The ue field represents an entity model for the UsuarioEntidades database.
        /// </summary>
        UsuariosEntidades ue = null;

        /// <summary>
        /// The vResultado field represents an object parameter which is used when a stored procedures is invoked.
        /// </summary>
        ObjectParameter vResultado = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServicioUsuario"/> class and and creates a database instance (UsuariosEntidades).
        /// </summary>
        public ServicioUsuario() {
            ue = new UsuariosEntidades(
                Utilidades.AES.desencriptar(
                    System.Configuration.ConfigurationManager.ConnectionStrings["UsuariosEntidades"].ConnectionString
                )
            );
        }

        /// <summary>
        /// Creates an administrator user in the database.
        /// </summary>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record created.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearUsuarioAdministrador() {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            Tb_Usuario tb_usuario = null;
            string usuario = "", contrasena = "";
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                usuario    = Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512("admin"), "qwerty256");
                contrasena = Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512("qwerty256"), "qwerty256");

                tb_usuario = ue.Tb_Usuario.Where(tu => tu.usuario == usuario).FirstOrDefault();

                if (tb_usuario == null) {

                    id = ue.Sp_UsuarioAdmin_Crear(
                        usuario,
                        Utilidades.Hashing.SHA_512("admin"),
                        contrasena,
                        vResultado
                    ).FirstOrDefault() ?? -1;

                    if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                        _res.mensaje = "OK";
                        _res.objeto  = ue.Tb_Usuario.Where(tu => tu.id_usuario == id)
                            .Select(s => new Usuarios_Objetos.Usuario {
                                id_usuario = s.id_usuario
                            }).FirstOrDefault();
                    }
                    else
                        _res.mensaje = vResultado.Value.ToString();
                }
                else
                    _res.mensaje = "ERROR";

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Checks if an username exists in the database.
        /// </summary>
        /// <param name="usuario">
        /// A string that represents an username.
        /// </param>
        /// <returns>
        /// A boolean. It will be <c>true</c> if the username exists, otherwise it will be <c>false</c>.
        /// </returns>
        public bool existeUsuario(string usuario) {
            string usuario_enc = "";

            try {
                usuario_enc = Utilidades.Hashing.SHA_512(usuario);

                return (ue.Tb_Usuario.Where(tu => tu.usuario_ == usuario_enc).FirstOrDefault() ?? null) != null ? true : false;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Checks if an email exists in the database.
        /// </summary>
        /// <param name="correo">
        /// A string that represents an email.
        /// </param>
        /// <returns>
        /// A boolean. It will be <c>true</c> if the email exists, otherwise it will be <c>false</c>.
        /// </returns>
        public bool existeCorreo(string correo) {
            string correo_enc = "";

            try {
                correo_enc = Utilidades.Hashing.SHA_512(correo);

                return (ue.Tb_Usuario.Where(tu => tu.usuarioCorreo_ == correo_enc).FirstOrDefault() ?? null) != null ? true : false;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        #region Normal

        /// <summary>
        /// Gets information about an user or all users registered in the database.
        /// </summary>
        /// <param name="id_usuario">
        /// An integer identifier related to an user. If set to a value less than or equal to <c>0</c> the method searchs all users.
        /// </param>
        /// <returns>
        /// A list that contains all information of the user/users registered in the database.
        /// </returns>
        public List<Usuarios_Objetos.Usuario> buscarUsuario(int id_usuario) {
            try {
                if (id_usuario <= 0)
                    return ue.Tb_UsuarioActivo
                        .Select(s => new {
                            id_usuario = s.id_usuario,
                            activo = s.activo,
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Usuario {
                            id_usuario = e.id_usuario,
                            id_perfil = ue.Tb_Usuario_Perfil.Where(tup => tup.id_usuario == e.id_usuario).Select(s => s.id_usuario).FirstOrDefault(),
                            activo = e.activo
                        }).ToList();
                else
                    return ue.Tb_UsuarioActivo.Where(tua => tua.id_usuario == id_usuario)
                        .Select(s => new {
                            id_usuario = s.id_usuario,
                            activo = s.activo,
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Usuario {
                            id_usuario = e.id_usuario,
                            id_perfil = ue.Tb_Usuario_Perfil.Where(tup => tup.id_usuario == e.id_usuario).Select(s => s.id_usuario).FirstOrDefault(),
                            activo = e.activo
                        }).ToList();
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Gets information about an user registered in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <returns>
        /// A list that contains all information of the user registered in the database.
        /// </returns>
        public List<Usuarios_Objetos.Usuario> buscarUsuario(string sesion) {
            int id = -1;
            
            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);

                return this.buscarUsuario(id);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an user in the database.
        /// </summary>
        /// <param name="id_usuarioCreador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="usuario">
        /// A string that represents the user name.
        /// </param>
        /// <param name="usuarioCorreo">
        /// A string that represents the user email.
        /// </param>
        /// <param name="contrasena">
        /// A string that represents the user password.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the user. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record created.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearUsuario(int id_usuarioCreador, string usuario, string usuarioCorreo, string contrasena, bool activo = true) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Crear(
                    id_usuarioCreador,
                    Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(usuario), contrasena),
                    Utilidades.Hashing.SHA_512(usuario),
                    !string.IsNullOrEmpty(usuarioCorreo) ? Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(usuarioCorreo), contrasena) : null,
                    !string.IsNullOrEmpty(usuarioCorreo) ? Utilidades.Hashing.SHA_512(usuarioCorreo) : null,
                    Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(contrasena), contrasena),
                    activo,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    _res.mensaje = "OK";
                    _res.objeto = new Usuarios_Objetos.Usuario { id_usuario = id };
                }
                else
                    _res.mensaje = vResultado.Value.ToString();

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an user in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="usuario">
        /// A string that represents the user name.
        /// </param>
        /// <param name="usuarioCorreo">
        /// A string that represents the user email.
        /// </param>
        /// <param name="contrasena">
        /// A string that represents the user password.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the user. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record created.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearUsuario(string sesion, string usuario, string usuarioCorreo, string contrasena, bool activo = true) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);
                
                return this.crearUsuario(id, usuario, usuarioCorreo, contrasena, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates a password for an user.
        /// </summary>
        /// <param name="id_usuarioModificador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user whose password should be updated.
        /// </param>
        /// <param name="contrasena">
        /// A string that represents the current user password.
        /// </param>
        /// <param name="contrasena_nueva">
        /// A string that represents the new user password.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarContrasena(int id_usuarioModificador, int id_usuario, string contrasena, string contrasena_nueva) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            Usuarios_Objetos.Usuario _usuario = null;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                if (!string.IsNullOrEmpty(contrasena_nueva)) {
                    _usuario = ue.Tb_Usuario.Where(tu => tu.id_usuario == id_usuario && tu.contrasena == Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(contrasena), contrasena))
                        .Select(s => new Usuarios_Objetos.Usuario {
                            id_usuario = s.id_usuario,
                            usuario    = s.usuario,
                            usuarioCorreo = s.usuarioCorreo
                        }).FirstOrDefault();

                    if (_usuario == null)
                        _res.mensaje = "ERROR - Usuario no encontrado";
                    else {
                        ue.Sp_Usuario_Modificar(
                            id_usuarioModificador,
                            id_usuario,
                            Utilidades.AES.encriptar(Utilidades.AES.desencriptar(_usuario.usuario, contrasena), contrasena_nueva),
                            null,
                            !string.IsNullOrEmpty(_usuario.usuarioCorreo) ? Utilidades.AES.encriptar(Utilidades.AES.desencriptar(_usuario.usuarioCorreo), contrasena_nueva) : null,
                            null,
                            Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(contrasena_nueva), contrasena_nueva),
                            null,
                            vResultado
                        ).FirstOrDefault();

                        if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                            _usuario.usuario       = "";
                            _usuario.usuarioCorreo = "";
                            _res.mensaje = "OK";
                            _res.objeto  = _usuario;
                        }
                        else
                            _res.mensaje = vResultado.Value.ToString();
                    }
                }
                else
                    _res.mensaje = "ERROR - Contrasena invalida";

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates a password for an user.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user whose password should be updated.
        /// </param>
        /// <param name="contrasena">
        /// A string that represents the current user password.
        /// </param>
        /// <param name="contrasena_nueva">
        /// A string that represents the new user password.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarContrasena(string sesion, int id_usuario, string contrasena, string contrasena_nueva) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);
                
                return this.modificarContrasena(id, id_usuario, contrasena, contrasena_nueva);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an username for an user.
        /// </summary>
        /// <param name="id_usuarioModificador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user whose username should be updated.
        /// </param>
        /// <param name="usuario">
        /// A string that represents the new user name.
        /// </param>
        /// <param name="usuarioCorreo">
        /// A string that represents the new user email.
        /// </param>
        /// <param name="contrasena">
        /// A string that represent the current user password.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the user. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarUsuario(int id_usuarioModificador, int id_usuario, string usuario, string usuarioCorreo, string contrasena, bool activo) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            Usuarios_Objetos.Usuario _usuario = null;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                _usuario = ue.Tb_Usuario.Where(tu => tu.id_usuario == id_usuario && tu.contrasena == Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(contrasena), contrasena))
                    .Select(s => new Usuarios_Objetos.Usuario {
                        id_usuario = s.id_usuario
                    }).FirstOrDefault();

                if (_usuario == null)
                    _res.mensaje = "ERROR - Usuario no encontrado";
                else {
                    ue.Sp_Usuario_Modificar(
                        id_usuarioModificador,
                        id_usuario,
                        !string.IsNullOrEmpty(usuario) ? Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(usuario), contrasena) : null,
                        !string.IsNullOrEmpty(usuario) ? Utilidades.Hashing.SHA_512(usuario) : null,
                        !string.IsNullOrEmpty(usuarioCorreo) ? Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(usuarioCorreo), contrasena) : null,
                        !string.IsNullOrEmpty(usuarioCorreo) ? Utilidades.Hashing.SHA_512(usuarioCorreo) : null,
                        null,
                        activo,
                        vResultado
                    ).FirstOrDefault();

                    if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                        _res.mensaje = "OK";
                        _res.objeto  = _usuario;
                    }
                    else
                        _res.mensaje = vResultado.Value.ToString();
                }

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an username for an user.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user whose username should be updated.
        /// </param>
        /// <param name="usuario">
        /// A string that represents the new user name.
        /// </param>
        /// <param name="usuarioCorreo">
        /// A string that represents the new user email.
        /// </param>
        /// <param name="contrasena">
        /// A string that represent the current user password.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the user. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarUsuario(string sesion, int id_usuario, string usuario, string usuarioCorreo, string contrasena, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);
                
                return this.modificarUsuario(id, id_usuario, usuario, usuarioCorreo, contrasena, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an association between an user and an application in the database.
        /// </summary>
        /// <param name="id_usuarioCreador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user that will be associated.
        /// </param>
        /// <param name="id_aplicacion">
        /// An integer identifier related to the application that will be associated.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier created.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearUsuario_Aplicacion(int id_usuarioCreador, int id_usuario, int id_aplicacion, bool activo) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Aplicacion_Crear(
                    id_usuarioCreador,
                    id_usuario,
                    id_aplicacion,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    _res.mensaje = "OK";
                    _res.objeto = id;
                }
                else
                    _res.mensaje = vResultado.Value.ToString();

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an association between an user and an application in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user that will be associated.
        /// </param>
        /// <param name="id_aplicacion">
        /// An integer identifier related to the application that will be associated.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier created.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearUsuario_Aplicacion(string sesion, int id_usuario, int id_aplicacion, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);

                return this.crearUsuario_Aplicacion(id, id_usuario, id_aplicacion, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an association between an user and an application in the database.
        /// </summary>
        /// <param name="id_usuarioModificador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_usuarioAplicacion">
        /// An integer identifier related to the association that should be updated.
        /// </param>
        /// <param name="id_usuario">
        /// An intenger identifier related to an user.
        /// </param>
        /// <param name="id_aplicacion">
        /// An integer identifier related to an application.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier updated.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarUsuario_Aplicacion(int id_usuarioModificador, int id_usuarioAplicacion, int id_usuario, int id_aplicacion, bool activo) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Aplicacion_Modificar(
                    id_usuarioModificador,
                    id_usuarioAplicacion,
                    id_usuario,
                    id_aplicacion,
                    activo,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    _res.mensaje = "OK";
                    _res.objeto  = id;
                }
                else
                    _res.mensaje = vResultado.Value.ToString();

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an association between an user and an application in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_usuarioAplicacion">
        /// An integer identifier related to the association that should be updated.
        /// </param>
        /// <param name="id_usuario">
        /// An intenger identifier related to an user.
        /// </param>
        /// <param name="id_aplicacion">
        /// An integer identifier related to an application.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier updated.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarUsuario_Aplicacion(string sesion, int id_usuarioAplicacion, int id_usuario, int id_aplicacion, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);
                
                return this.modificarUsuario_Aplicacion(id, id_usuarioAplicacion, id_usuario, id_aplicacion, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an association between an user and a group in the database.
        /// </summary>
        /// <param name="id_usuarioCreador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user that will be associated.
        /// </param>
        /// <param name="id_grupo">
        /// An integer identifier related to the group that will be associated.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier created.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearUsuario_Grupo(int id_usuarioCreador, int id_usuario, int id_grupo, bool activo) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Grupo_Crear(
                    id_usuarioCreador,
                    id_usuario,
                    id_grupo,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    _res.mensaje = "OK";
                    _res.objeto  = id;
                }
                else
                    _res.mensaje = vResultado.Value.ToString();

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an association between an user and a group in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user that will be associated.
        /// </param>
        /// <param name="id_grupo">
        /// An integer identifier related to the group that will be associated.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier created.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearUsuario_Grupo(string sesion, int id_usuario, int id_grupo, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);

                return this.crearUsuario_Grupo(id, id_usuario, id_grupo, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an association between an user and a group in the database.
        /// </summary>
        /// <param name="id_usuarioModificador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_usuarioGrupo">
        /// An integer identifier related to the association that should be updated.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to an user.
        /// </param>
        /// <param name="id_grupo">
        /// An integer identifier related to a group.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier updated.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarUsuario_Grupo(int id_usuarioModificador, int id_usuarioGrupo, int id_usuario, int id_grupo, bool activo) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Grupo_Modificar(
                    id_usuarioModificador,
                    id_usuarioGrupo,
                    id_usuario,
                    id_grupo,
                    activo,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    _res.mensaje = "OK";
                    _res.objeto  = id;
                }
                else
                    _res.mensaje = vResultado.Value.ToString();

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an association between an user and a group in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_usuarioGrupo">
        /// An integer identifier related to the association that should be updated.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to an user.
        /// </param>
        /// <param name="id_grupo">
        /// An integer identifier related to a group.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier updated.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarUsuario_Grupo(string sesion, int id_usuarioGrupo, int id_usuario, int id_grupo, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);
                
                return this.modificarUsuario_Grupo(id, id_usuarioGrupo, id_usuario, id_grupo, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an association between an user and a profile in the database.
        /// </summary>
        /// <param name="id_usuarioCreador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user that will be associated.
        /// </param>
        /// <param name="id_perfil">
        /// An integer identifier related to the profile that will be associated.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier created.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearUsuario_Perfil(int id_usuarioCreador, int id_usuario, int id_perfil, bool activo) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Perfil_Crear(
                    id_usuarioCreador,
                    id_usuario,
                    id_perfil,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    _res.mensaje = "OK";
                    _res.objeto  = id;
                }
                else
                    _res.mensaje = vResultado.Value.ToString();

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an association between an user and a profile in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's sesion related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user that will be associated.
        /// </param>
        /// <param name="id_perfil">
        /// An integer identifier related to the profile that will be associated.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier created.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearUsuario_Perfil(string sesion, int id_usuario, int id_perfil, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);
                
                return this.crearUsuario_Perfil(id, id_usuario, id_perfil, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an association between an user and a profile in the database.
        /// </summary>
        /// <param name="id_usuarioModificador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_usuarioPerfil">
        /// An integer identifier related to the association that should be updated.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to an user.
        /// </param>
        /// <param name="id_perfil">
        /// An integer identifier related to a profile.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier updated.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarUsuario_Perfil(int id_usuarioModificador, int id_usuarioPerfil, int id_usuario, int id_perfil, bool activo) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Aplicacion_Modificar(
                    id_usuarioModificador,
                    id_usuarioPerfil,
                    id_usuario,
                    id_perfil,
                    activo,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    _res.mensaje = "OK";
                    _res.objeto  = id;
                }
                else
                    _res.mensaje = vResultado.Value.ToString();

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an association between an user and a profile in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's sesion related to the user that creates the request.
        /// </param>
        /// <param name="id_usuarioPerfil">
        /// An integer identifier related to the association that should be updated.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to an user.
        /// </param>
        /// <param name="id_perfil">
        /// An integer identifier related to a profile.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier updated.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarUsuario_Perfil(string sesion, int id_usuarioPerfil, int id_usuario, int id_perfil, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);
                
                return this.modificarUsuario_Perfil(id, id_usuarioPerfil, id_usuario, id_perfil, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        #endregion

        #region Secure

        /// <summary>
        /// Gets information about an user or all users registered in the database.
        /// </summary>
        /// <param name="id_usuario">
        /// An integer identifier related to an user. If set to a value less than or equal to <c>0</c> the method searchs all users.
        /// </param>
        /// <returns>
        /// A list that contains all information of the user/users registered in the database, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_usuario), usuario, usuarioCorreo, contrasena, Enc(id_perfil), activo]
        /// </returns>
        public List<Usuarios_Objetos.Sec_Usuario> buscarSec_Usuario(int id_usuario) {
            try {
                if (id_usuario <= 0)
                    return ue.Tb_UsuarioActivo
                        .Select(s => new {
                            id_usuario = s.id_usuario,
                            activo     = s.activo,
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Usuario(
                            e.id_usuario,
                            "",
                            "",
                            "",
                            ue.Tb_Usuario_Perfil.Where(tup => tup.id_usuario == e.id_usuario).Select(s => s.id_usuario).FirstOrDefault(),
                            e.activo,
                            null,
                            null
                        )).ToList();
                else
                    return ue.Tb_UsuarioActivo.Where(tua => tua.id_usuario == id_usuario)
                        .Select(s => new {
                            id_usuario = s.id_usuario,
                            activo     = s.activo,
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Usuario(
                            e.id_usuario,
                            "",
                            "",
                            "",
                            ue.Tb_Usuario_Perfil.Where(tup => tup.id_usuario == e.id_usuario).Select(s => s.id_usuario).FirstOrDefault(),
                            e.activo,
                            null,
                            null
                        )).ToList();
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Gets information about an user registered in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <returns>
        /// A list that contains all information of the user registered in the database, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_usuario), usuario, usuarioCorreo, contrasena, Enc(id_perfil), activo]
        /// </returns>
        public List<Usuarios_Objetos.Sec_Usuario> buscarSec_Usuario(string sesion) {
            int id = -1; 

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);

                return this.buscarSec_Usuario(id);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an user in the database.
        /// </summary>
        /// <param name="id_usuarioCreador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="usuario">
        /// A string that represents the user name.
        /// </param>
        /// <param name="usuarioCorreo">
        /// A string that represents the user email.
        /// </param>
        /// <param name="contrasena">
        /// A string that represents the user password.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the user. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record created, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_usuario), usuario, usuarioCorreo, contrasena, Enc(id_perfil), activo] 
        /// </returns>
        public Usuarios_Objetos.Respuesta crearSec_Usuario(int id_usuarioCreador, string usuario, string usuarioCorreo, string contrasena, bool activo = true) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Crear(
                    id_usuarioCreador,
                    Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(usuario), contrasena),
                    Utilidades.Hashing.SHA_512(usuario),
                    !string.IsNullOrEmpty(usuarioCorreo) ? Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(usuarioCorreo), contrasena) : null,
                    !string.IsNullOrEmpty(usuarioCorreo) ? Utilidades.Hashing.SHA_512(usuarioCorreo) : null,
                    Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(contrasena), contrasena),
                    activo,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    _res.mensaje = "OK";
                    _res.objeto  = new Usuarios_Objetos.Sec_Usuario(id, "", "", "", 0, false, null, null);
                }
                else
                    _res.mensaje = vResultado.Value.ToString();

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an user in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="usuario">
        /// A string that represents the user name.
        /// </param>
        /// <param name="usuarioCorreo">
        /// A string that represents the user email.
        /// </param>
        /// <param name="contrasena">
        /// A string that represents the user password.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the user. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record created, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_usuario), usuario, usuarioCorreo, contrasena, Enc(id_perfil), activo] 
        /// </returns>
        public Usuarios_Objetos.Respuesta crearSec_Usuario(string sesion, string usuario, string usuarioCorreo, string contrasena, bool activo = true) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);
                
                return this.crearSec_Usuario(id, usuario, usuarioCorreo, contrasena, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates a password for an user.
        /// </summary>
        /// <param name="id_usuarioModificador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user whose password should be updated.
        /// </param>
        /// <param name="contrasena">
        /// A string that represents the current user password.
        /// </param>
        /// <param name="contrasena_nueva">
        /// A string that represents the new user password.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_usuario), usuario, usuarioCorreo, contrasena, Enc(id_perfil), activo] 
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarSec_Contrasena(int id_usuarioModificador, int id_usuario, string contrasena, string contrasena_nueva) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            Usuarios_Objetos.Sec_Usuario _usuario = null;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                if (!string.IsNullOrEmpty(contrasena_nueva)) {
                    _usuario = ue.Tb_UsuarioActivo.Where(tu => tu.id_usuario == id_usuario && tu.Tb_Usuario.contrasena == Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(contrasena), contrasena))
                        .Select(s => new {
                            id_usuario    = s.id_usuario,
                            usuario       = s.Tb_Usuario.usuario,
                            usuarioCorreo = s.Tb_Usuario.usuarioCorreo,
                            activo        = s.activo
                        }).AsEnumerable()
                        .Select(e => new Usuarios_Objetos.Sec_Usuario(
                            e.id_usuario,
                            e.usuario,
                            e.usuarioCorreo,
                            "",
                            0,
                            e.activo,
                            null,
                            null
                        )).FirstOrDefault();

                    if (_usuario == null)
                        _res.mensaje = "ERROR - Usuario no encontrado";
                    else {
                        ue.Sp_Usuario_Modificar(
                            id_usuarioModificador,
                            id_usuario,
                            Utilidades.AES.encriptar(Utilidades.AES.desencriptar(_usuario.datos[1], contrasena), contrasena_nueva),
                            null,
                            !string.IsNullOrEmpty(_usuario.datos[2]) ? Utilidades.AES.encriptar(Utilidades.AES.desencriptar(_usuario.datos[2]), contrasena_nueva) : null,
                            null,
                            Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(contrasena_nueva), contrasena_nueva),
                            null,
                            vResultado
                        ).FirstOrDefault();

                        if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                            _res.mensaje      = "OK";
                            _usuario.datos[1] = "";
                            _usuario.datos[2] = "";
                            _res.objeto       = _usuario;
                        }
                        else
                            _res.mensaje = vResultado.Value.ToString();
                    }
                }
                else
                    _res.mensaje = "ERROR - Contrasena invalida";

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates a password for an user.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user whose password should be updated.
        /// </param>
        /// <param name="contrasena">
        /// A string that represents the current user password.
        /// </param>
        /// <param name="contrasena_nueva">
        /// A string that represents the new user password.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_usuario), usuario, usuarioCorreo, contrasena, Enc(id_perfil), activo] 
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarSec_Contrasena(string sesion, int id_usuario, string contrasena, string contrasena_nueva) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);

                return this.modificarSec_Contrasena(id, id_usuario, contrasena, contrasena_nueva);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an username for an user.
        /// </summary>
        /// <param name="id_usuarioModificador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user whose username should be updated.
        /// </param>
        /// <param name="usuario">
        /// A string that represents the new user name.
        /// </param>
        /// <param name="usuarioCorreo">
        /// A string that represents the new user email.
        /// </param>
        /// <param name="contrasena">
        /// A string that represent the current user password.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the user. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_usuario), usuario, usuarioCorreo, contrasena, Enc(id_perfil), activo]
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarSec_Usuario(int id_usuarioModificador, int id_usuario, string usuario, string usuarioCorreo, string contrasena, bool activo) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            Usuarios_Objetos.Sec_Usuario _usuario = null;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                _usuario = ue.Tb_UsuarioActivo.Where(tu => tu.id_usuario == id_usuario && tu.Tb_Usuario.contrasena == Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(contrasena), contrasena))
                    .Select(s => new {
                        id_usuario = s.id_usuario,
                        activo     = s.activo
                    }).AsEnumerable()
                    .Select(e => new Usuarios_Objetos.Sec_Usuario(
                        e.id_usuario,
                        "",
                        "",
                        "",
                        0,
                        e.activo,
                        null,
                        null
                    )).FirstOrDefault();

                if (_usuario == null)
                    _res.mensaje = "ERROR - Usuario no encontrado";
                else {
                    ue.Sp_Usuario_Modificar(
                        id_usuarioModificador,
                        id_usuario,
                        !string.IsNullOrEmpty(usuario) ? Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(usuario), contrasena) : null,
                        !string.IsNullOrEmpty(usuario) ? Utilidades.Hashing.SHA_512(usuario) : null,
                        !string.IsNullOrEmpty(usuarioCorreo) ? Utilidades.AES.encriptar(Utilidades.Hashing.SHA_512(usuarioCorreo), contrasena) : null,
                        !string.IsNullOrEmpty(usuarioCorreo) ? Utilidades.Hashing.SHA_512(usuarioCorreo) : null,
                        null,
                        activo,
                        vResultado
                    ).FirstOrDefault();

                    if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                        _res.mensaje = "OK";
                        _res.objeto  = _usuario;
                    }
                    else
                        _res.mensaje = vResultado.Value.ToString();
                }

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an username for an user.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user whose username should be updated.
        /// </param>
        /// <param name="usuario">
        /// A string that represents the new user name.
        /// </param>
        /// <param name="usuarioCorreo">
        /// A string that represents the new user email.
        /// </param>
        /// <param name="contrasena">
        /// A string that represent the current user password.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the user. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record updated, but some fields are encrypted.
        /// e.g.:
        /// [Enc(id_usuario), usuario, usuarioCorreo, contrasena, Enc(id_perfil), activo]
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarSec_Usuario(string sesion, int id_usuario, string usuario, string usuarioCorreo, string contrasena, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);
                
                return this.modificarSec_Usuario(id, id_usuario, usuario, usuarioCorreo, contrasena, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an association between an user and an application in the database.
        /// </summary>
        /// <param name="id_usuarioCreador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user that will be associated.
        /// </param>
        /// <param name="id_aplicacion">
        /// An integer identifier related to the application that will be associated.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier created, but the identifier is encrypted.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearSec_Usuario_Aplicacion(int id_usuarioCreador, int id_usuario, int id_aplicacion, bool activo) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Aplicacion_Crear(
                    id_usuarioCreador,
                    id_usuario,
                    id_aplicacion,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    _res.mensaje = "OK";
                    _res.objeto  = Utilidades.AES.encriptar(id.ToString());
                }
                else
                    _res.mensaje = vResultado.Value.ToString();

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an association between an user and an application in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user that will be associated.
        /// </param>
        /// <param name="id_aplicacion">
        /// An integer identifier related to the application that will be associated.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier created, but the identifier is encrypted.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearSec_Usuario_Aplicacion(string sesion, int id_usuario, int id_aplicacion, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);
                
                return this.crearSec_Usuario_Aplicacion(id, id_usuario, id_aplicacion, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an association between an user and an application in the database.
        /// </summary>
        /// <param name="id_usuarioModificador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_usuarioAplicacion">
        /// An integer identifier related to the association that should be updated.
        /// </param>
        /// <param name="id_usuario">
        /// An intenger identifier related to an user.
        /// </param>
        /// <param name="id_aplicacion">
        /// An integer identifier related to an application.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier updated, but the indentifier is encrypted.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarSec_Usuario_Aplicacion(int id_usuarioModificador, int id_usuarioAplicacion, int id_usuario, int id_aplicacion, bool activo) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Aplicacion_Modificar(
                    id_usuarioModificador,
                    id_usuarioAplicacion,
                    id_usuario,
                    id_aplicacion,
                    activo,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    _res.mensaje = "OK";
                    _res.objeto  = Utilidades.AES.encriptar(id.ToString());
                }
                else
                    _res.mensaje = vResultado.Value.ToString();

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an association between an user and an application in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_usuarioAplicacion">
        /// An integer identifier related to the association that should be updated.
        /// </param>
        /// <param name="id_usuario">
        /// An intenger identifier related to an user.
        /// </param>
        /// <param name="id_aplicacion">
        /// An integer identifier related to an application.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier updated, but the indentifier is encrypted.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarSec_Usuario_Aplicacion(string sesion, int id_usuarioAplicacion, int id_usuario, int id_aplicacion, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);

                return this.modificarSec_Usuario_Aplicacion(id, id_usuarioAplicacion, id_usuario, id_aplicacion, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an association between an user and a group in the database.
        /// </summary>
        /// <param name="id_usuarioCreador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user that will be associated.
        /// </param>
        /// <param name="id_grupo">
        /// An integer identifier related to the group that will be associated.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier created, but the identifier is encrypted.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearSec_Usuario_Grupo(int id_usuarioCreador, int id_usuario, int id_grupo, bool activo) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Grupo_Crear(
                    id_usuarioCreador,
                    id_usuario,
                    id_grupo,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    _res.mensaje = "OK";
                    _res.objeto  = Utilidades.AES.encriptar(id.ToString());
                }
                else
                    _res.mensaje = vResultado.Value.ToString();

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an association between an user and a group in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user that will be associated.
        /// </param>
        /// <param name="id_grupo">
        /// An integer identifier related to the group that will be associated.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier created, but the identifier is encrypted.
        /// </returns>
        public Usuarios_Objetos.Respuesta crearSec_Usuario_Grupo(string sesion, int id_usuario, int id_grupo, bool activo) {
           int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);

                return this.crearSec_Usuario_Grupo(id, id_usuario, id_grupo, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an association between an user and a group in the database.
        /// </summary>
        /// <param name="id_usuarioModificador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_usuarioGrupo">
        /// An integer identifier related to the association that should be updated.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to an user.
        /// </param>
        /// <param name="id_grupo">
        /// An integer identifier related to a group.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier updated, but the identifier is encrypted.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarSec_Usuario_Grupo(int id_usuarioModificador, int id_usuarioGrupo, int id_usuario, int id_grupo, bool activo) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Grupo_Modificar(
                    id_usuarioModificador,
                    id_usuarioGrupo,
                    id_usuario,
                    id_grupo,
                    activo,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    _res.mensaje = "OK";
                    _res.objeto  = Utilidades.AES.encriptar(id.ToString());
                }
                else
                    _res.mensaje = vResultado.Value.ToString();

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an association between an user and a group in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's usession related to the user that creates the request.
        /// </param>
        /// <param name="id_usuarioGrupo">
        /// An integer identifier related to the association that should be updated.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to an user.
        /// </param>
        /// <param name="id_grupo">
        /// An integer identifier related to a group.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier updated, but the identifier is encrypted.
        /// </returns>
        public Usuarios_Objetos.Respuesta modificarSec_Usuario_Grupo(string sesion, int id_usuarioGrupo, int id_usuario, int id_grupo, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);

                return this.modificarSec_Usuario_Grupo(id, id_usuarioGrupo, id_usuario, id_grupo, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an association between an user and a profile in the database.
        /// </summary>
        /// <param name="id_usuarioCreador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user that will be associated.
        /// </param>
        /// <param name="id_perfil">
        /// An integer identifier related to the profile that will be associated.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier created, but the identifier is encrypted.
        /// </returns
        public Usuarios_Objetos.Respuesta crearSec_Usuario_Perfil(int id_usuarioCreador, int id_usuario, int id_perfil, bool activo) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Perfil_Crear(
                    id_usuarioCreador,
                    id_usuario,
                    id_perfil,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    _res.mensaje = "OK";
                    _res.objeto  = Utilidades.AES.encriptar(id.ToString());
                }
                else
                    _res.mensaje = vResultado.Value.ToString();

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates an association between an user and a profile in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represents an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to the user that will be associated.
        /// </param>
        /// <param name="id_perfil">
        /// An integer identifier related to the profile that will be associated.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier created, but the identifier is encrypted.
        /// </returns
        public Usuarios_Objetos.Respuesta crearSec_Usuario_Perfil(string sesion, int id_usuario, int id_perfil, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);

                return this.crearSec_Usuario_Perfil(id, id_usuario, id_perfil, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an association between an user and a profile in the database.
        /// </summary>
        /// <param name="id_usuarioModificador">
        /// An integer identifier related to the user that creates the request.
        /// </param>
        /// <param name="id_usuarioPerfil">
        /// An integer identifier related to the association that should be updated.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to an user.
        /// </param>
        /// <param name="id_perfil">
        /// An integer identifier related to a profile.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier updated, but the identifier is encrypted.
        /// </returns
        public Usuarios_Objetos.Respuesta modificarSec_Usuario_Perfil(int id_usuarioModificador, int id_usuarioPerfil, int id_usuario, int id_perfil, bool activo) {
            Usuarios_Objetos.Respuesta _res = new Usuarios_Objetos.Respuesta();
            int id = -1;

            try {
                vResultado = new ObjectParameter("resultado", typeof(string));

                id = ue.Sp_Usuario_Aplicacion_Modificar(
                    id_usuarioModificador,
                    id_usuarioPerfil,
                    id_usuario,
                    id_perfil,
                    activo,
                    vResultado
                ).FirstOrDefault() ?? -1;

                if (!System.Text.RegularExpressions.Regex.IsMatch(vResultado.Value.ToString(), "ERROR")) {
                    _res.mensaje = "OK";
                    _res.objeto  = Utilidades.AES.encriptar(id.ToString());
                }
                else
                    _res.mensaje = vResultado.Value.ToString();

                return _res;
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// Updates an association between an user and a profile in the database.
        /// </summary>
        /// <param name="sesion">
        /// A string that represent an user's session related to the user that creates the request.
        /// </param>
        /// <param name="id_usuarioPerfil">
        /// An integer identifier related to the association that should be updated.
        /// </param>
        /// <param name="id_usuario">
        /// An integer identifier related to an user.
        /// </param>
        /// <param name="id_perfil">
        /// An integer identifier related to a profile.
        /// </param>
        /// <param name="activo">
        /// A boolean that represents the status of the association. If set to <c>true</c> the record will be active, otherwise it will be inactive.
        /// </param>
        /// <returns>
        /// A Respuesta object that contains information about the request status and the record identifier updated, but the identifier is encrypted.
        /// </returns
        public Usuarios_Objetos.Respuesta modificarSec_Usuario_Perfil(string sesion, int id_usuarioPerfil, int id_usuario, int id_perfil, bool activo) {
            int id = -1;

            try {
                id = ServicioUtilidades.buscarIdUsuario(sesion, ue);

                return this.modificarSec_Usuario_Perfil(id, id_usuarioPerfil, id_usuario, id_perfil, activo);
            }
            catch (Exception ex) {
                throw new Exception("ERROR:\n\tMessage: " + ex.Message + "\n\tException: " + ex.InnerException + "\n\tTrace: " + ex.StackTrace);
            }
        }

        #endregion
    }
}