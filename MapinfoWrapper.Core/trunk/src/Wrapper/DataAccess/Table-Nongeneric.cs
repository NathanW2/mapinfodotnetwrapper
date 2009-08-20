namespace MapinfoWrapper.DataAccess
{
    using System;
    using System.Collections.Generic;
    using MapinfoWrapper.Core;
    using MapinfoWrapper.Core.Extensions;
    using MapinfoWrapper.Core.InfoWrappers;
    using MapinfoWrapper.DataAccess.RowOperations;
    using MapinfoWrapper.DataAccess.RowOperations.Entities;
    using MapinfoWrapper.DataAccess.RowOperations.Enumerators;
    using MapinfoWrapper.Mapinfo;
    using System.Collections.ObjectModel;
    using MapinfoWrapper.DataAccess.LINQ.SQLBuilders;
    using MapinfoWrapper.Exceptions;

    public interface IExposesSession
    {
        MapinfoSession MapinfoSession { get; }
    }

    /// <summary>
    /// A Mapinfo Table container which can be used when the
    /// table definition is not known. This will still give you strong access
    /// to the RowId column.
    /// </summary>
    // HACK! This class feels like it's doing to much, needs a bit of a refactor.
    public class Table : ITable, IEquatable<Table>, IExposesSession
    {
        private readonly string name;
        protected readonly IDataReader reader;
        private readonly TableInfoWrapper tableinfo;

        /// <summary>
        /// The event that is raised just before the table is saved.
        /// </summary>
        public event Action<Table> OnTableSaving;

        internal Table(MapinfoSession MISession, string tableName)
        {
            Guard.AgainstNull(MISession,"MISession");
            Guard.AgainstNullOrEmpty(tableName,"tableName");
           
            this.MapinfoSession = MISession;
            this.name = tableName;
            this.reader = new DataReader(MISession, tableName);
            this.tableinfo = new TableInfoWrapper(MISession);
            this.TableManger = new TableChangeManger();
            this.EntityFactory = new EntityFactory(MISession, this,this.reader);
        }

        /// <summary>
        /// Gets the <see cref="MapinfoSession"/> that this table is associated with.
        /// </summary>
        public MapinfoSession MapinfoSession { get; private set; }

        /// <summary>
        /// Gets the <see cref="TableManger"/> for the current table.  Allows for access to current
        /// change set for the table.
        /// </summary>
        public TableChangeManger TableManger { get; private set; }
        
        /// <summary>
        /// Gets the <see cref="EntityFactory"/> that is used by this table to create entities.
        /// </summary>
        internal EntityFactory EntityFactory { get; set; }

        /// <summary>
        /// Returns a <see cref="BaseEntity"/> for the supplied index in the table.
        /// </summary>
        /// <param name="index">The index at which to get the <see cref="BaseEntity"/></param>
        /// <returns>An instace of <see cref="BaseEntity"/> for the supplied index.</returns>
        public BaseEntity this[int index]
        {
            get
            {
                return this.EntityFactory.GenerateEntityForIndex<BaseEntity>(index);
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
                return new RowList<BaseEntity>(this, this.reader,this.MapinfoSession,this.EntityFactory);
            }
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

        private IEnumerable<Column> columns;
        public IEnumerable<Column> Columns
        {
            get
            {
                if (this.columns == null)
                {
                    List<Column> cols = new List<Column>();
                    ColumnFactory colfactory = new ColumnFactory(this.MapinfoSession,this);
                    // Create the row id and object column.
                    cols.Add(colfactory.CreateColumnForRowId());
                    cols.Add(colfactory.CreateColumnForObj());
                    for (int i = 1; i < this.GetNumberOfColumns() + 1; i++)
                    {
                        cols.Add(colfactory.CreateColumnFor(i));
                    }
                    this.columns = cols;
                }
                return this.columns;
            }
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
            this.RaiseOnSaving();

            string command = "Commit Table {0}".FormatWith(this.Name);

            if (interactive)
                command += " Interactive";

            this.MapinfoSession.RunCommand(command);
        }

        /// <summary>
        /// Raises the OnTableSaving event for this table.
        /// </summary>
        private void RaiseOnSaving()
        {
            if (this.OnTableSaving != null)
                this.OnTableSaving(this);
        }

        /// <summary>
        /// Discards the changes made to a Mapinfo table, this is the same as calling the revert command in Mapinfo.
        /// </summary>
        public void DiscardChanges()
        {
            this.MapinfoSession.RunCommand("Rollback Table {0}".FormatWith(this.Name));
        }

        /// <summary>
        /// Close the current table in Mapinfo.
        /// </summary>
        public void Close()
        {
            this.MapinfoSession.RunCommand("Close Table {0}".FormatWith(this.Name));
        }

        /// <summary>
        /// Deletes a row from the table at the specified index.
        /// </summary>
        /// <param name="index"></param>
        public void DeleteRowAt(int index)
        {
            Guard.AgainstIsZero(index, "index");

            this.MapinfoSession.RunCommand("Delete From {0} Where Rowid = {1}".FormatWith(this.Name, index));
        }

        /// <summary>
        /// Deletes the selected entity from the current table.
        /// </summary>
        /// <param name="entity">The row that will be deleted.</param>
        public void Delete(BaseEntity entity)
        {
            Guard.AgainstNull(entity, "entity");

            this.DeleteRowAt(entity.RowId);

            entity.State = BaseEntity.EntityState.Deleted;
        }

        /// <summary>
        /// Inserts the supplied entity into the current table.
        /// </summary>
        /// <param name="entity">The entity that will be inserted into the table.</param>
        public void Insert(BaseEntity entity)
        {
            string insertstring = this.TableManger.GetInsertString(entity, this.Name);
            this.MapinfoSession.RunCommand(insertstring);
            // TODO Return the new repopulated entity, you just have to requery the object for the time being.
        }

        public void Update(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the entity to the table in a pending insert state.  Entities that are
        /// added to the table using this method are not inserted until CommitPendingChanges
        /// is called.
        /// </summary>
        /// <param name="entity">The entity to add the current table.</param>
        public void InsertOnCommit(BaseEntity entity)
        {
            this.TableManger.AddForInsert(entity);
        }

        /// <summary>
        /// Adds the entity to the table in a pending delete state.  Entities that are
        /// removed from the table using this method are not deleted until CommitPendingChanges
        /// is called.
        /// </summary>
        /// <param name="entity">The entity to add the current table.</param>
        public void DeleteOnCommit(BaseEntity entity)
        {
            this.TableManger.AddForDelete(entity);
        }

        /// <summary>
        /// Adds the entity to the table in a pending update state.  Entities that are
        /// updated using this method are not updated in the table until CommitPendingChanges
        /// is called.
        /// </summary>
        /// <param name="entity">The entity to add the current table.</param>
        public void UpdateOnCommit(BaseEntity entity)
        {
            if (entity.RowId == 0)
                throw new TableException("Entity is not yet inserted into this table, so it may not be updated");

            this.TableManger.AddForUpdate(entity);
        }

        /// <summary>
        /// Commits any pending inserts,updates and deletes to the table in Mapinfo.  
        /// This method does NOT save the underlying Mapinfo table.  
        /// To save any changes that have been made from calling this method you will need
        /// to call SaveChanges.
        /// </summary>
        public void CommitPendingChanges()
        {
            var changeset = this.TableManger.GetCurrentChangeSet();

            // Just return if we have no entities to process.
            if (changeset == null)
                return;
            
            //Process the inserts first.
            foreach (var entity in changeset.ForInsert)
            {
                this.Insert(entity);
            }

            // Remove all the entites from the pending insert list.
            this.TableManger.ClearInsertes();

            // Process the updates next.
            foreach (var entity in changeset.ForUpdate)
            {
                // TODO Implement update logic here.
            }

            // TODO Clear the update list.

            // Finally run though the deletes
            foreach (var entity in changeset.ForDelete)
            {
                this.Delete(entity);
            }

            this.TableManger.ClearDeletes();
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
            hash = (hash * 23) + this.MapinfoSession.GetHashCode();
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

            return (this.MapinfoSession == other.MapinfoSession && this.name == other.Name);
        }

        public override bool Equals(object obj)
        {
            Table table = obj as Table;

            return this.Equals(table);
        }

        public static bool operator ==(Table t1, Table t2)
        {
            if ((object)t1 == null)
                return ((object)t2 == null);
            else
                return t1.Equals(t2);

        }

        public static bool operator !=(Table t1, Table t2)
        {
            return !(t1 == t2);
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
        public Table<TEntity> ToGenericTable<TEntity>()
            where TEntity : BaseEntity, new()
        {
            // We can return a new generic version of the table here,
            // because we have overriden Equals and GetHashCode, making the
            // new table and this one equal.
            return new Table<TEntity>(this.MapinfoSession, this.Name);
        }
    }   
}