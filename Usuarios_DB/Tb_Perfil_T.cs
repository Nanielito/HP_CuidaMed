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
    
    public partial class Tb_Perfil_T
    {
        public byte id_perfil_T { get; set; }
        public string perfil { get; set; }
        public string descripcion { get; set; }
        public byte id_perfil { get; set; }
        public byte id_idioma { get; set; }
        public Nullable<int> id_usuarioCreador { get; set; }
        public Nullable<int> id_usuarioModificador { get; set; }
        public bool activo { get; set; }
    
        public virtual Tb_Perfil Tb_Perfil { get; set; }
        public virtual Tb_Idioma Tb_Idioma { get; set; }
    }
}