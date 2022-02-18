using System.Collections;

namespace Main.ApiModels;

public class PaginatedResponse<TModel>
{
    public IQueryable<TModel> Queryable;

    public IList Data => Queryable.ToList();
    
    public PaginatedResponse(IQueryable<TModel> queryable,
        int size = 10,
        int page = 1)
    {
        Queryable = queryable.Skip((page-1)*size).Take(size);
        Console.WriteLine(Queryable.Count());
    }
}