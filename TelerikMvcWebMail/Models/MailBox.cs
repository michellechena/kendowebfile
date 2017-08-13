//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TelerikMvcWebMail.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MailBox
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MailBox()
        {
            this.MailBoxAccesses = new HashSet<MailBoxAccess>();
            this.MailBoxFolders = new HashSet<MailBoxFolder>();
        }
    
        public int MailBoxId { get; set; }
        public string MailBoxName { get; set; }
        public int UserId { get; set; }
        public long MailBoxSequence { get; set; }
    
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MailBoxAccess> MailBoxAccesses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MailBoxFolder> MailBoxFolders { get; set; }
    }
}
