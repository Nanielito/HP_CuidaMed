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
    
    public partial class Tb_TipoAuditoria
    {
        public Tb_TipoAuditoria()
        {
            this.Tb_AuditoriaAplicacion = new HashSet<Tb_AuditoriaAplicacion>();
            this.Tb_AuditoriaGrupo = new HashSet<Tb_AuditoriaGrupo>();
            this.Tb_AuditoriaIdioma = new HashSet<Tb_AuditoriaIdioma>();
            this.Tb_AuditoriaPerfil = new HashSet<Tb_AuditoriaPerfil>();
            this.Tb_AuditoriaUsuario = new HashSet<Tb_AuditoriaUsuario>();
            this.Tb_AuditoriaVariable = new HashSet<Tb_AuditoriaVariable>();
            this.Tb_TipoAuditoria_T = new HashSet<Tb_TipoAuditoria_T>();
        }
    
        public byte id_tipoAuditoria { get; set; }
        public string codigo { get; set; }
        public bool activo { get; set; }
    
        public virtual ICollection<Tb_AuditoriaAplicacion> Tb_AuditoriaAplicacion { get; set; }
        public virtual ICollection<Tb_AuditoriaGrupo> Tb_AuditoriaGrupo { get; set; }
        public virtual ICollection<Tb_AuditoriaIdioma> Tb_AuditoriaIdioma { get; set; }
        public virtual ICollection<Tb_AuditoriaPerfil> Tb_AuditoriaPerfil { get; set; }
        public virtual ICollection<Tb_AuditoriaUsuario> Tb_AuditoriaUsuario { get; set; }
        public virtual ICollection<Tb_AuditoriaVariable> Tb_AuditoriaVariable { get; set; }
        public virtual ICollection<Tb_TipoAuditoria_T> Tb_TipoAuditoria_T { get; set; }
    }
}
