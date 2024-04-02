using LanchesMac.Models;

namespace LanchesMac.Services
{
    public interface IPedidoService
    {
        Task<bool> ConfirmarPedidoAsync(Pedido pedido);
    }
}
