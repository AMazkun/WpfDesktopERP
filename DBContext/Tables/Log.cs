namespace WpfApp5.DBContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Log")]
    public partial class Log
    {
        public int Id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Date { get; set; }

        public bool Error { get; set; }

        public int? User { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(4000)]
        public string Message { get; set; }

        public virtual User theUser { get; set; }
        public virtual string theUserName { get { return DataWorker.GetUserNameById(User ?? 0); } }

    }
}
