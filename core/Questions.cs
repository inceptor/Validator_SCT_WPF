//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _164253
{
    using System;
    using System.Collections.Generic;
    
    public partial class Questions
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public Nullable<bool> Bonus { get; set; }
        public string CodeMat { get; set; }
    
        public virtual Courses Courses { get; set; }
    }
}
