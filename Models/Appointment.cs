using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace mvc.Models;

public class Appointment{

    public int ID { get; set; }    

    [Remote(action: "IsNotRepeatedAppointmentNumber", controller: "Appointment", ErrorMessage = "ErrorRepeatedAppointmentNumber")]
    [Required(ErrorMessage = "RequiredErrorMessage")] // Necessario colocar en todos para se ter tradução
    public string AppoitmentNumber { get; set; }

    [Display(Name="Date")]
    [Required]
    [DataType(DataType.Date)]
    public DateOnly Date { get; set; }

    [Display(Name="Time")]
    [Required]    
    public DateTime Time { get; set; }

    [DataType(DataType.MultilineText)]
    [Display(Name = "Informations")]
    public string Information { get; set; }
    
    [Required]
    [Display(Name ="Is Done?")]
    public bool IsDone { get; set; }

    [ValidateNever]
    public Staff Staff { get; set; }

    [Display(Name="Staff")]
    [Required]
    public int StaffID { get; set; }
    
    [ValidateNever]
    public Client Client { get; set; }

    [Display(Name="Client")]
    [Required]    
    public int ClientID { get; set; }

}