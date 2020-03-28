using JpDotFranca.Autenticacao.API.Modelos;
using Microsoft.AspNetCore.Mvc;
using System;

namespace JpDotFranca.Autenticacao.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExemploController : ControllerBase
    {
        [HttpGet("ConsultarHoraAtual")]
        [Autorizador(EnumTipoUsuario.ADMINISTRADOR)]
        public ActionResult<DateTime> ConsultarHoraAtual()
        {
            return DateTime.Now;
        }
    }
}