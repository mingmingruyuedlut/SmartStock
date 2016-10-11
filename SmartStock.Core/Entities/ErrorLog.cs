using System;

namespace SmartStock.Core.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(ErrorLog))]
    public class ErrorLog
    {
        public Nullable<System.DateTime> ErrorTime { get; set; }

        [StringLength(1000)]
        public string ErrorNote { get; set; }
    }
}
