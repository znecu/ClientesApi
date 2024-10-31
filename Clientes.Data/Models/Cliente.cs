using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clientes.Data.Models;

public class Cliente
{
    [Key]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "En este campo solo se permiten letras. ")]
    public string? Nombres { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio. ")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Ingrese un número de teléfono válido. ")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Solo se permiten números.")]
    public string? Whatsapp { get; set; }
}