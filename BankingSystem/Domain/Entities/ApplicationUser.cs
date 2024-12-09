using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BankingSystem.Application.Commands.UserRegistration;

namespace BankingSystem.Domain.Entities
{
    public class ApplicationUser : IdentityUser<string>
    {
        public DateTime DOB { get; set; }
        //public UserTypeEnum UserType { get; set; }

        public string BusinessName { get; set; }

        [Display(Name = "Last name")]
        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        [Display(Name = "First name")]
        [Column(TypeName = "VARCHAR")]
        public string FirstName { get; set; }
        public string Password { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(150)]
        public string ProfilePhotoName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool IsRegistrationVerified { get; set; }

        public int ProfileStatus { get; set; }

        
        public string? State { get; set; }

        public string? Country { get; set; }

       

        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        public static explicit operator ApplicationUser(RoleCommand source)
        {
            var destination = new ApplicationUser();
            destination.BusinessName = source.BusinessName;
            destination.FirstName = source.FirstName;
            destination.LastName = source.LastName;
            destination.Email = source.Email.Trim().ToLowerInvariant();
            destination.UserName = source.Email.Trim().ToLowerInvariant();
            destination.PhoneNumber = source.PhoneNumber;
            destination.RegistrationDate = DateTime.Now;
            destination.EmailConfirmed = false;
            return destination;
        }
    }
}
