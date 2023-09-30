using Application.Mappings;
using Application.ViewModels;
using AutoMapper;
using Domain.Aggregates;

namespace Application.Models
{
    public class MainBeaverDTO : IMapFrom<BoberViewModel>, IMapFrom<MainBeaver>
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BoberViewModel, MainBeaverDTO>();
            profile.CreateMap<MainBeaver, MainBeaverDTO>()
                .ReverseMap();
        }
    }
}
