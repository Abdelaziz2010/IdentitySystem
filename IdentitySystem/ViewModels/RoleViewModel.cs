using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentitySystem.ViewModels
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {
            Users = new List<string>();
        }
        public string Id { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
