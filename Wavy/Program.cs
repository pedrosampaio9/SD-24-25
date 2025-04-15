using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Wavy
{
    static void Main()
    {
        // Mensagem inicial
        Console.WriteLine("WAVY - Monitoramento Oceânico");

        // Solicita o ID da WAVY ao utilizador
        Console.Write("Digite o ID da WAVY (ex: 01): ");
        string wavyID = Console.ReadLine();

        // Solicita os tipos de dados ativos (ex: acelerometro,giroscopio)
        Console.Write("Tipos de dados ativos (separados por vírgula): ");
        string tiposDeDados = Console.ReadLine();

        try
        {
            // Cria a conexão com o Agregador na porta 6000
            TcpClient cliente = new TcpClient("127.0.0.1", 6000);
            NetworkStream stream = cliente.GetStream();

            // Envia o ID da WAVY ao Agregador
            EnviarMensagem(stream, $"ID:{wavyID}");

            // Informa os tipos de dados ativos
            EnviarMensagem(stream, $"TIPOS:{tiposDeDados}");

            // Inicia envio contínuo de dados simulados
            Console.WriteLine("Enviando dados simulados... Pressione Ctrl+C para encerrar.");

            Random rand = new Random();

            // Loop contínuo de envio
            while (true)
            {
                // Para cada tipo de dado informado
                foreach (string tipo in tiposDeDados.Split(','))
                {
                    // Gera um valor aleatório para o tipo
                    string valorSimulado = rand.NextDouble().ToString("F3");

                    // Formata a mensagem no protocolo
                    string mensagem = $"DADO:{wavyID}:{tipo}:{valorSimulado}";

                    // Envia a mensagem ao Agregador
                    EnviarMensagem(stream, mensagem);

                    // Mostra no terminal a mensagem enviada
                    Console.WriteLine($"Enviado: {mensagem}");

                    // Aguarda 1 segundo entre envios
                    Thread.Sleep(1000);
                }
            }
        }
        catch (Exception ex)
        {
            // Mostra erro de conexão
            Console.WriteLine($"Erro: {ex.Message}");
            Console.WriteLine("Verifique se o Agregador está executando na porta 6000.");
        }
    }

    // Função para envio de mensagens e leitura da resposta do Agregador
    static void EnviarMensagem(NetworkStream stream, string mensagem)
    {
        // Codifica a mensagem em bytes e envia
        byte[] dados = Encoding.UTF8.GetBytes(mensagem);
        stream.Write(dados, 0, dados.Length);

        // Lê a resposta do Agregador
        byte[] buffer = new byte[1024];
        int bytesLidos = stream.Read(buffer, 0, buffer.Length);
        string resposta = Encoding.UTF8.GetString(buffer, 0, bytesLidos);

        // Mostra a resposta recebida
        Console.WriteLine($"Resposta do Agregador: {resposta}");
    }
}
