using System;
using System.Collections.Generic;
using System.Net.Sockets; // Adicionado para TcpClient
using System.Text; // Adicionado para Encoding
public static class Aggregator
{
    private static Dictionary<string, List<double>> buffer = new();

    public static void ProcessarMensagem(string mensagem)
    {
        var partes = mensagem.Split('|');
        if (partes[0] != "DATA") return;

        string wavyId = partes[1];
        string tipo = partes[2];
        double valor = double.Parse(partes[3]);
        string chave = $"{wavyId}_{tipo}";

        if (!buffer.ContainsKey(chave))
            buffer[chave] = new List<double>();

        buffer[chave].Add(valor);

        var config = ConfigLoader.Ler(tipo, wavyId);
        if (config != null && buffer[chave].Count >= config.Volume)
        {
            double resultado = DataPreProcessor.Processar(config.PreProcessamento, buffer[chave]);
            EnviarParaServidor(wavyId, tipo, config.PreProcessamento, resultado);
            WavyStatusUpdater.Atualizar(wavyId);
            buffer[chave].Clear();
        }
    }

    private static void EnviarParaServidor(string wavyId, string tipo, string proc, double valor)
    {
        try
        {
            using (TcpClient client = new TcpClient("127.0.0.1", 6000))
            using (NetworkStream stream = client.GetStream())
            {
                string mensagem = $"PROCESSED|{wavyId}|{tipo}|{proc}|{valor}";
                byte[] data = Encoding.UTF8.GetBytes(mensagem);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("-> Enviado ao servidor!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao enviar: " + ex.Message);
        }
    }
}
