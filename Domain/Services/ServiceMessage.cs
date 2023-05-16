using Domain.Interfaces;
using Domain.Interfaces.InterfceServices;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class ServiceMessage : IServiceMessage
    {
        private readonly IMessage _IMessage;

        public ServiceMessage(IMessage IMessage)
        {
            _IMessage = IMessage;
        }

        public async Task Adicionar(Message Objeto)
        {
            var validarTitulo = Objeto.ValidarPropriedadeString(Objeto.Titulo, "Titulo");
            if (validarTitulo) 
            {
                Objeto.DataCadastro = DateTime.Now;
                Objeto.DataAlteracao = DateTime.Now;
                Objeto.Ativo = true;
                await _IMessage.Add(Objeto);
             }
        }

        public async Task Atualizar(Message Objeto)
        {
            var validarTitulo = Objeto.ValidarPropriedadeString(Objeto.Titulo, "Titulo");
            if (validarTitulo)
            {
                Objeto.DataAlteracao = DateTime.Now;
                await _IMessage.Update(Objeto);
            }
        }

        public async Task<List<Message>> ListarMessageAtiva()
        {
            return await _IMessage.ListarMessage(x => x.Ativo);
        }
    }
}
