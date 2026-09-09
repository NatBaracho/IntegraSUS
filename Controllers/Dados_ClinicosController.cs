<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace APIestoque.Controllers
=======
﻿using DadosClinicosAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using pacientesAPI.Models;
using System;
using System.Collections.Generic;

namespace DadosClinicosAPI.Controllers
>>>>>>> 0d0bd9f (atualizado)
{
    [Route("api/[controller]")]
    [ApiController]
    public class Dados_ClinicosController : ControllerBase
    {
        private readonly string connectionStrings;
<<<<<<< HEAD
        public Dados_ClinicosController(IConfiguration configuration)
        {
            connectionStrings = configuration.GetConnectionString("Dados_ClinicosConnection");
        }

        // METODOS: GET : API/Dados_Clinicos/1

=======

        public Dados_ClinicosController(IConfiguration configuration)
        {
            connectionStrings = configuration.GetConnectionString("IntegraSUSConnection");
        }

        // METODOS: GET : API/Dados_Clinicos
>>>>>>> 0d0bd9f (atualizado)
        [HttpGet]
        public IActionResult ListarDados()
        {
            var Dados_Clinicos = new List<Models.Dados_Clinicos>();
            using (var connection = new SqlConnection(connectionStrings))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM dados_clinicos", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Dados_Clinicos.Add(new Models.Dados_Clinicos
                        {
                            id_dado_clinico = (int)reader["id_dado_clinico"],
                            id_paciente = (int)reader["id_paciente"],
<<<<<<< HEAD
                            id_usuario_medico = (int)reader["id_usuario_medico"],
                            tipo_sanguineo = (string)reader["tipo_sanguineo"].ToString(),
                            alergias = (string)reader["alergias"].ToString(),
                            hipertensao = (bool)reader["hipertensao"],
                            diabetes = (bool)reader["diabetes"],
                            outras_doencas_cronicas = (string)reader["outras_doencas_cronicas"].ToString(),
                            observacoes_medicas = (string)reader["observacoes_medicas"].ToString(),
                            data_registro = (DateTime)reader["data_regustro"]



                        });
                    }

                }
                
=======
                            // Tratamento de DBNull adicionado abaixo
                            id_usuario_registro = reader["id_usuario_registro"] is DBNull ? 0 : (int)reader["id_usuario_registro"],
                            tipo_sanguineo = reader["tipo_sanguineo"] is DBNull ? null : reader["tipo_sanguineo"].ToString(),
                            alergias = reader["alergias"] is DBNull ? null : reader["alergias"].ToString(),
                            hipertensao = (bool)reader["hipertensao"],
                            diabetes = (bool)reader["diabetes"],
                            outras_doencas_cronicas = reader["outras_doencas_cronicas"] is DBNull ? null : reader["outras_doencas_cronicas"].ToString(),
                            observacoes_medicas = reader["observacoes_medicas"] is DBNull ? null : reader["observacoes_medicas"].ToString(),
                            data_registro = reader["data_registro"] is DBNull ? DateTime.MinValue : (DateTime)reader["data_registro"]
                        });
                    }
                }
>>>>>>> 0d0bd9f (atualizado)
            }
            return Ok(Dados_Clinicos);
        }

        // GET : API/DADOS_CLINICOS/2
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
                        var dadoClinico = new Models.Dados_Clinicos
                        {
<<<<<<< HEAD

                            id_dado_clinico = (int)reader["id_dado_clinico"],
                            id_paciente = (int)reader["id_paciente"],
                            id_usuario_medico = (int)reader["id_usuario_medico"],
                            tipo_sanguineo = (string)reader["tipo_sanguineo"].ToString(),
                            alergias = (string)reader["alergias"].ToString(),
                            hipertensao = (bool)reader["hipertensao"],
                            diabetes = (bool)reader["diabetes"],
                            outras_doencas_cronicas = (string)reader["outras_doencas_cronicas"].ToString(),
                            observacoes_medicas = (string)reader["observacoes_medicas"].ToString(),
                            data_registro = (DateTime)reader["data_regustro"]

                        };
                        return Ok("Dado clinico encontrado com sucesso!");
                    }
                }
            }
            return Ok("Dado clinico não encontrado!");
        }
        // POST : API/DADOS_CLINICOS/1

        [HttpPost]
        public IActionResult CriarDados([FromBody] Models.Dados_Clinicos dados_Clinicos) {
            using (var connection = new SqlConnection(connectionStrings))
        {
            connection.Open();
                var command = new SqlCommand("INSERT INTO dados_clinicos (id_paciente, id_usuario_medico, tipo_sanguineo, alergias, hipertensao, diabetes, outras_doencas_cronicas, observacoes_medicas, data_registro) VALUES (@id_paciente, @id_usuario_medico, @tipo_sanguineo, @alergias, @hipertensao, @diabetes, @outras_doencas_cronicas, @observacoes_medicas, @data_registro)", connection);
                command.Parameters.AddWithValue("@id_paciente", dados_Clinicos.id_paciente);
                command.Parameters.AddWithValue("@id_usuario_medico", dados_Clinicos.id_usuario_medico);
                command.Parameters.AddWithValue("@tipo_sanguineo", dados_Clinicos.tipo_sanguineo);
                command.Parameters.AddWithValue("@alergias", dados_Clinicos.alergias);
                command.Parameters.AddWithValue("@hipertensao", dados_Clinicos.hipertensao);
                command.Parameters.AddWithValue("@diabetes", dados_Clinicos.diabetes);
                command.Parameters.AddWithValue("@outras_doencas_cronicas", dados_Clinicos.outras_doencas_cronicas);
                command.Parameters.AddWithValue("@observacoes_medicas", dados_Clinicos.observacoes_medicas);
                command.Parameters.AddWithValue("@data_registro", dados_Clinicos.data_registro);
                command.ExecuteNonQuery();
            }
            return StatusCode(201, dados_Clinicos);

        }
=======
                            id_dado_clinico = (int)reader["id_dado_clinico"],
                            id_paciente = (int)reader["id_paciente"],
                            // Tratamento de DBNull adicionado abaixo
                            id_usuario_registro = reader["id_usuario_registro"] is DBNull ? 0 : (int)reader["id_usuario_registro"],
                            tipo_sanguineo = reader["tipo_sanguineo"] is DBNull ? null : reader["tipo_sanguineo"].ToString(),
                            alergias = reader["alergias"] is DBNull ? null : reader["alergias"].ToString(),
                            hipertensao = (bool)reader["hipertensao"],
                            diabetes = (bool)reader["diabetes"],
                            outras_doencas_cronicas = reader["outras_doencas_cronicas"] is DBNull ? null : reader["outras_doencas_cronicas"].ToString(),
                            observacoes_medicas = reader["observacoes_medicas"] is DBNull ? null : reader["observacoes_medicas"].ToString(),
                            data_registro = reader["data_registro"] is DBNull ? DateTime.MinValue : (DateTime)reader["data_registro"]
                        };
                        return Ok(dadoClinico);
                    }
                }
            }
            return NotFound("Dado clinico não encontrado!");
        }


        private Models.Dados_Clinicos MapearDadoClinicos(SqlDataReader reader)
        {
            return new Models.Dados_Clinicos
            {
                
                id_paciente = (int)reader["id_paciente"],
                id_usuario_registro = reader["id_usuario_registro"] != DBNull.Value ? (int?)reader["id_usuario_registro"] : null,
                tipo_sanguineo = reader["tipo_sanguineo"] != DBNull.Value ? reader["tipo_sanguineo"].ToString() : null,
                alergias = reader["alergias"] != DBNull.Value ? reader["alergias"].ToString() : null,
                outras_doencas_cronicas = reader["outras_doencas_cronicas"] != DBNull.Value ? reader["outras_doencas_cronicas"].ToString() : null,
                observacoes_medicas = reader["observacoes_medicas"] != DBNull.Value ? reader["observacoes_medicas"].ToString() : null,
                         
                hipertensao = (bool)reader["hipertensao"],
                diabetes = (bool)reader["diabetes"],

                data_registro = reader["data_registro"] != DBNull.Value ? (DateTime)reader["data_registro"] : DateTime.MinValue
            };
        }

        // POST : API/DADOS_CLINICOS
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

                    // Removido o ?? DBNull.Value, pois um tipo 'bool' no C# padrão não aceita null
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
        

>>>>>>> 0d0bd9f (atualizado)
        // PUT : API/DADOS_CLINICOS/1
        [HttpPut("{id}")]
        public IActionResult AtualizarDados(int id, [FromBody] Models.Dados_Clinicos dados_Clinicos)
        {
            using (var connection = new SqlConnection(connectionStrings))
            {
                connection.Open();
<<<<<<< HEAD
                var command = new SqlCommand("UPDATE dados_clinicos SET id_paciente = @id_paciente, id_usuario_medico = @id_usuario_medico, tipo_sanguineo = @tipo_sanguineo, alergias = @alergias, hipertensao = @hipertensao, diabetes = @diabetes, outras_doencas_cronicas = @outras_doencas_cronicas, observacoes_medicas = @observacoes_medicas, data_registro = @data_registro WHERE id_dado_clinico = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@id_paciente", dados_Clinicos.id_paciente);
                command.Parameters.AddWithValue("@id_usuario_medico", dados_Clinicos.id_usuario_medico);
                command.Parameters.AddWithValue("@tipo_sanguineo", dados_Clinicos.tipo_sanguineo);
                command.Parameters.AddWithValue("@alergias", dados_Clinicos.alergias);
                command.Parameters.AddWithValue("@hipertensao", dados_Clinicos.hipertensao);
                command.Parameters.AddWithValue("@diabetes", dados_Clinicos.diabetes);
                command.Parameters.AddWithValue("@outras_doencas_cronicas", dados_Clinicos.outras_doencas_cronicas);
                command.Parameters.AddWithValue("@observacoes_medicas", dados_Clinicos.observacoes_medicas);
                command.Parameters.AddWithValue("@data_registro", dados_Clinicos.data_registro);
=======
                var command = new SqlCommand("UPDATE dados_clinicos SET id_paciente = @id_paciente, id_usuario_registro = @id_usuario_registro, tipo_sanguineo = @tipo_sanguineo, alergias = @alergias, hipertensao = @hipertensao, diabetes = @diabetes, outras_doencas_cronicas = @outras_doencas_cronicas, observacoes_medicas = @observacoes_medicas, data_registro = @data_registro WHERE id_dado_clinico = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@id_paciente", dados_Clinicos.id_paciente);
                // Tratamento seguro caso a classe Models.Dados_Clinicos permita id_usuario_registro nulo
                command.Parameters.AddWithValue("@id_usuario_registro", (object)dados_Clinicos.id_usuario_registro ?? DBNull.Value);

                command.Parameters.AddWithValue("@tipo_sanguineo", (object)dados_Clinicos.tipo_sanguineo ?? DBNull.Value);
                command.Parameters.AddWithValue("@alergias", (object)dados_Clinicos.alergias ?? DBNull.Value);
                command.Parameters.AddWithValue("@hipertensao", dados_Clinicos.hipertensao);
                command.Parameters.AddWithValue("@diabetes", dados_Clinicos.diabetes);
                command.Parameters.AddWithValue("@outras_doencas_cronicas", (object)dados_Clinicos.outras_doencas_cronicas ?? DBNull.Value);
                command.Parameters.AddWithValue("@observacoes_medicas", (object)dados_Clinicos.observacoes_medicas ?? DBNull.Value);
                command.Parameters.AddWithValue("@data_registro", dados_Clinicos.data_registro);

>>>>>>> 0d0bd9f (atualizado)
                int linhasafetadas = command.ExecuteNonQuery();
                if (linhasafetadas == 0) return NotFound();
            }
            return NoContent();
        }
<<<<<<< HEAD
        // DELETE : API/DADOS_CLINICOS/1

        [HttpDelete]
=======

        // DELETE : API/DADOS_CLINICOS/1
        [HttpDelete("{id}")]
>>>>>>> 0d0bd9f (atualizado)
        public IActionResult DeletarDados(int id)
        {
            using (var connection = new SqlConnection(connectionStrings))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM DADOS_CLINICOS WHERE id_dado_clinico = @id", connection);
                command.Parameters.AddWithValue("@id", id);
                int linhasafetadas = command.ExecuteNonQuery();
                if (linhasafetadas == 0) return NotFound();
            }
            return NoContent();
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> 0d0bd9f (atualizado)
