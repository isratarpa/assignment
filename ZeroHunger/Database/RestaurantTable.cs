//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZeroHunger.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class RestaurantTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RestaurantTable()
        {
            this.CollectRequestTables = new HashSet<CollectRequestTable>();
        }
    
        public int RestId { get; set; }
        public string RestName { get; set; }
        public string RestLocation { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CollectRequestTable> CollectRequestTables { get; set; }
    }
}
