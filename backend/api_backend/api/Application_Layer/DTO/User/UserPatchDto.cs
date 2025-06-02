using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Domain.Repositories;

namespace WebApplication1.Application_Layer.DTO.User
{
    public class UserPatchDto
    {


        [MaxLength(100)]
        public string? NewName { get; set; }

        [MaxLength(100)]
        public string? NewPassword { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? NewEmail { get; set; }

    }
}
