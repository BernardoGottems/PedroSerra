using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();
var app = builder.Build();

var folha = new List<Folha>();

app.MapPost("/api/folha/cadastrar", (
    [FromBody] Folha? folha,
    [FromServices] AppDataContext ctx) =>{
        
    if(folha.Mes > 12 || folha.Mes <= 0){
        return Results.BadRequest("Mês inválido");
    }
    if(folha.Ano < 2000){
        return Results.BadRequest("Ano inválido");
    }
    foreach(Folha folhaCadastrada in ctx.Folhas.ToList()){
        if(folha.CPF == folhaCadastrada.CPF && folha.Mes == folhaCadastrada.Mes && folha.Ano == folhaCadastrada.Ano){
            return Results.BadRequest("Esse cadastro já foi feito");
        }
        
    }
    folha.salarioBruto = folha.HorasTrabalhadas * folha.ValorHora;

    ctx.Folhas.Add(folha);
    ctx.SaveChanges();
    return Results.Created("", folha);

    
    });

app.MapGet("/api/folha/listar", (
    [FromServices] AppDataContext ctx) =>
{
    if(ctx.Folhas.Count() == 0){
        return Results.BadRequest("Nenhuma folha cadastrada");

    }
    return Results.Ok(ctx.Folhas.ToList());
});

app.MapGet("/api/folha/buscar/{CPF}/{Mes}/{Ano}", (
    [FromRoute] string cpf, [FromRoute] int mes, [FromRoute] int ano,
    [FromServices] AppDataContext ctx) =>
{
    foreach(Folha folhaCadastrada in ctx.Folhas.ToList()){
        if(cpf == folhaCadastrada.CPF && mes== folhaCadastrada.Mes && ano == folhaCadastrada.Ano){
            return Results.Ok(folhaCadastrada);
        }
    }

    return Results.NotFound("Folha não encontrada!");
});

app.MapDelete("/api/folha/remover/{CPF}/{Mes}/{Ano}", (
    [FromRoute] string cpf, [FromRoute] int mes, [FromRoute] int ano,
    [FromServices] AppDataContext ctx) =>
{
    foreach(Folha folhaCadastrada in ctx.Folhas.ToList()){
        if(cpf == folhaCadastrada.CPF && mes== folhaCadastrada.Mes && ano == folhaCadastrada.Ano){
            ctx.Folhas.Remove(folhaCadastrada);
            ctx.SaveChanges();
            return Results.Ok(folhaCadastrada);
        }
    }

    return Results.NotFound("Folha não encontrada!");
});

/*app.MapGet("api/folha/total-liquido", (
    [FromRoute] string cpf, [FromRoute] int mes, [FromRoute] int ano,
    [FromServices] AppDataContext ctx) =>
{
    double soma = 0;
    foreach(Folha folhaCadastrada in ctx.Folhas.ToList()){
        soma += folhaCadastrada.salarioLiquido;
    }

    return Results.NotFound("Folha não encontrada!");
});*/

app.Run();