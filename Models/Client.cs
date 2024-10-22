using System.ComponentModel.DataAnnotations;

namespace mvc.Models;

public class Client{

    public required int ID { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public DateOnly Birthday { get; set; }
    
    [StringLength(100)]
    [Required]
    public string Address { get; set; }
    
    [Required]
    public string VATNumber { get; set; }    
    
    public DateTime AdmissionDate { get; set; }
    
    public DateTime DeactivationDate { get; set; }
    
    public string? CellPhoneNumber { get; set; }
    [EmailAddress]
    [Required]
    public string Email { get; set; }

}