//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApplication1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Campus
    {
        public Campus()
        {
            this.Teacher = new HashSet<Teacher>();
            this.Teacher1 = new HashSet<Teacher>();
            this.Teacher2 = new HashSet<Teacher>();
        }
    
        public int Id { get; set; }
        public string CampusName { get; set; }
    
        public virtual ICollection<Teacher> Teacher { get; set; }
        public virtual ICollection<Teacher> Teacher1 { get; set; }
        public virtual ICollection<Teacher> Teacher2 { get; set; }
    }
}
