using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using pacientesAPI.Models;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration; // Necessário para o IConfiguration

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
        [HttpGet("{id}")] // Corrigido para bater com o nome do parâmetro 'id'
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
            return NotFound();
        }

        // GET: api/pacientes/nascimento/ano/1989
        [HttpGet("nascimento/ano/{ano:int}")] // Corrigido para bater com a variável 'ano'
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

        private Paciente MapearPaciente(SqlDataReader reader)
        {
            return new Paciente
            {
                id_paciente = (int)reader["id_paciente"],
                nome_completo = reader["nome_completo"].ToString(),
                data_nascimento = (DateTime)reader["data_nascimento"],
                sexo = reader["sexo"].ToString(),
                nome_mae = reader["nome_mae"].ToString(),
                cpf = reader["cpf"].ToString(),
                rg = reader["rg"].ToString(),
                cns = reader["cns"].ToString(),
                numero_prontuario = reader["numero_prontuario"].ToString(),
                telefone = reader["telefone"].ToString(),
                telefone_emergencia = reader["telefone_emergencia"].ToString(),
                email = reader["email"].ToString(),
                data_cadastro = (DateTime)reader["data_cadastro"]
            };
        }

        // Post 
        [HttpPost]
        public IActionResult Criar([FromBody] Paciente paciente)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "INSERT INTO Pacientes(Nome_Completo, data_nascimento, sexo, nome_mae, cpf, rg, cns, numero_prontuario, telefone, telefone_emergencia, email, data_cadastro) " +
                             "VALUES(@nome_completo, @data_nascimento, @sexo, @nome_mae, @cpf, @rg, @cns, @numero_prontuario, @telefone, @telefone_emergencia, @email, @data_cadastro)";

                // Variável 'connection' adicionada ao SqlCommand
                using (var command = new SqlCommand(sql, connection))
                {
                    // Trocado Paciente (Classe) por paciente (Objeto instanciado)
                    command.Parameters.AddWithValue("@nome_completo", paciente.nome_completo);
                    command.Parameters.AddWithValue("@data_nascimento", paciente.data_nascimento);
                    command.Parameters.AddWithValue("@sexo", paciente.sexo);
                    command.Parameters.AddWithValue("@nome_mae", paciente.nome_mae);
                    command.Parameters.AddWithValue("@cpf", paciente.cpf);
                    command.Parameters.AddWithValue("@rg", paciente.rg);
                    command.Parameters.AddWithValue("@cns", paciente.cns);
                    command.Parameters.AddWithValue("@numero_prontuario", paciente.numero_prontuario);
                    command.Parameters.AddWithValue("@telefone", paciente.telefone);
                    command.Parameters.AddWithValue("@telefone_emergencia", paciente.telefone_emergencia);
                    command.Parameters.AddWithValue("@email", paciente.email);

                    command.Parameters.AddWithValue("@data_cadastro", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }

            return StatusCode(201, paciente);
        }

        // PUT: api/pacientes/1
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, [FromBody] Paciente paciente)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = @"
                    UPDATE pacientes 
                    SET nome_completo = @nome_completo, 
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
                    command.Parameters.AddWithValue("@nome_completo", paciente.nome_completo);
                    command.Parameters.AddWithValue("@data_nascimento", paciente.data_nascimento);
                    command.Parameters.AddWithValue("@sexo", paciente.sexo);
                    command.Parameters.AddWithValue("@nome_mae", paciente.nome_mae);

                    // Tratamento para evitar que campos nulos quebrem o banco de dados
                    command.Parameters.AddWithValue("@cpf", (object)paciente.cpf ?? DBNull.Value);
                    command.Parameters.AddWithValue("@rg", (object)paciente.rg ?? DBNull.Value);
                    command.Parameters.AddWithValue("@cns", (object)paciente.cns ?? DBNull.Value);
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

            return NoContent(); // O padrão HTTP para PUT com sucesso é retornar 204 NoContent
        }

        // DELETE: api/pacientes/5
        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql =
                    "DELETE FROM pacientes WHERE id_paciente = @id_paciente";


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

            // Retorna 204 No Content (sucesso, sem conteúdo na resposta), que é o padrão correto para DELETE
            return NoContent();
        }
    }
}