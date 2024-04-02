using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using LanchesMac.Services;
using LanchesMac.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IPedidoService _pedidoService;
        private readonly CarrinhoCompra _carrinhoCompra;

        public PedidoController(IPedidoRepository pedidoRepository,
            IPedidoService pedidoService,
            CarrinhoCompra carrinhoCompra)
        {
            _pedidoRepository = pedidoRepository;
            _pedidoService = pedidoService;
            _carrinhoCompra = carrinhoCompra;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CheckoutAsync(Pedido pedido)
        {
            int totalItensPedido = 0;
            decimal precoTotalPedido = 0.0m;

            // Enviar o pedido e aguardar confirmação
            bool pedidoConfirmado = await EnviarPedidoComConfirmacaoAsync(pedido);

            if (!pedidoConfirmado)
            {
                // Retransmitir o pedido se não houver confirmação
                return StatusCode(500, "Erro no pedido. Pedido está sendo retransmitido.");
            }

            // Simulação de perda de mensagem com probabilidade F
            if (new Random().NextDouble() < Config.F)
            {
                await Task.Delay(5000); // Simular atraso na resposta do servidor
                // Retransmitir o pedido
                return StatusCode(500, "Pedido perdido. Pedido está sendo retransmitido.");
            }

            //obtem os itens do carrinho de compra do cliente
            List<CarrinhoCompraItem> items = _carrinhoCompra.GetCarrinhoCompraItens();
            _carrinhoCompra.CarrinhoCompraItems = items;

            //verifica se existem itens de pedido
            if (_carrinhoCompra.CarrinhoCompraItems.Count == 0)
            {
                ModelState.AddModelError("", "Seu carrinho esta vazio, que tal incluir um lanche...");
            }

            //calcula o total de itens e o total do pedido
            foreach (var item in items)
            {
                totalItensPedido += item.Quantidade;
                precoTotalPedido += (item.Lanche.Preco * item.Quantidade);
            }

            //atribui os valores obtidos ao pedido
            pedido.TotalItensPedido = totalItensPedido;
            pedido.PedidoTotal = precoTotalPedido;

            //valida os dados do pedido
            if (ModelState.IsValid)
            {
                //cria o pedido e os detalhes
                _pedidoRepository.CriarPedido(pedido);

                //define mensagens ao cliente
                ViewBag.CheckoutCompletoMensagem = "Obrigado pelo seu pedido :)";
                ViewBag.TotalPedido = _carrinhoCompra.GetCarrinhoCompraTotal();

                //limpa o carrinho do cliente
                _carrinhoCompra.LimparCarrinho();

                //exibe a view com dados do cliente e do pedido
                return View("~/Views/Pedido/CheckoutCompleto.cshtml", pedido);
            }
            return View(pedido);
        }

        private async Task<bool> EnviarPedidoComConfirmacaoAsync(Pedido pedido)
        {
            bool pedidoConfirmado = false;
            int tentativas = 0;

            while (!pedidoConfirmado && tentativas < Config.MaxTentativas)
            {
                // Enviar o pedido e aguardar confirmação
                pedidoConfirmado = await _pedidoService.ConfirmarPedidoAsync(pedido);

                if (!pedidoConfirmado)
                {
                    // Aguardar um tempo antes de retransmitir
                    await Task.Delay(Config.IntervaloRetransmissao);
                }

                tentativas++;
            }

            return pedidoConfirmado;
        }
    }
}
