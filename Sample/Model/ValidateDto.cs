using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Model
{
    public class ValidateDto
    {
        [Required(ErrorMessage = "id must not be null")]
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string Description { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public System.DateTime? UpdateTime { get; set; }
        public int Status { get; set; }
    }
}
