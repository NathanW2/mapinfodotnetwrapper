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
    using Wrapper.Exceptions;

    /// <summary>
    /// A Mapinfo Table container which can be used when the
    /// table definition is not known. This will still give you strong access
    /// to the RowId column.
    /// </summary>
    // HACK! This class feels like it's doing to much, needs a bit of a refactor.
    public class Table : ITable, IEquatable<Table>
    {
        protected readonly MapinfoSession miSession;
        private readonly string name;
        protected readonly IDataReader reader;
        private readonly TableInfoWrapper tableinfo;

        internal Table(MapinfoSession MISession, string tableName)
        {
            Guard.AgainstNull(MISession,"MISession");
           
            this.miSession = MISession;
            this.name = tableName;
            this.reader = new DataReader(this.miSession, tableName);
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

            if (entity.AttachedTo != this) 
                throw new TableException("Entity is not currently attached to this table");

            this.DeleteRowAt(entity.RowId);
        }

        internal BaseEntity InsertRow(BaseEntity newRow)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the name of the current table.
        /// </summary>
        public string Name
        {
            get
            {
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

        private bool? mappable;

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
        /// Returns a new <see cref="Table{TEntity}"/> using the <typeparamref name="TEntity"/> as the
        /// tables row entity.
        /// 
        /// <para>This function is usful if you have retrived an table from a <see cref="MapinfoSession"/>'s  <see cref="TableCollection"/> 
        /// which are stored as <see cref="Table"/> but you know the entity type for it.</para>
        /// </summary>
        /// <typeparam name="TEntity">The type to use as the entity type for the table.</typeparam>
        /// <returns>A new <see cref="Table{TEntity}"/> for the same table as this <see cref="Table"/>.</returns>
        public Table<TEntity> CastToGenericTable<TEntity>()
            where TEntity : BaseEntity, new()
        {
            // We can return a new generic version of the table here,
            // because we have overriden Equals and GetHashCode, making the
            // new table and this one equal.
            return new Table<TEntity>(miSession, this.Name);
        }

        /// <summary>
        /// Builds up entities for table, setting all the needed properties.
        /// </summary>
        // HACK! This class is doing similer things to DataReader and might need to be refactored.
        // HACK! This class doesn't belong here.
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

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            int hash = 17;
            hash = (hash * 23) + this.Name.GetHashCode();
            hash = (hash * 23) + this.miSession.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public bool Equals(Table other)
        {
            if (other == null) 
                return false;

            return (this.miSession == other.miSession && this.name == other.Name);
        }

        public override bool Equals(object obj)
        {
            Table table = obj as Table;

            return this.Equals(table);
        }        public static bool operator ==(Table t1, Table t2)        {
            if ((object)t1 == null || ((object)t2 == null))
                return false;
            else return
                t1.Equals(t2);

        }

        public static bool operator !=(Table t1, Table t2)
        {
            return !(t1 == t2);
        }    }   
}