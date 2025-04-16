using System.IO;

public class Config
{
    public string WavyId;
    public string PreProcessamento;
    public int Volume;
    public string Servidor;
}

public static class ConfigLoader
{
    public static Config Ler(string tipo, string wavyId)
    {
        string[] linhas = File.ReadAllLines($"Files/{tipo}.csv");
        foreach (var linha in linhas)
        {
            var partes = linha.Split(':');
            if (partes[0] == wavyId)
            {
                return new Config
                {
                    WavyId = partes[0],
                    PreProcessamento = partes[1],
                    Volume = int.Parse(partes[2]),
                    Servidor = partes[3]
                };
            }
        }
        return null;
    }
}
