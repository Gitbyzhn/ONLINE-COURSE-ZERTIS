//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineCourseZertis.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class VideoL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VideoL()
        {
            this.Tests = new HashSet<Test>();
        }
    
        public int Id { get; set; }
        public double XId { get; set; }
        public string Name { get; set; }
        public int ModulId { get; set; }
        public string Link { get; set; }
        public string ShortT { get; set; }
        public string Iconimg { get; set; }
        public string Iconimg2 { get; set; }
        public string language { get; set; }
    
        public virtual Modul Modul { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Tests { get; set; }
        public virtual VideoXL VideoXL { get; set; }
    }
}
