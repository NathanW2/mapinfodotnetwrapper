using MapInfo.Wrapper.Core;
using MapInfo.Wrapper.DataAccess.Entities;
using MapInfo.Wrapper.Mapinfo;

namespace MapInfo.Wrapper.DataAccess
{
    public class Query
    {
        private readonly string querystring;

        public Query(IMapInfoWrapper miSession,string queryString)
        {
            Guard.AgainstNull(miSession, "session");
            this.MapInfoSession = miSession;
            this.querystring = queryString;
        }

        /// <summary>
        /// Executes a query with no return result, eg INSERT and UPDATE statments.
        /// </summary>
        public virtual void ExecuteNonQuery()
        {
            this.MapInfoSession.Do(querystring);
        }

        /// <summary>
        /// Returns the query string that will be executed when this query is run.
        /// </summary>
        /// <returns></returns>
        public virtual string GetQueryString()
        {
            return this.querystring;
        }

        public IMapInfoWrapper MapInfoSession { get; protected set; }

        public SelectQuery<TEntity> ToSelect<TEntity>()
            where TEntity : BaseEntity, new()
        {
            return new SelectQuery<TEntity>(this.MapInfoSession,this.GetQueryString());
        }
    }
}
