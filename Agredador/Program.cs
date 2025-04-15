using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Agregador
{
    static void Main()
    {
        TcpListener servidor = new TcpListener(IPAddress.Any, 6000);
        servidor.Start();
        Console.WriteLine("Agregador iniciado na porta 6000...");

        while (true)
        {
            TcpClient wavy = servidor.AcceptTcpClient();
            Thread thread = new Thread(() => GerirWavy(wavy));
            thread.Start();
        }
    }

    static void GerirWavy(TcpClient wavy)
    {
        NetworkStream stream = wavy.GetStream();
        byte[] buffer = new byte[1024];
        int bytesLidos = stream.Read(buffer, 0, buffer.Length);
        string mensagemRecebida = Encoding.UTF8.GetString(buffer, 0, bytesLidos);

        Console.WriteLine($"Recebido da WAVY: {mensagemRecebida}");

        EnviarParaServidor(mensagemRecebida);
        wavy.Close();
    }

    static void EnviarParaServidor(string mensagem)
    {
        TcpClient servidor = new TcpClient("127.0.0.1", 5000);
        NetworkStream stream = servidor.GetStream();

        byte[] dados = Encoding.UTF8.GetBytes(mensagem);
        stream.Write(dados, 0, dados.Length);

        servidor.Close();
    }
}
