using Core;
using Microsoft.EntityFrameworkCore;

namespace DatabaseSeeder;

public static class Extensions
{
    public static void Put<TKey, TModel>(this DbSet<TModel> models, TKey id, TModel instance) where TModel : class
    {
        
        TModel? existing = models.Find(id);
        if (existing == null)
        {
            models.Add(instance);
        }
    }
}
