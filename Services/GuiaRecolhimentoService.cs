using AutoMapper;
using Common.Extensions;
using Entities;
using Repositories;
using Services.Interfaces;
using System.Collections.Generic;

namespace Services
{
    public class GuiaRecolhimentoService : BaseServices<GuiaRecolhimentoEntity>, IGuiaRecolhimentoService
    {
        public GuiaRecolhimentoService(BaseContext baseContext, Dictionary<string, string> query)
        {
            QuerySet = ApplyQuery(query);
            UnityOfWork = new UnityOfWork(baseContext);
        }

        public override GuiaRecolhimentoEntity Create(GuiaRecolhimentoEntity Entity)
        {
            throw new System.NotImplementedException();
        }

        public override GuiaRecolhimentoEntity Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public override List<GuiaRecolhimentoEntity> GetAll()
        {
            throw new System.NotImplementedException();
        }    

        public override GuiaRecolhimentoEntity Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public override GuiaRecolhimentoEntity Update(GuiaRecolhimentoEntity Entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
