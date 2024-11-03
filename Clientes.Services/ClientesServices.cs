using Clientes.Abstractions;
using Clientes.Data.Context;
using Clientes.Data.Models;
using Clientes.Domain.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Clientes.Services;

public class ClientesServices(IDbContextFactory<Contexto> DbFactory) : IClientesService
{
    private async Task<bool> Existe(int ClientesId)
    {
        await using var _contexto = await DbFactory.CreateDbContextAsync();
        return await _contexto.Cliente
            .AnyAsync(c => c.ClienteId == ClientesId);
    }

    public async Task<bool> ExisteCliente(int ClientesId, string Nombres, string whatsapp)
    {
        await using var _contexto = await DbFactory.CreateDbContextAsync();
        return await _contexto.Cliente
            .AnyAsync(c => c.ClienteId != ClientesId &&
            (c.Whatsapp.Equals(whatsapp) || c.Nombres.ToLower().Equals(Nombres.ToLower())));
    }

    private async Task<bool> Insertar(ClientesDto clienteDto)
    {
        await using var _contexto = await DbFactory.CreateDbContextAsync();
        var cliente = new Cliente()
        {
            Nombres = clienteDto.Nombres,
            Whatsapp = clienteDto.Whatsapp
        };
        _contexto.Cliente.Add(cliente);
        var guardo = await _contexto
            .SaveChangesAsync() > 0;
        clienteDto.ClienteId = cliente.ClienteId;
        return guardo;
    }

    private async Task<bool> Modificar(ClientesDto clienteDto)
    {
        await using var _contexto = await DbFactory.CreateDbContextAsync();
        var cliente = new Cliente()
        {
            ClienteId = clienteDto.ClienteId,
            Nombres = clienteDto.Nombres,
            Whatsapp= clienteDto.Whatsapp
        };
        _contexto.Cliente.Update(cliente);
        var modificado = await _contexto
            .SaveChangesAsync() > 0;
        return modificado;
    }

    public async Task<bool> Guardar(ClientesDto cliente)
    {
        if (!await Existe(cliente.ClienteId))
            return await Insertar(cliente);
        else
            return await Modificar(cliente);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var _contexto = await DbFactory.CreateDbContextAsync();
        var clientes = await _contexto.Cliente
            .Where(c => c.ClienteId == id)
            .ExecuteDeleteAsync();
        return clientes > 0;
    }

    public async Task<ClientesDto> Buscar(int id)
    {
        await using var _contexto = await DbFactory.CreateDbContextAsync();
        var cliente = await _contexto.Cliente
            .Where(c => c.ClienteId == id)
            .Select(c => new ClientesDto()
            {
                ClienteId = c.ClienteId,
                Nombres = c.Nombres,
                Whatsapp = c.Whatsapp
            }).FirstOrDefaultAsync();
        return cliente ?? new ClientesDto();
    }

    public async Task<List<ClientesDto>> Listar(Expression<Func<ClientesDto, bool>> criterio)
    {
        await using var _contexto = await DbFactory.CreateDbContextAsync();
        return await _contexto.Cliente
            .Select(c => new ClientesDto()
            {
                ClienteId = c.ClienteId,
                Nombres = c.Nombres,
                Whatsapp = c.Whatsapp
            })
            .Where(criterio)
            .ToListAsync();
    }
}
