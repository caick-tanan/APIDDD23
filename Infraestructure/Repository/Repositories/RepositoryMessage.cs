using Domain.Interfaces;
using Entities.Entities;
using Infraestructure.Configuration;
using Infraestructure.Repository.Generics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository.Repositories
{
    public class RepositoryMessage : RepositoryGenerics<Message>, IMessage
    {
        private readonly DbContextOptions<ContextBase> _OptionBuilder;

        public RepositoryMessage()
        {
            _OptionBuilder = new DbContextOptions<ContextBase>();
        }

        public async Task<List<Message>> ListarMessage(Expression<Func<Message, bool>> exMessage)
        {
            using (var banco = new ContextBase(_OptionBuilder))
            {
                return await banco.Message.Where(exMessage).AsNoTracking().ToListAsync();
            }
        }
    }
}
