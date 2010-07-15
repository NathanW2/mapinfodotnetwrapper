using MapInfo.Wrapper.DataAccess;
using MapInfo.Wrapper.DataAccess.Entities;

namespace MapInfo.Wrapper.Mapinfo.Extensions
{
    public static class SessionExtensions
    {
           public static ITable<TEntity> ResolveTable<TEntity>(this IMapInfoWrapper mapinfo, string tableName)
               where TEntity : BaseEntity, new()
           {
               return new Table<TEntity>(new MapInfoSession(mapinfo),tableName);
           }
    }
}