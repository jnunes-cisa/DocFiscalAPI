using System;

namespace Entities
{
    /// <summary>
    /// Entidade Base
    /// </summary>
    public class EntityBase
    {  
        /// <summary>
        /// Data de Criação
        /// </summary>
        public DateTime? DataCriacao { get; set; }

        /// <summary>
        /// Data de Alteração
        /// </summary>
        public DateTime? DataAlteracao { get; set; }        
        
    }
}
