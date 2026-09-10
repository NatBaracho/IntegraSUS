using Endereco.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Endereco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class enderecoController : ControllerBase
    {
        private readonly string connectionString;

        public enderecoController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("EnderecoConnection");
        }

        [HttpGet("paciente/{pacienteId}")]
        public IActionResult GetPorPaciente(int pacienteId)
        {
            var enderecos = new List<endereco>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM enderecos WHERE PacienteId = @PacienteId", connection);
                command.Parameters.AddWithValue("@PacienteId", pacienteId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        enderecos.Add(new endereco
                        {
                            Id = (int)reader["Id"],
                            cep = reader["cep"].ToString(),
                            longradouro = reader["longradouro"].ToString(),
                            numero = (int)reader["numero"],
                            complemento = reader["complemento"].ToString(),
                            bairro = reader["bairro"].ToString(),
                            municipio = reader["municipio"].ToString(),
                            uf = reader["uf"].ToString(),
                            PacienteId = (int)reader["PacienteId"]
                        });
                    }
                }
            }
            return Ok(enderecos);
        }

        [HttpGet("{id}")]
        public IActionResult GetPorId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM enderecos WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var endereco = new endereco
                        {
                            Id = (int)reader["Id"],
                            cep = reader["cep"].ToString(),
                            longradouro = reader["longradouro"].ToString(),
                            numero = (int)reader["numero"],
                            complemento = reader["complemento"].ToString(),
                            bairro = reader["bairro"].ToString(),
                            municipio = reader["municipio"].ToString(),
                            uf = reader["uf"].ToString(),
                            PacienteId = (int)reader["PacienteId"]
                        };
                        return Ok(endereco);
                    }
                }
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Criar([FromBody] endereco endereco)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO enderecos (cep, longradouro, numero, complemento, bairro, municipio, uf, PacienteId) " +
                    "VALUES (@cep, @longradouro, @numero, @complemento, @bairro, @municipio, @uf, @PacienteId)", connection);
                command.Parameters.AddWithValue("@cep", endereco.cep);
                command.Parameters.AddWithValue("@longradouro", endereco.longradouro);
                command.Parameters.AddWithValue("@numero", endereco.numero);
                command.Parameters.AddWithValue("@complemento", endereco.complemento);
                command.Parameters.AddWithValue("@bairro", endereco.bairro);
                command.Parameters.AddWithValue("@municipio", endereco.municipio);
                command.Parameters.AddWithValue("@uf", endereco.uf);
                command.Parameters.AddWithValue("@PacienteId", endereco.PacienteId);
                command.ExecuteNonQuery();
            }
            return StatusCode(201, endereco);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, [FromBody] endereco endereco)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE enderecos SET cep=@cep, longradouro=@longradouro, numero=@numero, " +
                    "complemento=@complemento, bairro=@bairro, municipio=@municipio, uf=@uf, PacienteId=@PacienteId " +
                    "WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@cep", endereco.cep);
                command.Parameters.AddWithValue("@longradouro", endereco.longradouro);
                command.Parameters.AddWithValue("@numero", endereco.numero);
                command.Parameters.AddWithValue("@complemento", endereco.complemento);
                command.Parameters.AddWithValue("@bairro", endereco.bairro);
                command.Parameters.AddWithValue("@municipio", endereco.municipio);
                command.Parameters.AddWithValue("@uf", endereco.uf);
                command.Parameters.AddWithValue("@PacienteId", endereco.PacienteId);
                command.Parameters.AddWithValue("@Id", id);

                int linhasAfetadas = command.ExecuteNonQuery();
                if (linhasAfetadas == 0) return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM enderecos WHERE Id=@Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                int linhasAfetadas = command.ExecuteNonQuery();
                if (linhasAfetadas == 0) return NotFound();
            }
            return NoContent();
        }
    }
}