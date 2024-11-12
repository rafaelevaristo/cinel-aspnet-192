using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mvc.Models;

public class Client{

    public required int ID { get; set; }
    
    [Display(Name="Name")]
    [Required]
    public string Name { get; set; }
    
    [Display(Name="Birthday")]
    [Required]
    [DataType(DataType.Date)]
    public DateOnly Birthday { get; set; }
    
    [StringLength(100)]
    [Required]
    public string Address { get; set; }
    
    [Display(Name="VAT")]
    [Required]
    public string VATNumber { get; set; }    
    
    public DateTime AdmissionDate { get; set; }
    
    public DateTime DeactivationDate { get; set; }
    
    [Display(Name="Celphone")]
    public string? CellPhoneNumber { get; set; }
    [EmailAddress]
    [Required]
    public string Email { get; set; }

}