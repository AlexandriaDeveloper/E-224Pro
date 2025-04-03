
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

    public async Task<List<FundDto>> GetFundsWithSpecs(GetFundRequest request)
    {

        var fundSpec = new GetFundsSpecifiction(request);

        var fund = await _fundRepository.GetAll(fundSpec);
        return fund.Select(x => new FundDto(x)).ToList();
    }
}