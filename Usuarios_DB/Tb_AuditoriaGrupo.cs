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
    
    public partial class Tb_AuditoriaGrupo
    {
        public int id_auditoria { get; set; }
        public Nullable<int> id_usuario { get; set; }
        public string usuario { get; set; }
        public int id_registro { get; set; }
        public System.DateTime fechaRegistro { get; set; }
        public byte id_tipoAuditoria { get; set; }
        public string tabla { get; set; }
        public string datos_a { get; set; }
        public string datos_d { get; set; }
    
        public virtual Tb_TipoAuditoria Tb_TipoAuditoria { get; set; }
    }
}
