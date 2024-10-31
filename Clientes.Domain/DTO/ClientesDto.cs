using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clientes.Domain.DTO;

public class ClientesDto
{
    public int ClientesId { get; set; }

    public string? Nombres { get; set; }

    public string? Whatsapp { get; set; }
}
