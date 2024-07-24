using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Domain.Entities
{
    public class Consulta
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string PacienteId { get; set; }
        public string MedicoId { get; set; }
        public DateOnly DataConsulta { get; set; }
        public string HorarioConsulta { get; set; }
        public decimal Valor { get; set; }
        public string Link { get; set; }
        public string Status { get; set; }
        public string DescricaoCancelamento { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
