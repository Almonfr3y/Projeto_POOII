namespace LojaVirtual.Model.Enums;

/// <summary>Situação do pedido. Gravada no banco como texto (ex.: "Finalizado").</summary>
public enum StatusPedido
{
    Aberto,      // pedido sendo montado no checkout
    Finalizado,  // compra concluída e paga
    Cancelado    // reservado para uma futura função de cancelamento
}
