using System;
using System.Collections.Generic;
using MapinfoWrapper.Core;
using MapinfoWrapper.Core.Extensions;
using MapinfoWrapper.DataAccess.RowOperations;
using MapinfoWrapper.DataAccess.RowOperations.Entities;
using MapinfoWrapper.DataAccess.RowOperations.Enumerators;
using MapinfoWrapper.Mapinfo;

namespace MapinfoWrapper.DataAccess
{
    /// <summary>
    /// A Mapinfo Table container which can be used when the
    /// table definition is not known. This will still give you strong access
    /// to the RowId column.
    /// </summary>
    public class Table : ITable
    {
        protected readonly MapinfoSession miSession;
        protected readonly string internalname;
        protected readonly IDataReader reader;

        internal Table(MapinfoSession MISession, IDataReader reader, string tableName)
        {
            miSession = MISession;
            this.internalname = tableName;
            this.reader = reader;
        }

        /// <summary>
        /// Returns a <see cref="T:TEntity"/> at the supplied index in the table.
        /// </summary>
        /// <param name="index">The index at which to get the <see cref="T:TEntity"/></param>
        /// <returns>An instace of <see cref="T:TEntity"/> for the supplied index.</returns>
        public BaseEntity this[int index]
        {
            get
            {
                BaseEntity row = new BaseEntity();
                row.RowId = index;
                row.reader = this.reader;
                return row;
            }
        }

        /// <summary>
        /// Rows a collection of rows from the table, using <see cref="T:BaseEntity"/> as
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
            Guard.AgainstNotInserted(entity, "entity");

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
                string returnValue = (String)this.miSession.RunTableInfo(this.Name,
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
                    object value = this.miSession.RunTableInfo(this.Name, TableInfo.Number);
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
                    string value = (string)this.miSession.RunTableInfo(this.Name, TableInfo.Mappable);
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
                string value = (string)this.miSession.RunTableInfo(this.Name, TableInfo.Ncols);
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
    }
}