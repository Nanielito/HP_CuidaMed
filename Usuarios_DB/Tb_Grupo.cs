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
    
    public partial class Tb_Grupo
    {
        public Tb_Grupo()
        {
            this.Tb_Usuario_Grupo = new HashSet<Tb_Usuario_Grupo>();
        }
    
        public short id_grupo { get; set; }
        public string codigo { get; set; }
        public string grupo { get; set; }
        public string descripcion { get; set; }
        public byte id_aplicacion { get; set; }
        public Nullable<int> id_usuarioCreador { get; set; }
        public Nullable<int> id_usuarioModificador { get; set; }
        public bool activo { get; set; }
    
        public virtual Tb_Aplicacion Tb_Aplicacion { get; set; }
        public virtual ICollection<Tb_Usuario_Grupo> Tb_Usuario_Grupo { get; set; }
    }
}
