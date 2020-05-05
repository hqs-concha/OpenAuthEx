using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Model
{
    public class ValidateDto
    {
        [Required(ErrorMessage = "name must not be null")]
        public string Name { get; set; }
    }
}
