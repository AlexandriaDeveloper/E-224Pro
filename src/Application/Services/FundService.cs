
using System.Threading.Tasks;
using Core.Interfaces.Repository;
using Persistence.Specification;

public class FundService
{
    private readonly IFundRepository _fundRepository;
    private readonly IUow _uow;

    public FundService(IFundRepository fundRepository, IUow uow)
    {
        this._fundRepository = fundRepository;
        this._uow = uow;
    }

    public async Task<FundDto> CreateFund(PostFundRequest request, CancellationToken cancellationToken = default)
    {
        var fund = new Core.Models.Fund
        {
            FundName = request.FundName,
            FundCode = request.FundCode,
            CollageId = request.CollageId
        };

        await _fundRepository.AddAsync(fund);
        await _uow.CommitAsync(cancellationToken);

        return new FundDto(fund);
    }

    public async Task DeleteFund(int id, CancellationToken cancellationToken)
    {
        var fund = await _fundRepository.GetById(id);
        if (fund == null)
        {
            throw new KeyNotFoundException($"Fund with ID {id} not found.");
        }

        _fundRepository.Delete(fund);
        await _uow.CommitAsync(cancellationToken);
    }

    public async Task<List<FundDto>> GetFundsWithSpecs(GetFundRequest request)
    {

        var fundSpec = new GetFundsSpecifiction(request);

        var fund = await _fundRepository.GetAll(fundSpec);
        return fund.Select(x => new FundDto(x)).ToList();
    }

    public async Task<object> UpdateFund(int id, PutFundRequest request, CancellationToken cancellationToken = default)
    {
        var fund = await _fundRepository.GetById(id);
        if (fund == null)
        {
            throw new KeyNotFoundException($"Fund with ID {id} not found.");
        }

        fund.FundName = request.FundName ?? fund.FundName;
        fund.FundCode = request.FundCode ?? fund.FundCode;
        fund.CollageId = request.CollageId;

        await _fundRepository.UpdateAsync(fund);
        await _uow.CommitAsync(cancellationToken);

        return new FundDto(fund);
    }
}