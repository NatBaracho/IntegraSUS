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
            connectionStrings = configuration.GetConnectionString("IntegraSUSConnection");
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
                hipertensao = (bool)reader["hipertensao"],
                diabetes = (bool)reader["diabetes"],
                outras_doencas_cronicas = reader["outras_doencas_cronicas"] != DBNull.Value ? reader["outras_doencas_cronicas"].ToString() : null,
                observacoes_medicas = reader["observacoes_medicas"] != DBNull.Value ? reader["observacoes_medicas"].ToString() : null,
                data_registro = reader["data_registro"] != DBNull.Value ? (DateTime)reader["data_registro"] : DateTime.MinValue
            };
        }

        // POST: api/Dados_Clinicos
        [HttpPost]
        public IActionResult CriarDados([FromBody] Models.Dados_Clinicos dados_Clinicos)
        {
            using (var connection = new SqlConnection(connectionStrings))
            {
                connection.Open();

                string sql = @"INSERT INTO dados_clinicos (id_paciente, id_usuario_registro, tipo_sanguineo, alergias, hipertensao, diabetes, outras_doencas_cronicas, observacoes_medicas, data_registro) 
                               VALUES (@id_paciente, @id_usuario_registro, @tipo_sanguineo, @alergias, @hipertensao, @diabetes, @outras_doencas_cronicas, @observacoes_medicas, @data_registro)";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id_paciente", dados_Clinicos.id_paciente);
                    command.Parameters.AddWithValue("@id_usuario_registro", (object)dados_Clinicos.id_usuario_registro ?? DBNull.Value);
                    command.Parameters.AddWithValue("@tipo_sanguineo", (object)dados_Clinicos.tipo_sanguineo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@alergias", (object)dados_Clinicos.alergias ?? DBNull.Value);
                    command.Parameters.AddWithValue("@hipertensao", dados_Clinicos.hipertensao);
                    command.Parameters.AddWithValue("@diabetes", dados_Clinicos.diabetes);
                    command.Parameters.AddWithValue("@outras_doencas_cronicas", (object)dados_Clinicos.outras_doencas_cronicas ?? DBNull.Value);
                    command.Parameters.AddWithValue("@observacoes_medicas", (object)dados_Clinicos.observacoes_medicas ?? DBNull.Value);
                    command.Parameters.AddWithValue("@data_registro", dados_Clinicos.data_registro);

                    command.ExecuteNonQuery();
                }
            }

            return StatusCode(201, dados_Clinicos);
        }

        // PUT: api/Dados_Clinicos/5
        [HttpPut("{id}")]
        public IActionResult AtualizarDados(int id, [FromBody] Models.Dados_Clinicos dados_Clinicos)
        {
            using (var connection = new SqlConnection(connectionStrings))
            {
                connection.Open();

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
                    command.Parameters.AddWithValue("@id_usuario_registro", (object)dados_Clinicos.id_usuario_registro ?? DBNull.Value);
                    command.Parameters.AddWithValue("@tipo_sanguineo", (object)dados_Clinicos.tipo_sanguineo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@alergias", (object)dados_Clinicos.alergias ?? DBNull.Value);
                    command.Parameters.AddWithValue("@hipertensao", dados_Clinicos.hipertensao);
                    command.Parameters.AddWithValue("@diabetes", dados_Clinicos.diabetes);
                    command.Parameters.AddWithValue("@outras_doencas_cronicas", (object)dados_Clinicos.outras_doencas_cronicas ?? DBNull.Value);
                    command.Parameters.AddWithValue("@observacoes_medicas", (object)dados_Clinicos.observacoes_medicas ?? DBNull.Value);
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