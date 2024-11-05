using System.ComponentModel.DataAnnotations;

namespace mvc.Models;

public class Staff{

    public required int ID { get; set; }
    
    [Required]
    public string Name { get; set; }

    [StringLength(10)]
    [Required]
    public string StaffNumber { get; set; }

    [StringLength(100)]
    [Required]
    public string Address { get; set; }
    
    [Required]
    public string VATNumber { get; set; }    
    
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    
    public string? CellPhoneNumber { get; set; }

    
    public DateTime AdmissionDate { get; set; }
    
    public DateTime DeactivationDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateOnly Birthday { get; set; }


}