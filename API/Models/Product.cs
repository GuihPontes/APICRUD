using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Este campo é obrigatório")]
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        public string Title { get; set; }

        [MaxLength(1024,ErrorMessage ="Esse campo deve ter no máximo 1024 caracteres")]
        public string Description { get; set; }

        [Required(ErrorMessage ="Esse campo é obrigatório")]
        [Range (1,int.MaxValue, ErrorMessage ="o preço deve ser maior que zero")]
        public decimal Price { get; set; }

        [Required(ErrorMessage ="Esse campo é obrigatório")]
        [Range(1,int.MaxValue,ErrorMessage ="Categoria inválida")]
        public int CategoriaId { get; set; }
        public Category Category { get; set; }
    }
}
