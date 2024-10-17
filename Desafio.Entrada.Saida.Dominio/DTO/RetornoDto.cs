using Microsoft.AspNetCore.Mvc;

namespace Desafio.Entrada.Saida.Dominio.DTO
{
    public class RetornoDto
    {
        public RetornoDto()
        { }
        public RetornoDto(bool houveErro = false,
                          string? tituloErro = "",
                          string? mensagemErro = "",
                          string? codigoErro = "",
                          object? objetoRetorno = null
                        )
        {
            HouveErro = houveErro;
            TituloErro = tituloErro;
            MensagemErro = mensagemErro;
            CodigoErro = codigoErro;
            ObjetoRetorno = objetoRetorno;
        }
        public bool HouveErro { get; set; }
        public string? TituloErro { get; set; }
        public string? MensagemErro { get; set; }
        public string? CodigoErro { get; set; }
        public object? ObjetoRetorno { get; set; }

        public CreatedResult RetornarResultado(string rotaRequisao)
        {
            ProblemDetails detalhesDoProblema = new();
            detalhesDoProblema.Status = int.Parse(CodigoErro);
            detalhesDoProblema.Type = "";
            detalhesDoProblema.Detail = MensagemErro;
            detalhesDoProblema.Title = TituloErro;
            detalhesDoProblema.Instance = rotaRequisao;

            CreatedResult createdResult = new("", null);
            createdResult.StatusCode = int.Parse(CodigoErro);
            createdResult.Value = detalhesDoProblema;

            return createdResult;
        }
    }
}
