using System;

namespace pacientesAPI.Models
{
    public class Paciente
    {
        public int id_paciente { get; set; }

        // Novos campos adicionados para refletir o banco de dados atualizado
        public int? id_responsavel { get; set; }
        public int? id_usuario_cadastro { get; set; }

        public string nome_completo { get; set; } = string.Empty;
        public DateTime data_nascimento { get; set; }

        // Lembre-se: No banco definimos como CHAR(1) com CHECK ('M', 'F', 'O')
        public string sexo { get; set; } = string.Empty;

        public string nome_mae { get; set; } = string.Empty;
        public string cns { get; set; } = string.Empty;

        public string? cpf { get; set; }
        public string? rg { get; set; }
        public string? numero_prontuario { get; set; }
        public string? telefone { get; set; }
        public string? telefone_emergencia { get; set; }
        public string? email { get; set; }
        public DateTime? data_cadastro { get; set; }
    }
}