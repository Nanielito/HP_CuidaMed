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
    
    public partial class Tb_Sesion
    {
        public int id_sesion { get; set; }
        public string sesion { get; set; }
        public System.DateTime fechaInicio { get; set; }
        public System.DateTime fechaUltimaOp { get; set; }
        public bool activo { get; set; }
        public int id_usuario { get; set; }
    
        public virtual Tb_Usuario Tb_Usuario { get; set; }
    }
}