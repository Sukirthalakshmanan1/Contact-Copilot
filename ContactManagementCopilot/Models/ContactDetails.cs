// namespace ContactManagementCopilot.Models;
// public class ContactDetails
// {
//     public int Id { get; set; } //Id also auto generated here
//     public string Firstname { get; set; }
//     public string Lastname { get; set; }
//     public string Email { get; set; }
//     public string Phone { get; set; }
//     public string Address { get; set; }
// }
using System.ComponentModel.DataAnnotations;

namespace ContactManagementCopilot.Models
{
    public class ContactDetails : IContactDetails
    {
        
    [Key]
    public int Id { get; private set; }
    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
    public string Firstname { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Last name must be between 1 and 50 characters.")]
    public string Lastname { get; set; }

    
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }
        [Required]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
    public string Phone { get; set; }
    [Required]
    public string Address { get; set; }

    public string FullName => $"{Firstname} {Lastname}";

    }

    public interface IContactDetails
    {
        int Id { get; }
        string Firstname { get; set; }
        string Lastname { get; set; }
        string Email { get; set; }
        string Phone { get; set; }
        string Address { get; set; }
        string FullName { get; }
    }
}
public interface ILogger
{
    void Log(string message);
}
