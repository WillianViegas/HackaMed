using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Domain.Entities
{
    public class Agenda
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string MedicoId { get; set; }
        public DateOnly? DataAgendamento { get; set; }
        public string? HorarioAgendamento { get; set; }
        public decimal? Valor { get; set; }
        public string Status { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
