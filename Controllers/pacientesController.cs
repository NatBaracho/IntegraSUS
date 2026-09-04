using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using pacientesAPI.Models;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace pacientesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class pacientesController : ControllerBase
    {
        private readonly string connectionString;

        public pacientesController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("IntegraSUSConnection");
        }

        // GET: api/pacientes
        [HttpGet]
        public IActionResult Listartodos()
        {
            var pacientes = new List<Paciente>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM pacientes", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pacientes.Add(MapearPaciente(reader));
                    }
                }
            }

            return Ok(pacientes);
        }

        // GET: api/pacientes/5
        [HttpGet("{id}")]
        public IActionResult BuscarPorId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM pacientes WHERE id_paciente = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var paciente = MapearPaciente(reader);
                        return Ok(paciente);
                    }
                }
            }
            return NotFound(new { mensagem = "Paciente não encontrado." });
        }

        // GET: api/pacientes/nascimento/ano/1989
        [HttpGet("nascimento/ano/{ano:int}")]
        public IActionResult BuscarPorIdadeEmRelacaoAoAno(int ano)
        {
            var listaPacientes = new List<Paciente>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM pacientes WHERE YEAR(data_nascimento) < @Ano", connection);
                command.Parameters.AddWithValue("@Ano", ano);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaPacientes.Add(MapearPaciente(reader));
                    }
                }
            }

            if (listaPacientes.Count == 0)
            {
                return NotFound(new { mensagem = $"Nenhum paciente encontrado com ano base {ano}." });
            }

            return Ok(listaPacientes);
        }

        // Método Auxiliar de Mapeamento com tratamento para valores Nulos (DBNull)
        private Paciente MapearPaciente(SqlDataReader reader)
        {
            return new Paciente
            {
                id_paciente = (int)reader["id_paciente"],
                id_responsavel = reader["id_responsavel"] != DBNull.Value ? (int?)reader["id_responsavel"] : null,
                id_usuario_cadastro = reader["id_usuario_cadastro"] != DBNull.Value ? (int?)reader["id_usuario_cadastro"] : null,
                nome_completo = reader["nome_completo"].ToString(),
                data_nascimento = (DateTime)reader["data_nascimento"],
                sexo = reader["sexo"].ToString(),
                nome_mae = reader["nome_mae"].ToString(),
                cns = reader["cns"].ToString(),

                cpf = reader["cpf"] != DBNull.Value ? reader["cpf"].ToString() : null,
                rg = reader["rg"] != DBNull.Value ? reader["rg"].ToString() : null,
                numero_prontuario = reader["numero_prontuario"] != DBNull.Value ? reader["numero_prontuario"].ToString() : null,
                telefone = reader["telefone"] != DBNull.Value ? reader["telefone"].ToString() : null,
                telefone_emergencia = reader["telefone_emergencia"] != DBNull.Value ? reader["telefone_emergencia"].ToString() : null,
                email = reader["email"] != DBNull.Value ? reader["email"].ToString() : null,
                data_cadastro = reader["data_cadastro"] != DBNull.Value ? (DateTime?)reader["data_cadastro"] : null
            };
        }

        // POST: api/pacientes
        [HttpPost]
        public IActionResult Criar([FromBody] Paciente paciente)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    INSERT INTO Pacientes(id_responsavel, id_usuario_cadastro, nome_completo, data_nascimento, sexo, nome_mae, cpf, rg, cns, numero_prontuario, telefone, telefone_emergencia, email, data_cadastro) 
                    VALUES(@id_responsavel, @id_usuario_cadastro, @nome_completo, @data_nascimento, @sexo, @nome_mae, @cpf, @rg, @cns, @numero_prontuario, @telefone, @telefone_emergencia, @email, @data_cadastro)";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id_responsavel", (object)paciente.id_responsavel ?? DBNull.Value);
                    command.Parameters.AddWithValue("@id_usuario_cadastro", (object)paciente.id_usuario_cadastro ?? DBNull.Value);
                    command.Parameters.AddWithValue("@nome_completo", paciente.nome_completo);
                    command.Parameters.AddWithValue("@data_nascimento", paciente.data_nascimento);
                    command.Parameters.AddWithValue("@sexo", paciente.sexo);
                    command.Parameters.AddWithValue("@nome_mae", paciente.nome_mae);
                    command.Parameters.AddWithValue("@cns", paciente.cns);

                    // Tratamento para evitar que campos nulos quebrem o banco de dados no POST
                    command.Parameters.AddWithValue("@cpf", (object)paciente.cpf ?? DBNull.Value);
                    command.Parameters.AddWithValue("@rg", (object)paciente.rg ?? DBNull.Value);
                    command.Parameters.AddWithValue("@numero_prontuario", (object)paciente.numero_prontuario ?? DBNull.Value);
                    command.Parameters.AddWithValue("@telefone", (object)paciente.telefone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@telefone_emergencia", (object)paciente.telefone_emergencia ?? DBNull.Value);
                    command.Parameters.AddWithValue("@email", (object)paciente.email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@data_cadastro", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }

            return StatusCode(201, paciente);
        }

        // PUT: api/pacientes/5
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, [FromBody] Paciente paciente)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = @"
                    UPDATE pacientes 
                    SET id_responsavel = @id_responsavel,
                        nome_completo = @nome_completo, 
                        data_nascimento = @data_nascimento, 
                        sexo = @sexo,
                        nome_mae = @nome_mae, 
                        cpf = @cpf, 
                        rg = @rg, 
                        cns = @cns, 
                        numero_prontuario = @numero_prontuario,
                        telefone = @telefone,
                        telefone_emergencia = @telefone_emergencia,
                        email = @email
                    WHERE id_paciente = @id_paciente";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id_responsavel", (object)paciente.id_responsavel ?? DBNull.Value);
                    command.Parameters.AddWithValue("@nome_completo", paciente.nome_completo);
                    command.Parameters.AddWithValue("@data_nascimento", paciente.data_nascimento);
                    command.Parameters.AddWithValue("@sexo", paciente.sexo);
                    command.Parameters.AddWithValue("@nome_mae", paciente.nome_mae);

                    // Tratamento de nulos
                    command.Parameters.AddWithValue("@cpf", (object)paciente.cpf ?? DBNull.Value);
                    command.Parameters.AddWithValue("@rg", (object)paciente.rg ?? DBNull.Value);
                    command.Parameters.AddWithValue("@cns", (object)paciente.cns ?? DBNull.Value); // NOT NULL no BD, mas protegido aqui
                    command.Parameters.AddWithValue("@numero_prontuario", (object)paciente.numero_prontuario ?? DBNull.Value);
                    command.Parameters.AddWithValue("@telefone", (object)paciente.telefone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@telefone_emergencia", (object)paciente.telefone_emergencia ?? DBNull.Value);
                    command.Parameters.AddWithValue("@email", (object)paciente.email ?? DBNull.Value);

                    command.Parameters.AddWithValue("@id_paciente", id);

                    int linhasAfetadas = command.ExecuteNonQuery();

                    if (linhasAfetadas == 0)
                    {
                        return NotFound(new { mensagem = "Paciente não encontrado." });
                    }
                }
            }

            return NoContent();
        }

        // DELETE: api/pacientes/5
        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "DELETE FROM pacientes WHERE id_paciente = @id_paciente";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id_paciente", id);

                    int linhasAfetadas = command.ExecuteNonQuery();

                    if (linhasAfetadas == 0)
                    {
                        return NotFound(new { mensagem = "Paciente não encontrado." });
                    }
                }
            }

            return NoContent();
        }
    }
}