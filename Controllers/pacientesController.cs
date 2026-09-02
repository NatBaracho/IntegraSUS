using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; // tras os controles 
using Microsoft.Data.SqlClient; // tras as classe 
using pacientesAPI.Models;
using System.Security.Cryptography.X509Certificates;



namespace pacientesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class pacientesController : ControllerBase
    {
        // criando conexao
        private readonly string connectionString;
        public pacientesController(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("IntegraSUSConnection");
        }

        //get: api/pacient
        [HttpGet]
        public IActionResult Listartodos()
        {
            var produtos = new List<Paciente>();
            using (var connection = new SqlConnection(connectionString)) //Ele evita de ficar aberto a minha busca
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM pacientes", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Paciente.Add(new Paciente
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
                        });
                    }
                }
            }

            return Ok(Paciente);
        }
        //Get: api/paciente
        [HttpGet("{id_paciente}")]
        public IActionResult BuscarPorId(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT *FROM pacientes WERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var Paciente = new Paciente()
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

                        return Ok(Paciente);

                    }
                }
            }
            return NotFound();

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
        } 
    }
}