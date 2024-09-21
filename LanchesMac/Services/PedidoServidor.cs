using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class PedidoServidor
{
    private const int Port = 8080;

    public static async Task Main()
    {
        var listener = new TcpListener(IPAddress.Any, Port);
        listener.Start();
        Console.WriteLine("Servidor aguardando conexões...");

        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = ProcessClient(client);
        }
    }

    private static async Task ProcessClient(TcpClient client)
    {
        Console.WriteLine("Cliente conectado.");

        using (var networkStream = client.GetStream())
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
            string pedidoJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine($"Pedido recebido: {pedidoJson}");

            // Aqui você pode desserializar o JSON e processar o pedido
            // Para simplificação, vamos apenas confirmar o pedido
            string confirmacao = "Pedido confirmado";
            byte[] confirmacaoBytes = Encoding.UTF8.GetBytes(confirmacao);
            await networkStream.WriteAsync(confirmacaoBytes, 0, confirmacaoBytes.Length);
        }

        client.Close();
        Console.WriteLine("Cliente desconectado.");
    }
}