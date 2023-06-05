namespace SavourySolutions.Data.Common.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public abstract class BaseModel<TKey> : IAuditInfo
    {
        [Key]
        public TKey Id { get; set; }

        public DateTime CreatedOn { get; set; }=DateTime.Now;

        public DateTime? ModifiedOn { get; set; }
    }
}
