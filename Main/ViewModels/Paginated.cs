using System.Collections;

namespace Main.ViewModels;

public class Paginated<TModel>
{
    private readonly IQueryable<TModel> _queryable;

    public int Count => _queryable.Count();
    public IList Data => _queryable.ToList();
    
    public Paginated(IQueryable<TModel> queryable,
        int size = 10,
        int page = 1)
    {
        _queryable = queryable.Skip((page-1)*size).Take(size);
    }
}
