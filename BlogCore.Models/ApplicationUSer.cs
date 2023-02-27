using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class ApplicationUSer : IdentityUser
    {
        [Required(ErrorMessage = "Nombre es obligatorio")]
        public string Name  { get; set; }

        [Required(ErrorMessage = "La direccion es obligatorio")]
        public string Address { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatorio")]
        public string City { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatorio")]
        public string Country { get; set; }
    }
}
