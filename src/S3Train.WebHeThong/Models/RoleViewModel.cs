using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace S3Train.WebHeThong.Models
{
    public class RoleViewModel
    {
        public RoleViewModel() { }

        public RoleViewModel(ApplicationRole role)
        {
            Id = role.Id;
            Name = role.Name;
            Description = role.Description;
        }

        public string Id { get; set; }

        [Required(ErrorMessage = "Điền Tên Quyền")]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Display(Name = "Mô Tả")]
        public string Description { get; set; }
    }
}