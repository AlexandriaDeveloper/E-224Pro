using Core.Interfaces.Repository;
using Persistence.Specification;
using Shared.DTOs.AccountDtos;
using Shared.DTOs.CollageDtos;

public class CollageService
{
    private readonly ICollageRepository _collageRepository;
    private readonly IUow _uow;
    public CollageService(ICollageRepository collageRepository, IUow uow)
    {
        _collageRepository = collageRepository;
        _uow = uow;
    }
    public async Task<List<CollageDto>?> GetCollages()
    {
        var collages = await _collageRepository.GetAll(null);
        if (!collages.Any())
        {
            return null;
        }
        return collages.Select(x => new CollageDto()
        {
            Id = x.Id,
            CollageName = x.CollageName
        }).ToList();

    }
}