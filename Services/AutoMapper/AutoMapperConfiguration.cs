using AutoMapper;

namespace Services.AutoMapper
{
    class AutoMapperConfiguration
    {
        private static IMapper _mapper;

        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(ps =>
            {
                ps.AddProfile(new InputMappingProfile());
                ps.AddProfile(new OutputMappingProfile());
            });
        }

        public static IMapper GetMapper()
        {
            if (_mapper == null)
                Initialize();

#if (!DEBUG)
			_mapper.ConfigurationProvider.CompileMappings();
#endif
            return _mapper;
        }

        public static void Initialize()
        {
            _mapper = _mapper ?? RegisterMappings().CreateMapper();
        }
    }
}
