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
    
    public partial class Tb_Idioma
    {
        public Tb_Idioma()
        {
            this.Tb_Aplicacion_T = new HashSet<Tb_Aplicacion_T>();
            this.Tb_Perfil_T = new HashSet<Tb_Perfil_T>();
            this.Tb_Variable_T = new HashSet<Tb_Variable_T>();
            this.Tb_TipoAuditoria_T = new HashSet<Tb_TipoAuditoria_T>();
            this.Tb_Idioma_T = new HashSet<Tb_Idioma_T>();
        }
    
        public byte id_idioma { get; set; }
        public string codigo { get; set; }
        public Nullable<int> id_usuarioCreador { get; set; }
        public Nullable<int> id_usuarioModificador { get; set; }
        public bool activo { get; set; }
    
        public virtual ICollection<Tb_Aplicacion_T> Tb_Aplicacion_T { get; set; }
        public virtual ICollection<Tb_Perfil_T> Tb_Perfil_T { get; set; }
        public virtual ICollection<Tb_Variable_T> Tb_Variable_T { get; set; }
        public virtual ICollection<Tb_TipoAuditoria_T> Tb_TipoAuditoria_T { get; set; }
        public virtual ICollection<Tb_Idioma_T> Tb_Idioma_T { get; set; }
    }
}
