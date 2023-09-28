namespace WpfApp5.DBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Logs = new HashSet<Log>();
            Settings = new HashSet<Setting>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public bool Active { get; set; }

        public bool Suspended { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime LastLoginDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedOn { get; set; }

        public int CreatedBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ModifiedOn { get; set; }

        public int ModifiedBy { get; set; }

        [Required]
        [StringLength(18)]
        public string Password { get; set; }

        public int Role { get; set; }

        public virtual Role RoleNavigation { get { return DataWorker.GetRoleById(id: Role); } }

        public virtual string RoleName { get { return DataWorker.GetRoleNameById(id: Role); } }

        public virtual string CreatedByName { get { return DataWorker.GetUserNameById(id: CreatedBy); } }

        public virtual string ModyfiedByName { get { return DataWorker.GetUserNameById(id: ModifiedBy); } }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Log> Logs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Setting> Settings { get; set; }
    }
}
