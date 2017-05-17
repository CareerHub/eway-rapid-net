using AutoMapper;
using eWAY.Rapid.Internals.Mappings;

namespace eWAY.Rapid.Internals.Services {
    internal class MappingService : IMappingService {
        private readonly static MapperConfiguration config;

        static MappingService() {
            config = new MapperConfiguration(c => {
                c.AddProfile<CustomMapProfile>();
                c.AddProfile<EntitiesMapProfile>();
                c.AddProfile<RequestMapProfile>();
                c.AddProfile<ResponseMapProfile>();
            });
        }

        public MappingService() { }

        public TDest Map<TSource, TDest>(TSource obj) {
            return config.CreateMapper().Map<TSource, TDest>(obj);
        }
    }
}
