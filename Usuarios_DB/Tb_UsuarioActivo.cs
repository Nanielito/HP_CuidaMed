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
    
    public partial class Tb_UsuarioActivo
    {
        public int id_usuarioActivo { get; set; }
        public int id_usuario { get; set; }
        public Nullable<int> id_usuarioCreador { get; set; }
        public Nullable<int> id_usuarioModificador { get; set; }
        public bool activo { get; set; }
    
        public virtual Tb_Usuario Tb_Usuario { get; set; }
    }
}