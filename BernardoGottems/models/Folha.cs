public class Folha
{
    public string Id { get; set;} = Guid.NewGuid().ToString();

    public string Nome {get; set;} = String.Empty;

    public string CPF {get; set;} = String.Empty;

    public int Mes {get; set;}

    public int Ano {get; set;}

    public int HorasTrabalhadas {get; set;}

    public int ValorHora{get; set;}

    public int salarioBruto {get; set;}

    public double impostoRenda {get; set;}

    public double inss {get; set;}

    public int fgts {get; set;}

    public double salarioLiquido {get; set;}
    
}