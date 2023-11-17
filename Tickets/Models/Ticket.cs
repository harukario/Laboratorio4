using System.ComponentModel;

namespace Regularidad.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [DisplayName("Afiliado")]
        public int AfiliadoId { get; set; }

        [DisplayName("Fecha de Solicitud")]
        public DateTime FechaSolicitud { get; set; }
        [DisplayName("Observacion")]
        public string Observacion { get; set; }

        public Afiliado? afiliado { get; set; }
        public List<TicketDetalle>? TicketDetalles { get; set; }

        public string NombreTicket => $" {"Ticket nro: " + Id}{FechaSolicitud} ";

    }
}
