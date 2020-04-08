using System;
using System.ComponentModel.DataAnnotations;

namespace Model.Model
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
