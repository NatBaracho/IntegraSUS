using System;

namespace DadosClinicosAPI.Models
{
    public class Dados_Clinicos
    {
        public int id_dado_clinico { get; set; }
        public int id_paciente { get; set; }
        public int? id_usuario_registro { get; set; }
        public string? tipo_sanguineo { get; set; }
        public string? alergias { get; set; }
        public bool hipertensao { get; set; }
        public bool diabetes { get; set; }
        public string? outras_doencas_cronicas { get; set; }
        public string? observacoes_medicas { get; set; }
        public DateTime data_registro { get; set; } = DateTime.Now;
    }
}