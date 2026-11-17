namespace LojaVirtual.Model.Enums;

// Situação do pedido. No banco é gravada como texto (ex.: "Finalizado").
public enum StatusPedido
{
    Aberto,
    Finalizado,
    Cancelado   // ainda não usado (não existe cancelamento no sistema)
}
