using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Entities
{
    [Table("TB_MESSAGE")] // Vai criar com esse nome no banco
    public class Message : Notifies
    { 
            [Column("MSN_ID")]
            public int Id { get; set; }

            [Column("MSN_TITULO")]
            [MaxLength(255)]
            public string Titulo { get; set; }

            [Column("MSN_ATIVO")]
            public bool Ativo { get; set; }

            [Column("MSN_DATA_CADASTRO")]
            public DateTime DataCadastro { get; set; }

            [Column("MSN_DATA_ALTERACAO")]
            public DateTime DataAlteracao { get; set; }

            [ForeignKey("ApplicationUser")]
            [Column(Order = 1)]
            public string UserId { get; set; } // a tabela de message terá o usuárioId pra saber quem mandou a mesangem

            public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
