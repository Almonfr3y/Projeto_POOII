namespace LojaVirtual.Model.Enums;

// Situação do pedido. No banco é gravada como texto (ex.: "Finalizado").
// enum = uma lista fechada de valores possíveis.
public enum StatusPedido
{
    Aberto,       // pedido sendo montado no checkout
    Finalizado,   // compra concluída
    Cancelado   // ainda não usado (não existe cancelamento no sistema)
}
