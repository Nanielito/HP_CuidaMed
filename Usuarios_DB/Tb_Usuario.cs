//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Usuarios_DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tb_Usuario
    {
        public Tb_Usuario()
        {
            this.Tb_Contrasena = new HashSet<Tb_Contrasena>();
            this.Tb_Sesion = new HashSet<Tb_Sesion>();
            this.Tb_Usuario_Aplicacion = new HashSet<Tb_Usuario_Aplicacion>();
            this.Tb_Usuario_Grupo = new HashSet<Tb_Usuario_Grupo>();
            this.Tb_Usuario_Perfil = new HashSet<Tb_Usuario_Perfil>();
            this.Tb_UsuarioActivo = new HashSet<Tb_UsuarioActivo>();
        }
    
        public int id_usuario { get; set; }
        public string usuario { get; set; }
        public string usuario_ { get; set; }
        public string usuarioCorreo { get; set; }
        public string usuarioCorreo_ { get; set; }
        public string contrasena { get; set; }
        public System.DateTime fechaRegistro { get; set; }
        public Nullable<int> id_usuarioCreador { get; set; }
        public Nullable<int> id_usuarioModificador { get; set; }
    
        public virtual ICollection<Tb_Contrasena> Tb_Contrasena { get; set; }
        public virtual ICollection<Tb_Sesion> Tb_Sesion { get; set; }
        public virtual ICollection<Tb_Usuario_Aplicacion> Tb_Usuario_Aplicacion { get; set; }
        public virtual ICollection<Tb_Usuario_Grupo> Tb_Usuario_Grupo { get; set; }
        public virtual ICollection<Tb_Usuario_Perfil> Tb_Usuario_Perfil { get; set; }
        public virtual ICollection<Tb_UsuarioActivo> Tb_UsuarioActivo { get; set; }
    }
}
