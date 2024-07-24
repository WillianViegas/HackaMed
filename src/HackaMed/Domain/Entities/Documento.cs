using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Domain.Entities
{
    public class Documento
    {
        public string PacienteId { get; set; }
        public string Titulo { get; set; }
        public string Decricao { get; set; }
        public string? Url { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
