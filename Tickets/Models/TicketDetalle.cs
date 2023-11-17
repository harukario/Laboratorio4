using System.ComponentModel;

namespace Regularidad.Models
{
    public class TicketDetalle
    {
        public int Id { get; set; }

        [DisplayName("Ticket asociado")]
        public int TicketId { get; set; }

        [DisplayName("Descripcion Pedido")]
        public string DescripcionPedido { get; set; }

        [DisplayName("Estado")]
        public int EstadoId { get; set; }
        [DisplayName("Fecha de Estado")]
        public DateTime FechaEstado { get; set; }

        public Ticket? ticket {  get; set; }
        public Estado? estado { get; set; }
    }
}
