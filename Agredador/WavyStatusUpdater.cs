using System;
using System.IO;

public static class WavyStatusUpdater
{
    public static void Atualizar(string wavyId)
    {
        string[] linhas = File.ReadAllLines("Files/status.csv");
        for (int i = 0; i < linhas.Length; i++)
        {
            if (linhas[i].StartsWith(wavyId + ":"))
            {
                var partes = linhas[i].Split(':');
                partes[3] = DateTime.Now.ToString("s");
                linhas[i] = string.Join(":", partes);
            }
        }
        File.WriteAllLines("Files/status.csv", linhas);
    }
}
