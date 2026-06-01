using Application.DTOs.Request.Cliente;
using Application.DTOs.Response.Cliente;
using Application.Exceptions;
using Application.Interfaces.Cliente;
using Domain.Entities;
using System.Linq.Expressions;

namespace Application.UseCases
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteCommand _clienteCommand;
        private readonly IClienteQuery _clienteQuery;

        //Constructor 

        public ClienteService(IClienteCommand clienteCommand, IClienteQuery clienteQuery)
        {
            _clienteCommand = clienteCommand;
            _clienteQuery = clienteQuery;
        }


        // Métodos

        //1. Crear cliente

        public async Task<ClienteResponse> CrearCliente(CrearClienteRequest request)
        {
            if (request.Dni <= 0)
            {
                throw new ExceptionBadRequest("Ingrese un número de DNI válido");
            }

            if (string.IsNullOrEmpty(request.Nombre) || string.IsNullOrEmpty(request.Apellido))
            {
                throw new ExceptionBadRequest("El nombre y el apellido son obligatorios");
            }


            var nuevoCliente = new Domain.Entities.Cliente
            {
                Dni = request.Dni,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Correo = request.Correo,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Localidad = request.Localidad,
                Pais = request.Pais,
                FechaNac = request.FechaNac,
                EsSocio = false,
                Estado = true
            };


            var clienteGuardado = await _clienteCommand.CrearCliente(nuevoCliente);

            return new ClienteResponse
            {
                Dni = clienteGuardado.Dni,
                Nombre = clienteGuardado.Nombre,
                Apellido = clienteGuardado.Apellido,
                Correo = clienteGuardado.Correo,
                Localidad = clienteGuardado.Localidad,
                Pais = clienteGuardado.Pais,
                EsSocio = clienteGuardado.EsSocio
            };

        }

        //2. Consultar cliente

        public async Task<ClienteResponse> ConsultarCliente(int dni)
        {
            if (dni <= 0) throw new ExceptionBadRequest("DNI no válido");

            var cliente = await _clienteQuery.ConsultarCliente(dni);
            if (cliente == null) throw new ExceptionNotFound("Cliente no encontrado.");

            return new ClienteResponse
            {
                Dni = cliente.Dni,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Correo = cliente.Correo,
                Localidad = cliente.Localidad,
                Pais = cliente.Pais,
                EsSocio = cliente.EsSocio
            };
        }

        //3. Modificar cliente

        public async Task<ClienteResponse> ModificarCliente(ActualizarClienteRequest request)
        {
            if (request.Dni <= 0) throw new ExceptionBadRequest("DNI no válido");

            var clienteExistente = await _clienteQuery.ConsultarCliente(request.Dni);
            if (clienteExistente == null) throw new ExceptionNotFound("Cliente no encontrado");

            clienteExistente.Nombre = request.Nombre;
            clienteExistente.Apellido = request.Apellido;
            clienteExistente.Correo = request.Correo;
            clienteExistente.Localidad = request.Localidad;
            clienteExistente.Pais = request.Pais;
            clienteExistente.EsSocio = (bool)request.EsSocio;

            var clienteActualizado = await _clienteCommand.ModificarCliente(clienteExistente);

            return new ClienteResponse
            {
                Dni = clienteActualizado.Dni,
                Nombre = clienteActualizado.Nombre,
                Apellido = clienteActualizado.Apellido,
                Correo = clienteActualizado.Correo,
                Localidad = clienteActualizado.Localidad,
                Pais = clienteActualizado.Pais,
                EsSocio = clienteActualizado.EsSocio
            };

                        
        }


        //4. Eliminar cliente

        public async Task<ClienteResponse> EliminarCliente(int dni)
        {
            if (dni <= 0) throw new ExceptionBadRequest("DNI no válido");

            var cliente = await _clienteQuery.ConsultarCliente(dni);
            if (cliente == null) throw new ExceptionNotFound("Cliente no encontrado");

            await _clienteCommand.EliminarCliente(cliente);

            return new ClienteResponse
            {
                Dni = cliente.Dni,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido
            };


        }


        //5. Listado clientes

        public async Task<List<ClienteResponse>> ListarClientes()
        {
            var listaClientes = await _clienteQuery.ListarClientes();

            return listaClientes.Select(c => new ClienteResponse
            {
                Dni = c.Dni,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Correo = c.Correo,
                Localidad = c.Localidad,
                Pais = c.Pais,
                EsSocio = c.EsSocio
            }).ToList();
        }
        
        //6. Login
        public async Task<ClienteShortResponse> Login(LoginRequest request)
        {
            if (String.IsNullOrWhiteSpace(request.Correo) && String.IsNullOrWhiteSpace(request.Password)) 
            {
                throw new ExceptionBadRequest("Credenciales invalidas");
            }
            var cliente = await _clienteQuery.ConsultarClientePorEmail(request.Correo);
            if(cliente == null) 
            {
                throw new ExceptionNotFound("Cliente no encontrado");
            }
            if(!BCrypt.Net.BCrypt.Verify(request.Password,cliente.Password)) 
            {
                throw new ExceptionBadRequest("Credenciales invalidas");
            }

            return new ClienteShortResponse
            {
               Dni= cliente.Dni,
               Nombre= cliente.Nombre,
               Apellido= cliente.Apellido,
            };




        }
    }
}

