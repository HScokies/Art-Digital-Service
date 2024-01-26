using Contracts;
using Contracts.Cases;
using Data.Interfaces;
using Domain.Core.Primitives;
using Domain.Entities;
using Domain.Enumeration;
using Domain.Repositories;

namespace Application.Services.Case
{
    public sealed class CaseService : ICaseService
    {
        private readonly ICaseRepository caseRepository;
        private readonly IRepository repository;

        public CaseService(ICaseRepository caseRepository, IRepository repository)
        {
            this.caseRepository = caseRepository;
            this.repository = repository;
        }
        public async Task<int> CreateAsync(UpsertCaseRequest request,CancellationToken cancellationToken)
        {
            CaseDto entity = request.toCase();
            var res = await caseRepository.CreateAsync(entity, cancellationToken);
            return res.id;
        }

        public async Task<Result<bool>> DropAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await caseRepository.GetAsync(id, cancellationToken);
            if (entity is null)
                return new Result<bool>(CommonErrors.Case.NotFound);
            await caseRepository.DropAsync(entity, cancellationToken);
            return new Result<bool>();
        }

        public IEnumerable<CasePreviewResponse> Get(string? search) => caseRepository.Get(search?.ToLower());

        public async Task<Result<CaseDto>> GetAsync(int id, CancellationToken cancellationToken)
        {
            var res = await caseRepository.GetAsync(id, cancellationToken);
            if (res is null)
                return new Result<CaseDto>(CommonErrors.Case.NotFound);
            return new Result<CaseDto>(res);
        }

        public async Task<Result<bool>> UpdateAsync(int id, UpsertCaseRequest request, CancellationToken cancellationToken)
        {
            var res = await caseRepository.GetAsync(id, cancellationToken);
            if (res is null)
                return new Result<bool>(CommonErrors.Case.NotFound);
            res.updateCase(request);
            await repository.SaveChangesAsync(cancellationToken);
            return new Result<bool>();
        }


    }
}
