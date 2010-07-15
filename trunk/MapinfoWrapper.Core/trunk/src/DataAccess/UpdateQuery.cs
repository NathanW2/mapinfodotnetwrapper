using System;
using System.Collections;
using System.Collections.Generic;
using MapInfo.Wrapper.Core;
using MapInfo.Wrapper.DataAccess.Entities;
using MapInfo.Wrapper.DataAccess.Row;
using MapInfo.Wrapper.Mapinfo;
using MapInfo.Wrapper.Mapinfo.Extensions;

namespace MapInfo.Wrapper.DataAccess
{
    /// <summary>
    /// Represents and update string that can be run in MapInfo.
    /// </summary>
    public class UpdateQuery : Query
    {
        public UpdateQuery(IMapInfoWrapper session, string queryString)
            : base(session,queryString)
        {}

        /// <summary>
        /// Executes a query with no return result, eg INSERT and UPDATE statments.
        /// </summary>
        public override void ExecuteNonQuery()
        {
            string command = base.GetQueryString();
            base.MapInfoSession.Do(command);
        }
    }

    public class SelectQuery<TEntity> : Query, IEnumerable<TEntity>
        where TEntity : BaseEntity, new()
    {
        private readonly IMapInfoWrapper session;
        private readonly string query;

        public SelectQuery(IMapInfoWrapper miSession, string queryString)
            : base(miSession, queryString)
        {
            this.session = base.MapInfoSession;
            this.query = base.GetQueryString();
        }

        public override void ExecuteNonQuery()
        {
            base.MapInfoSession.Do(base.GetQueryString());
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            this.session.Do(this.query);
            this.ResultTable = this.session.ResolveTable<TEntity>("TempTable");

            IMapInfoDataReader reader = new MapInfoDataReader(this.session, "TempTable");

            while (!reader.EndOfTable())
            {
                TEntity entity = new TEntity();
                entity.RowId = reader.CurrentRecord;
                entity.State = BaseEntity.EntityState.PossiblyModifed;
                yield return entity;

                reader.FetchNext();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns the <see cref="ITable"/> that was generated from the query.
        /// </summary>
        public ITable ResultTable { get; private set; }
    }
}
