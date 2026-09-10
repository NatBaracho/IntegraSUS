using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using DadosClinicosAPI.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DadosClinicosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Dados_ClinicosController : ControllerBase
    {
        private readonly string connectionStrings;

        public Dados_ClinicosController(IConfiguration configuration)
        {
            connectionStrings = configuration.GetConnectionString("IntegraSUSConnection")
                ?? throw new InvalidOperationException("Connection string 'IntegraSUSConnection' não configurada.");
        }

        // GET: api/Dados_Clinicos
        [HttpGet]
        public IActionResult ListarDados()
        {
            var dadosClinicosList = new List<Models.Dados_Clinicos>();

            using (var connection = new SqlConnection(connectionStrings))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM dados_clinicos", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dadosClinicosList.Add(MapearDadoClinicos(reader));
                    }
                }
            }

            return Ok(dadosClinicosList);
        }

        // GET: api/Dados_Clinicos/5
        [HttpGet("{id}")]
        public IActionResult ListarId(int id)
        {
            using (var connection = new SqlConnection(connectionStrings))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM dados_clinicos WHERE id_dado_clinico = @id", connection);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var dadoClinico = MapearDadoClinicos(reader);
                        return Ok(dadoClinico);
                    }
                }
            }

            return NotFound("Dado clínico não encontrado!");
        }

        // Método auxiliar para conversão do DataReader para o Model
        private Models.Dados_Clinicos MapearDadoClinicos(SqlDataReader reader)
        {
            return new Models.Dados_Clinicos
            {
                id_dado_clinico = (int)reader["id_dado_clinico"],
                id_paciente = (int)reader["id_paciente"],
                id_usuario_registro = reader["id_usuario_registro"] != DBNull.Value ? (int?)reader["id_usuario_registro"] : null,
                tipo_sanguineo = reader["tipo_sanguineo"] != DBNull.Value ? reader["tipo_sanguineo"].ToString() : null,
                alergias = reader["alergias"] != DBNull.Value ? reader["alergias"].ToString() : null,
                hipertensao = reader["hipertensao"] != DBNull.Value && (bool)reader["hipertensao"],
                diabetes = reader["diabetes"] != DBNull.Value && (bool)reader["diabetes"],
                outras_doencas_cronicas = reader["outras_doencas_cronicas"] != DBNull.Value ? reader["outras_doencas_cronicas"].ToString() : null,
                observacoes_medicas = reader["observacoes_medicas"] != DBNull.Value ? reader["observacoes_medicas"].ToString() : null,
                data_registro = reader["data_registro"] != DBNull.Value ? (DateTime)reader["data_registro"] : DateTime.MinValue
            };
        }

        // ==========================================================
        // Métodos auxiliares de validação
        // ==========================================================

        // Valida tamanhos máximos dos campos varchar e obrigatoriedade básica.
        // Retorna null se estiver tudo certo, ou a mensagem de erro caso contrário.
        private string? ValidarCampos(Models.Dados_Clinicos dados)
        {
            if (dados.id_paciente <= 0)
                return "id_paciente é obrigatório e deve ser maior que zero.";

            if (!string.IsNullOrEmpty(dados.tipo_sanguineo) && dados.tipo_sanguineo.Length > 5)
                return "Tipo sanguíneo inválido (máx. 5 caracteres).";

            // alergias, outras_doencas_cronicas e observacoes_medicas são varchar(MAX) no banco,
            // então não precisam de validação de tamanho — mas fica o exemplo caso o tipo mude:
            // if (!string.IsNullOrEmpty(dados.alergias) && dados.alergias.Length > 8000)
            //     return "Campo 'alergias' excede o tamanho máximo permitido.";

            return null;
        }

        // Verifica se o id_paciente informado existe na tabela pacientes
        private bool PacienteExiste(SqlConnection connection, int idPaciente)
        {
            var command = new SqlCommand("SELECT 1 FROM pacientes WHERE id_paciente = @id_paciente", connection);
            command.Parameters.AddWithValue("@id_paciente", idPaciente);
            using var reader = command.ExecuteReader();
            return reader.Read();
        }

        // Verifica se o id_usuario informado existe na tabela usuarios
        private bool UsuarioExiste(SqlConnection connection, int idUsuario)
        {
            var command = new SqlCommand("SELECT 1 FROM usuarios WHERE id_usuario = @id_usuario", connection);
            command.Parameters.AddWithValue("@id_usuario", idUsuario);
            using var reader = command.ExecuteReader();
            return reader.Read();
        }

        // POST: api/Dados_Clinicos
        [HttpPost]
        public IActionResult CriarDados([FromBody] Models.Dados_Clinicos dados_Clinicos)
        {
            // 1) Validação de tamanho/obrigatoriedade
            var erroValidacao = ValidarCampos(dados_Clinicos);
            if (erroValidacao != null)
                return BadRequest(erroValidacao);

            using (var connection = new SqlConnection(connectionStrings))
            {
                connection.Open();

                // 2) Validação de existência das FKs
                if (!PacienteExiste(connection, dados_Clinicos.id_paciente))
                    return BadRequest($"id_paciente {dados_Clinicos.id_paciente} não existe.");

                if (dados_Clinicos.id_usuario_registro.HasValue &&
                    !UsuarioExiste(connection, dados_Clinicos.id_usuario_registro.Value))
                    return BadRequest($"id_usuario_registro {dados_Clinicos.id_usuario_registro} não existe.");

                // 3) Insert
                string sql = @"INSERT INTO dados_clinicos (id_paciente, id_usuario_registro, tipo_sanguineo, alergias, hipertensao, diabetes, outras_doencas_cronicas, observacoes_medicas, data_registro) 
                               VALUES (@id_paciente, @id_usuario_registro, @tipo_sanguineo, @alergias, @hipertensao, @diabetes, @outras_doencas_cronicas, @observacoes_medicas, @data_registro);
                               SELECT CAST(SCOPE_IDENTITY() AS int);";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id_paciente", dados_Clinicos.id_paciente);
                    command.Parameters.AddWithValue("@id_usuario_registro", (object?)dados_Clinicos.id_usuario_registro ?? DBNull.Value);
                    command.Parameters.AddWithValue("@tipo_sanguineo", (object?)dados_Clinicos.tipo_sanguineo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@alergias", (object?)dados_Clinicos.alergias ?? DBNull.Value);
                    command.Parameters.AddWithValue("@hipertensao", dados_Clinicos.hipertensao);
                    command.Parameters.AddWithValue("@diabetes", dados_Clinicos.diabetes);
                    command.Parameters.AddWithValue("@outras_doencas_cronicas", (object?)dados_Clinicos.outras_doencas_cronicas ?? DBNull.Value);
                    command.Parameters.AddWithValue("@observacoes_medicas", (object?)dados_Clinicos.observacoes_medicas ?? DBNull.Value);
                    command.Parameters.AddWithValue("@data_registro", dados_Clinicos.data_registro);

                    var novoId = command.ExecuteScalar();
                    dados_Clinicos.id_dado_clinico = Convert.ToInt32(novoId);
                }
            }

            return StatusCode(201, dados_Clinicos);
        }

        // PUT: api/Dados_Clinicos/5
        [HttpPut("{id}")]
        public IActionResult AtualizarDados(int id, [FromBody] Models.Dados_Clinicos dados_Clinicos)
        {
            // 1) Validação de tamanho/obrigatoriedade
            var erroValidacao = ValidarCampos(dados_Clinicos);
            if (erroValidacao != null)
                return BadRequest(erroValidacao);

            using (var connection = new SqlConnection(connectionStrings))
            {
                connection.Open();

                // 2) Validação de existência das FKs
                if (!PacienteExiste(connection, dados_Clinicos.id_paciente))
                    return BadRequest($"id_paciente {dados_Clinicos.id_paciente} não existe.");

                if (dados_Clinicos.id_usuario_registro.HasValue &&
                    !UsuarioExiste(connection, dados_Clinicos.id_usuario_registro.Value))
                    return BadRequest($"id_usuario_registro {dados_Clinicos.id_usuario_registro} não existe.");

                // 3) Update
                string sql = @"UPDATE dados_clinicos 
                               SET id_paciente = @id_paciente, 
                                   id_usuario_registro = @id_usuario_registro, 
                                   tipo_sanguineo = @tipo_sanguineo, 
                                   alergias = @alergias, 
                                   hipertensao = @hipertensao, 
                                   diabetes = @diabetes, 
                                   outras_doencas_cronicas = @outras_doencas_cronicas, 
                                   observacoes_medicas = @observacoes_medicas, 
                                   data_registro = @data_registro 
                               WHERE id_dado_clinico = @id";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@id_paciente", dados_Clinicos.id_paciente);
                    command.Parameters.AddWithValue("@id_usuario_registro", (object?)dados_Clinicos.id_usuario_registro ?? DBNull.Value);
                    command.Parameters.AddWithValue("@tipo_sanguineo", (object?)dados_Clinicos.tipo_sanguineo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@alergias", (object?)dados_Clinicos.alergias ?? DBNull.Value);
                    command.Parameters.AddWithValue("@hipertensao", dados_Clinicos.hipertensao);
                    command.Parameters.AddWithValue("@diabetes", dados_Clinicos.diabetes);
                    command.Parameters.AddWithValue("@outras_doencas_cronicas", (object?)dados_Clinicos.outras_doencas_cronicas ?? DBNull.Value);
                    command.Parameters.AddWithValue("@observacoes_medicas", (object?)dados_Clinicos.observacoes_medicas ?? DBNull.Value);
                    command.Parameters.AddWithValue("@data_registro", dados_Clinicos.data_registro);

                    int linhasAfetadas = command.ExecuteNonQuery();
                    if (linhasAfetadas == 0) return NotFound();
                }
            }

            return NoContent();
        }

        // DELETE: api/Dados_Clinicos/5
        [HttpDelete("{id}")]
        public IActionResult DeletarDados(int id)
        {
            using (var connection = new SqlConnection(connectionStrings))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM dados_clinicos WHERE id_dado_clinico = @id", connection);
                command.Parameters.AddWithValue("@id", id);

                int linhasAfetadas = command.ExecuteNonQuery();
                if (linhasAfetadas == 0) return NotFound();
            }

            return NoContent();
        }
    }
}