using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using MapinfoWrapper.Core;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.Core.InfoWrappers;
using MapinfoWrapper.DataAccess.RowOperations;
using MapinfoWrapper.DataAccess.RowOperations.Entities;
using MapinfoWrapper.DataAccess.RowOperations.Enumerators;
using MapinfoWrapper.Mapinfo;
using IDataReader=MapinfoWrapper.DataAccess.RowOperations.IDataReader;

namespace MapinfoWrapper.DataAccess
{
    /// <summary>
    /// A Mapinfo Table container which can be used when the
    /// table definition is not known. This will still give you strong access
    /// to the RowId column.
    /// </summary>
    // HACK! This class feels like it's doing to much, needs a bit of a refactor.
    public class Table : ITable
    {
        protected readonly MapinfoSession miSession;
        protected readonly string internalname;
        protected readonly IDataReader reader;
        protected internal readonly TableInfoWrapper tableinfo;

        internal Table(MapinfoSession MISession, IDataReader reader, string tableName)
        {
            Guard.AgainstNull(MISession,"MISession");
            Guard.AgainstNull(reader, "reader");
            
            miSession = MISession;
            this.internalname = tableName;
            this.reader = reader;

            this.tableinfo = new TableInfoWrapper(MISession);
        }

        /// <summary>
        /// Returns a <see cref="BaseEntity"/> for the supplied index in the table.
        /// </summary>
        /// <param name="index">The index at which to get the <see cref="BaseEntity"/></param>
        /// <returns>An instace of <see cref="BaseEntity"/> for the supplied index.</returns>
        public BaseEntity this[int index]
        {
            get
            {
                EntityBuilder builder = new EntityBuilder(this.miSession, this);
                return builder.GenerateEntityForIndex<BaseEntity>(index);
            }
        }

        /// <summary>
        /// Rows a collection of rows from the table, using <see cref="BaseEntity"/> as
        /// the row collection type.
        /// </summary>
        public IEnumerable<BaseEntity> Rows
        {
            get 
            {
                return new RowList<BaseEntity>(this.Name, this.reader);
            }
        }

        /// <summary>
        /// Deletes a row from the table at the specified index.
        /// </summary>
        /// <param name="index"></param>
        public void DeleteRowAt(int index)
        {
            Guard.AgainstIsZero(index, "index");

            this.miSession.RunCommand("Delete From {0} Where Rowid = {1}".FormatWith(this.Name, index));
        }

        /// <summary>
        /// Deletes the selected row from the current table.
        /// </summary>
        /// <param name="entity">The row that will be deleted.</param>
        public void DeleteRow(BaseEntity entity)
        {
            Guard.AgainstNull(entity, "entity");

            if (entity.State == BaseEntity.EntityState.NotInserted)
                throw new ArgumentOutOfRangeException("Entity is not currently attached to this table");

            this.DeleteRowAt(entity.RowId);
        }

        internal BaseEntity InsertRow(BaseEntity newRow)
        {
            throw new NotImplementedException();
        }

        private string name;

        /// <summary>
        /// Returns the name of the current table.
        /// </summary>
        public string Name
        {
            get
            {
                if (this.name == null)
                {
                    //name = this.tablecommandrunner.GetName(this.internalname);
                    this.name = this.internalname;
                }
                return name;
            }
        }

        /// <summary>
        /// Returns the path of the tab file for the underlying table,
        /// if the table is a query table it will retrun null.
        /// </summary>
        public System.IO.DirectoryInfo TablePath
        {
            get
            {
                string returnValue = (String) this.tableinfo.GetTableInfo(this.name,
                                                                          TableInfo.Tabfile);
                if (string.IsNullOrEmpty(returnValue))
                {
                    return null;
                }
                else
                {
                    return new System.IO.DirectoryInfo(returnValue);
                }
            }
        }

        private int? number;

        /// <summary>
        /// Returns the number of the current table.
        /// </summary>
        public int Number
        {
            get
            {
                if (number == null)
                {
                    object value = this.tableinfo.GetTableInfo(this.Name, TableInfo.Number);
                    number = Convert.ToInt32(value);
                }
                return number.Value;
            }
        }

        public bool? mappable;

        /// <summary>
        /// Returns if the table is mappable or not.
        /// </summary>
        public bool IsMappable
        {
            get
            {
                if (mappable == null)
                {
                    string value = (string)this.tableinfo.GetTableInfo(this.Name, TableInfo.Mappable);
                    mappable = value == "T";
                }
                return mappable.Value;
            }
        }

        private int? cols;

        /// <summary>
        /// Returns the number of columns in the current table.
        /// </summary>
        /// <returns>The number of columns in the current table.</returns>
        public int GetNumberOfColumns()
        {
            if (cols == null)
            {
                string value = (string)this.tableinfo.GetTableInfo(this.Name, TableInfo.Ncols);
                cols = Convert.ToInt32(value);    
            }
            return cols.Value;
        }

        /// <summary>
        /// Commits any pending changes for the current table.
        /// </summary>
        public void SaveChanges()
        {
            this.SaveChanges(false);
        }

        /// <summary>
        /// Commits any pending changes for the current table.
        /// </summary>
        /// <param name="interactive">If set to true in the event of a conflict, MapInfo Professional displays the Conflict Resolution dialog box.
        /// <para>After a successful Commit Table Interactive statement, MapInfo Professional displays a dialog box allowing the user to refresh.</para>
        /// </param>
        public void SaveChanges(bool interactive)
        {
            string command = "Commit Table {0}".FormatWith(this.Name);

            if (interactive)
                command += " Interactive";

            this.miSession.RunCommand(command);
        }

        /// <summary>
        /// Discards the changes made to a Mapinfo table, this is the same as calling the revert command in Mapinfo.
        /// </summary>
        public void DiscardChanges()
        {
            this.miSession.RunCommand("Rollback Table {0}".FormatWith(this.Name));
        }

        /// <summary>
        /// Close the current table in Mapinfo.
        /// </summary>
        public void Close()
        {
            this.miSession.RunCommand("Close Table {0}".FormatWith(this.Name));
        }

        /// <summary>
        /// Builds up entities for table, setting all the needed properties.
        /// </summary>
        // HACK! This class is doing similer things to DataReader and might need to be refactored.
        internal class EntityBuilder
        {
            private readonly MapinfoSession miSession;
            private readonly Table table;
            private readonly DataReader datareader;

            public EntityBuilder(MapinfoSession MISession, Table table)
            {
                miSession = MISession;
                this.table = table;
                this.datareader = new DataReader(MISession, table.Name);
            }

            public int CurrentRecord { get; set; }  

            public TEntity GenerateEntityForIndex<TEntity>(int index)
                where TEntity : BaseEntity, new()
            {
                string name = this.table.Name;
                if (index != this.CurrentRecord)
                    this.datareader.Fetch(index);

                TEntity entity = new TEntity();
                entity.AttachedTo = this.table;
                entity.reader = this.datareader;
                return this.datareader.PopulateEntity(entity);
            }

            public TEntity GenerateEntityForNextRecord<TEntity>()
            {
                throw new NoNullAllowedException();
            }
        }
    }
}