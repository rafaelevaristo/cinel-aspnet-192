using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace mvc.Models;

public class Client{

    public required int ID { get; set; }
    
    [Display(Name="Name")]
    [Required(ErrorMessage = "RequiredErrorMessage")] // Necessario colocar en todos para se ter tradução
    public string Name { get; set; }
    
    [Display(Name="Birthday")]
    [Required(ErrorMessage = "RequiredErrorMessage")] // Necessario colocar en todos para se ter tradução
    [DataType(DataType.Date)]
    public DateOnly Birthday { get; set; }
    
    [StringLength(100)]
    [Required(ErrorMessage = "RequiredErrorMessage")] // Necessario colocar en todos para se ter tradução
    public string Address { get; set; }
    
    [Display(Name="VAT")]
    [Required(ErrorMessage = "RequiredErrorMessage")] // Necessario colocar en todos para se ter tradução
    public string VATNumber { get; set; }    
    
    public DateTime AdmissionDate { get; set; }
    
    public DateTime DeactivationDate { get; set; }
    
    [Display(Name="Celphone")]
    public string? CellPhoneNumber { get; set; }
    [EmailAddress]
    [Required(ErrorMessage = "RequiredErrorMessage")] // Necessario colocar en todos para se ter tradução
    public string Email { get; set; }

}