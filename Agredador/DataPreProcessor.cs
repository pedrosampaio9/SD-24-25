using System.Collections.Generic;
using System.Linq;

public static class DataPreProcessor
{
    public static double Processar(string tipo, List<double> valores)
    {
        return tipo switch
        {
            "media" => valores.Average(),
            _ => 0
        };
    }
}
