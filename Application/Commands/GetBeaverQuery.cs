using Application.Models;
using Application.Repositiories;
using Application.ViewModels;
using AutoMapper;
using Domain.Aggregates;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Commands
{
    public class GetBeaverQuery : IRequest<BoberViewModel>
    { }

    public class GetBeaverQueryHandler : IRequestHandler<GetBeaverQuery, BoberViewModel>
    {
        private readonly IGenericRepository<MainBeaver> _repository;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        public GetBeaverQueryHandler(IGenericRepository<MainBeaver> repository, IMapper mapper, IServiceProvider serviceProvider)
        {
            _repository = repository;
            _mapper = mapper;
            _serviceProvider = serviceProvider;
        }

        public async Task<BoberViewModel> Handle(GetBeaverQuery request, CancellationToken cancellationToken)
        {
            MainBeaver? beaver = await _repository.GetAll().FirstOrDefaultAsync(cancellationToken);

            if (beaver is not null) 
            {
                var data = _mapper.Map<MainBeaverDTO>(beaver);
                var vm = _mapper.Map<BoberViewModel>(data);
                return vm;
            }
            else
            {
                var vm = _serviceProvider.GetRequiredService<BoberViewModel>();
                return vm;
            }
        }
    }
}
