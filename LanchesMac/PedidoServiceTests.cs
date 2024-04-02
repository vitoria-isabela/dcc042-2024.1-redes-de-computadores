using LanchesMac.Models;
using LanchesMac.Services;
using LanchesMac.Settings;
using NuGet.ContentModel;
using Xunit;

namespace LanchesMac
{
    public class PedidoServiceTests
    {
        [Fact]
        public async Task ConfirmarPedidoAsync_Should_Return_True_When_No_Errors()
        {
            // Arrange
            var pedido = new Pedido();
            Config.E = 0.0; // Definindo probabilidade de erro como zero
            Config.F = 0.0; // Definindo probabilidade de perda de mensagem como zero
            var pedidoService = new PedidoService();

            // Act
            var result = await pedidoService.ConfirmarPedidoAsync(pedido);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ConfirmarPedidoAsync_Should_Return_False_When_Transmission_Error()
        {
            // Arrange
            var pedido = new Pedido();
            Config.E = 1.0; // Probabilidade de erro 100%
            Config.F = 0.0; // Sem probabilidade de perda de mensagem
            var pedidoService = new PedidoService();

            // Act
            var result = await pedidoService.ConfirmarPedidoAsync(pedido);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ConfirmarPedidoAsync_Should_Return_False_When_Message_Loss()
        {
            // Arrange
            var pedido = new Pedido();
            Config.E = 0.0; // Sem probabilidade de erro
            Config.F = 1.0; // Probabilidade de perda de mensagem 100%
            var pedidoService = new PedidoService();

            // Act
            var result = await pedidoService.ConfirmarPedidoAsync(pedido);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ConfirmarPedidoAsync_Should_Return_False_After_Max_Tries()
        {
            // Arrange
            var pedido = new Pedido();
            Config.E = 0.5; // Probabilidade de erro de 50%
            Config.F = 0.5; // Probabilidade de perda de mensagem de 50%
            var pedidoService = new PedidoService();

            // Act
            var result = await pedidoService.ConfirmarPedidoAsync(pedido);

            // Assert
            Assert.False(result);
        }
    }
}
