using Application.Models;
using Application.Repositiories;
using Application.ViewModels;
using AutoMapper;
using Domain.Aggregates;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands
{
    public class SaveMainBeaverCommand : IRequest<int>
    {
        public BoberViewModel BoberViewModel { get; set; }

        public SaveMainBeaverCommand(BoberViewModel boberViewModel)
        {
            BoberViewModel = boberViewModel;
        }
    }

    public class SaveMainBeaverCommandHandler : IRequestHandler<SaveMainBeaverCommand, int>
    {
        private readonly IMainRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SaveMainBeaverCommandHandler> _logger;

        public SaveMainBeaverCommandHandler(IMainRepository mainRepository, IMapper mapper, ILogger<SaveMainBeaverCommandHandler> logger)
        {
            _repository = mainRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(SaveMainBeaverCommand request, CancellationToken cancellationToken)
        {
            var beaverDto = _mapper.Map<MainBeaverDTO>(request.BoberViewModel);
            var beaver = _mapper.Map<MainBeaver>(beaverDto);

            if (beaver.Id == default)
            {
                // Create
                await _repository.Create(beaver);
            }
            else
            {
                // Update
                await _repository.Update(beaver);

                _logger.LogInformation("Updated Bober");
            }

            return beaver.Id;
        }
    }
}
