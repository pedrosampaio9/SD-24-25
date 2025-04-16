using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Wavy
{
    static void Main()
    {
        Console.WriteLine("WAVY - Monitoramento Oceânico");

        Console.Write("Digite o ID da WAVY (ex: WAVY001): ");
        string wavyID = Console.ReadLine();

        Console.Write("Tipos de dados ativos (separados por vírgula): ");
        string tiposDeDados = Console.ReadLine();

        try
        {
            // Conecta ao Agregador na porta 5000 (ajustado do 6000)
            TcpClient cliente = new TcpClient("127.0.0.1", 5000);
            NetworkStream stream = cliente.GetStream();

            // Mostra mensagem de status
            Console.WriteLine("Enviando dados simulados... Pressione Ctrl+C para encerrar.");

            Random rand = new Random();

            while (true)
            {
                foreach (string tipo in tiposDeDados.Split(','))
                {
                    string tipoTrimado = tipo.Trim();
                    string valorSimulado = rand.NextDouble().ToString("F3");

                    // Aqui está o novo protocolo DATA|ID|TIPO|VALOR
                    string mensagem = $"DATA|{wavyID}|{tipoTrimado}|{valorSimulado}";

                    EnviarMensagem(stream, mensagem);

                    Console.WriteLine($"Enviado: {mensagem}");

                    Thread.Sleep(1000); // Espera 1 segundo
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
            Console.WriteLine("Verifique se o Agregador está executando na porta 5000.");
        }
    }

    static void EnviarMensagem(NetworkStream stream, string mensagem)
    {
        byte[] dados = Encoding.UTF8.GetBytes(mensagem);
        stream.Write(dados, 0, dados.Length);

        // Opcional: ler resposta do Agregador
        byte[] buffer = new byte[1024];
        int bytesLidos = stream.Read(buffer, 0, buffer.Length);
        string resposta = Encoding.UTF8.GetString(buffer, 0, bytesLidos);

        Console.WriteLine($"Resposta do Agregador: {resposta}");
    }
}
