using System;
using System.Linq;
using AutoMapper;
using Entities;
using Repositories;
using Repositories.Entities;

namespace Services.AutoMapper
{
    class InputMappingProfile : Profile
    {
        private UnityOfWork _unityOfWork { get; set; }

        public UnityOfWork UnityOfWork
        {
            get
            {
                return this._unityOfWork;
            }
        }

        private Action<IMapperConfigurationExpression> Expression { get; set; }

        public InputMappingProfile(UnityOfWork unityOfWork = null)
        {
            _unityOfWork = unityOfWork;
           
        }
    }
}
