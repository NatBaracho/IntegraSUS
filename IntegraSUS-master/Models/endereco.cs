namespace Endereco.Models
{
    public class endereco
    {
        public int Id { get; set; }
        public string cep { get; set; } 
        public string longradouro { get; set; }
        public int numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string municipio { get; set; }
        public string uf { get; set; }

        public int PacienteId { get; set; }
    }
}