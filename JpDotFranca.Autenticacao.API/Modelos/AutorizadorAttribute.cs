using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace JpDotFranca.Autenticacao.API.Modelos
{
    /// <summary>
    /// Atributo usado para decorar os serviços REST que
    /// necessitam de uma validação antes de sua execução.
    /// </summary>
    public class AutorizadorAttribute : TypeFilterAttribute
    {
        public AutorizadorAttribute(EnumTipoUsuario tipoUsuario)
            : base(typeof(FiltroAutorizador))
        {
            Arguments = new object[] { tipoUsuario };
        }
    }

    /// <summary>
    /// Classe será o processador das autorizações. É aqui onde
    /// colocamos a lógica para conferir a configuração que 
    /// decoramos nosso serviço com os dados que vem da requisição.
    /// </summary>
    public class FiltroAutorizador : IAuthorizationFilter
    {
        private EnumTipoUsuario _tipoUsuarioAutorizadoConsumirServico;
        public FiltroAutorizador(EnumTipoUsuario tipoUsuarioAutorizadoConsumirServico)
        {
            _tipoUsuarioAutorizadoConsumirServico = tipoUsuarioAutorizadoConsumirServico;
        }

        /// <summary>
        /// Esse método sempre será invocado quando for feita
        /// uma chamada para os serviços decorados com o atributo
        /// <see cref="AutorizadorAttribute"/>.
        /// </summary>
        public void OnAuthorization(AuthorizationFilterContext contexto)
        {
            contexto.HttpContext.Request.Headers.TryGetValue("TipoUsuario", out StringValues argumentoTipoUsuario);
            int.TryParse(argumentoTipoUsuario, out int codigoTipoTipoUsuario);

            bool usuarioPossuiPermissaoServico = ValidarTipoUsuario(codigoTipoTipoUsuario);

            if (usuarioPossuiPermissaoServico == false)
            {
                string mensagemErro = $"O tipo de usuário não tem permissão para consumir o serviço.";
              
                // Atribui o método Http adequado para quando não autorizado.
                contexto.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
               
                // Escreve a mensagem de erro na resposta que o cliente visualizará.
                contexto.HttpContext.Response.WriteAsync(mensagemErro);

                // Impede a continuação do serviço.
                contexto.Result = new UnauthorizedResult {};
            }
        }

        private bool ValidarTipoUsuario(int codigoTipoUsuario)
        {
            return (int)_tipoUsuarioAutorizadoConsumirServico == codigoTipoUsuario;
        }
    }
}
