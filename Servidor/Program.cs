using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Servidor
{
    static void Main()
    {
        TcpListener servidor = new TcpListener(IPAddress.Any, 5000);
        servidor.Start();
        Console.WriteLine("Servidor iniciado na porta 5000...");

        while (true)
        {
            TcpClient cliente = servidor.AcceptTcpClient();
            Thread thread = new Thread(() => GerirCliente(cliente));
            thread.Start();
        }
    }

    static void GerirCliente(TcpClient cliente)
    {
        NetworkStream stream = cliente.GetStream();
        byte[] buffer = new byte[1024];
        int bytesLidos = stream.Read(buffer, 0, buffer.Length);
        string mensagemRecebida = Encoding.UTF8.GetString(buffer, 0, bytesLidos);

        Console.WriteLine($"Mensagem Recebida: {mensagemRecebida}");

        string resposta = "Mensagem recebida pelo Servidor";
        byte[] respostaBytes = Encoding.UTF8.GetBytes(resposta);
        stream.Write(respostaBytes, 0, respostaBytes.Length);

        cliente.Close();
    }
}
