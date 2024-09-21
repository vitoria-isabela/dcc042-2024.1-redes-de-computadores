namespace LanchesMac.Settings
{
    public static class Config
    {
        /// <summary>
        /// Probabilidade de erro na transmissão (E): 
        /// Representa a chance de que uma mensagem seja corrompida durante a
        /// transmissão devido a interferências ou problemas na rede.
        /// Se sua rede é estável e confiável, você pode definir um 
        /// valor baixo para E. Por exemplo, 0.01 indicaria 
        /// uma probabilidade de erro de 1%.
        /// </summary>
        //public static double E = 0.01;
        public static double E = 0.00;

        /// <summary>
        /// Probabilidade de perda de mensagem (F): Representa a chance de que uma 
        /// mensagem seja perdida durante a transmissão, ou seja, não chegue ao 
        /// seu destino. Isso pode ocorrer devido a congestionamentos na rede, 
        /// falhas de hardware ou outros problemas. Se sua rede é propensa a 
        /// congestionamentos ou instabilidades, você pode definir um valor 
        /// mais alto para F. 
        /// Por exemplo, 0.05 indicaria uma probabilidade de perda de mensagem de 5%.
        /// Esses valores podem variar dependendo das necessidades e das características da rede.
        /// </summary>
        //public static double F = 1; // Probabilidade de perda de mensagem
        public static double F = 0; // Probabilidade de perda de mensagem

        /// <summary>
        /// Número máximo de tentativas de envio do pedido antes de desistir.
        /// </summary>
        public static int MaxTentativas = 5;

        /// <summary>
        /// Intervalo de tempo (em milissegundos) entre as tentativas de retransmissão do pedido.
        /// </summary>
        public static int IntervaloRetransmissao = 5000; // 5 segundos
    }
}
