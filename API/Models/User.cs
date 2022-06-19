using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="O campo é obrigatório")]
        [MaxLength(20,ErrorMessage ="O campo deve conter entre 3 e 20 caracteres")]
        [MinLength(3,ErrorMessage = "O campo deve conter entre 3 e 20 caracteres")]
        public string Username { get; set; }


        [Required(ErrorMessage = "O campo é obrigatório")]
        [MaxLength(20, ErrorMessage = "O campo deve conter entre 3 e 20 caracteres")]
        [MinLength(3, ErrorMessage = "O campo deve conter entre 3 e 20 caracteres")]
        public string Password { get; set; }


        public string Role { get; set; }
        
    }
}
