using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.InterfceServices;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIs.Models;

namespace WebAPIs.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMapper _IMapper;

        private readonly IMessage _IMessage;

        private readonly IServiceMessage _IServiceMessage;

        public MessageController(IMapper IMapper, IMessage IMessage, IServiceMessage IServiceMessage)
        {
            _IMapper = IMapper;
            _IMessage = IMessage;
            _IServiceMessage = IServiceMessage;
        }


        //[Authorize]
        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/Add")]
        public async Task<List<Notifies>> Add(MessageViewModel message)
        {
            message.UserId = await RetornarIdUsuarioLogado();
            var messageMap = _IMapper.Map<Message>(message);
            await _IServiceMessage.Adicionar(messageMap);
           /* await _IMessage.Add(messageMap);*/
            return messageMap.Notificacoes;
        }

        //[Authorize]
        [Produces("application/json")]
        [HttpPost("/api/Update")]
        public async Task<List<Notifies>> Update(MessageViewModel message)
        {
            var messageMap = _IMapper.Map<Message>(message);
            await _IServiceMessage.Atualizar(messageMap);
            /*await _IMessage.Update(messageMap);*/
            return messageMap.Notificacoes;
        }

        //[Authorize]
        [Produces("application/json")]
        [HttpPost("/api/Delete")]
        public async Task<List<Notifies>> Delete(MessageViewModel message) // Pega a MessageViewModel do front e passa para a message do BD
        {
            var messageMap = _IMapper.Map<Message>(message);
            await _IMessage.Delete(messageMap);
            return messageMap.Notificacoes;
        }



        //[Authorize]
        [Produces("application/json")]
        [HttpPost("/api/GetEntityById")]
        public async Task<MessageViewModel> GetEntityById(MessageViewModel messageVM) // Aqui faz o inverso pega a message do BD e passa para a MessageViewModel do front 
        {
            var message = await _IMessage.GetEntityById(messageVM.Id);
            var messageMap = _IMapper.Map<MessageViewModel>(message);
            return messageMap;
        }

        //[Authorize]
        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/List")]
        public async Task<List<MessageViewModel>> List()
        {
            var mensagens = await _IMessage.List();
            var messageMap = _IMapper.Map<List<MessageViewModel>>(mensagens);
            return messageMap;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/ListarMessageAtiva")]
        public async Task<List<MessageViewModel>> ListarMessageAtiva()
        {
            var mensagens = await _IServiceMessage.ListarMessageAtiva();
            var messageMap = _IMapper.Map<List<MessageViewModel>>(mensagens);
            return messageMap;
        }

        private async Task<string> RetornarIdUsuarioLogado() // Vou retornar o Id do usuário
        {
            return "6b196742-de2a-4679-8804-7e8c6c549705";

            if (User != null) // Caso seja diferente de nulo
            { 
                var idUsuario = User.FindFirst("idUsuario");
                return idUsuario.Value;
            }

            return string.Empty;

        }
    }
}
