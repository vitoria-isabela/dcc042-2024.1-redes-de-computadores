using LanchesMac.Models;
using LanchesMac.Settings;

namespace LanchesMac.Services
{
    public class PedidoService : IPedidoService
    {
        public async Task<bool> ConfirmarPedidoAsync(Pedido pedido)
        {
            int tentativas = 0;

            while (tentativas < Config.MaxTentativas)
            {
                // Verifica se houve erro de transmissão
                if (new Random().NextDouble() < Config.E)
                {
                    // Incrementa o número de tentativas
                    tentativas++;
                    // Aguarda o intervalo de retransmissão
                    await Task.Delay(Config.IntervaloRetransmissao);
                }
                else
                {
                    // Simulação de perda de mensagem com probabilidade F
                    if (new Random().NextDouble() < Config.F)
                    {
                        // Incrementa o número de tentativas
                        tentativas++;
                        // Aguarda o intervalo de retransmissão
                        await Task.Delay(Config.IntervaloRetransmissao);
                    }
                    else
                    {
                        // Pedido confirmado com sucesso
                        return true;
                    }
                }
            }

            // Se todas as tentativas falharam, retorna falso
            return false;
        }
    }
}
