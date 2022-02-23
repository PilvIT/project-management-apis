using System.Collections;

namespace Main.ApiModels;

public class PaginatedResponse<TModel>
{
    private readonly IQueryable<TModel> _queryable;

    public int Count => _queryable.Count();
    public IList Data => _queryable.ToList();
    
    public PaginatedResponse(IQueryable<TModel> queryable,
        int size = 10,
        int page = 1)
    {
        _queryable = queryable.Skip((page-1)*size).Take(size);
    }
}
