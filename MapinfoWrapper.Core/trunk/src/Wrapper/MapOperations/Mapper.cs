using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.DataAccess;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.MapOperations
{
    public class Mapper
    {
        private readonly MapinfoSession miSession;

        public Mapper(MapinfoSession MISession)
        {
            miSession = MISession;
        }

        /// <summary>
        /// Opens a new Map window in Mapinfo using the tables supplied as the layers for that map.
        /// </summary>
        /// <param name="tablelist">A collection of tables which will be used in the new map window.</param>
        /// <returns>A map containing a referance to the newly opened map window.</returns>
        public MapWindow MapFromTables(IEnumerable<Table> tablelist)
        {
            var mappable = tablelist.ToList().FindAll(table => table.IsMappable);

            if (mappable == null)
                throw new ArgumentNullException("mappable", "No tables are mappable");

            StringBuilder commandbuilder = new StringBuilder("Map From ");
            foreach (var table in tablelist)
            {
                commandbuilder.AppendFormat("{0},".FormatWith(table.Name));
            }
            string command = commandbuilder.ToString().TrimEnd(',');
            this.miSession.RunCommand(command);
            return this.miSession.GetFrontWindow();
        }

        /// <summary>
        /// Opens a new Map window in Mapinfo using the table supplied as the first layer. 
        /// </summary>
        /// <param name="table">The table which will be opened in a new map window.</param>
        /// <returns>A map containing a referance to the newly opened map window. </returns>
        public MapWindow MapFromTable(Table table)
        {
            List<Table> tablelist = new List<Table>();
            tablelist.Add(table);
            return this.MapFromTables(tablelist);
        }
    }
}
