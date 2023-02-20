using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BlogCore.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre del articulo")]
        public string? Name { get; set; }


        [Display(Name = "La descripcion es obligatoria")]
        public string? Description { get; set; }

        [Display(Name = "Fecha de creacion")]
        public string DateCreate { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string UrlImage { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Categories { get; set; }
    }
}
