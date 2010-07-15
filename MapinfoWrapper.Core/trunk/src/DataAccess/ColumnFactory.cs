﻿using MapInfo.Wrapper.Core.Extensions;
using System;
using MapInfo.Wrapper.DataAccess.Row;
using MapInfo.Wrapper.Mapinfo;



namespace MapInfo.Wrapper.DataAccess
{
    enum ColumnAttribute
    {
        Name = 1,
        Number = 2,
        Type = 3,
        Width = 4,
        DecimalPlaces = 5,
        Indexed = 6,
        Editable = 7
    }

    class ColumnInfoWrapper
    {
        public ColumnInfoWrapper(MapInfoSession MISession, string tableName)
        {
            this.MapInfoSession = MISession;
            this.TableName = tableName;
        }

        public string TableName { get; private set; }
        public MapInfoSession MapInfoSession { get; private set; }

        public string GetColumnName(string columnName)
        {
            // TODO Add catch for no exsiting column
            string name = (string)this.ColumnInfo(columnName, ColumnAttribute.Name);
            return name;
        }

        public string GetColumnName(int columnIndex)
        {
            string name = (string)this.ColumnInfo(columnIndex, ColumnAttribute.Name);
            return name;
        }

        public object ColumnInfo(string columnName, ColumnAttribute attrbute)
        {
            int attributenumber = (int)attrbute;
            return this.MapInfoSession.Eval("ColumnInfo({0},{1},{2})".FormatWith(this.TableName.InQuotes(),
                                                                                     columnName.InQuotes(),
                                                                                     attributenumber));
        }

        public object ColumnInfo(int columnIndex, ColumnAttribute attribute)
        {
            return this.ColumnInfo("COL{0}".FormatWith(columnIndex), attribute);
        }

        internal int GetColumnNumber(string columnName)
        {
            string value = (string)this.ColumnInfo(columnName, ColumnAttribute.Number);
            return Convert.ToInt32(value);
        }
    }

    /// <summary>
    /// A factory used to create and populate <see cref="Column"/> objects from Mapinfo.
    /// </summary>
    class ColumnFactory
    {
        private string[] specialcolumns = { "rowid", "obj" };

        public ColumnFactory(MapInfoSession MISession, ITable table)
        {
            this.MapInfoSession = MISession;
            this.Table = table;
            this.ColumnInfo = new ColumnInfoWrapper(MISession,table.Name);
        }

        /// <summary>
        /// Gets the current <see cref="MapInfoSession"/> assigned to this object.
        /// </summary>
        public MapInfoSession MapInfoSession { get; private set; }
        
        /// <summary>
        /// Gets the <see cref="Table"/> that is associated with this <see cref="ColumnFactory"/>.
        /// </summary>
        public ITable Table { get; private set; }

        /// <summary>
        /// Gets the instance of the <see cref="ColumnInfoWrapper"/> used by this factory.
        /// </summary>
        public ColumnInfoWrapper ColumnInfo { get; private set; }

        /// <summary>
        /// Creates and returns a new <see cref="Column"/> for the supplied column using the column name.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>A new instace of a <see cref="Column"/>.</returns>
        public Column CreateColumnFor(string columnName)
        {
            Column column = new Column();
            column.Name = this.ColumnInfo.GetColumnName(columnName);
            column.Number = this.ColumnInfo.GetColumnNumber(columnName);
            column.Table = this.Table;
            return column;
        }

        /// <summary>
        /// Creates and returns a <see cref="Column"/> representing the RowID column in the table.
        /// </summary>
        /// <returns>A new <see cref="Column"/>.</returns>
        public Column CreateColumnForRowId()
        {
            Column column = new Column();
            column.Name = "RowID";
            column.Type = ColumnType.INTEGER;
            column.Table = this.Table;
            column.Number = 0;
            return column;
        }

        public Column CreateColumnForObj()
        {
            Column column = new Column();
            column.Name = "Obj";
            column.Type = ColumnType.GRAPHIC;
            column.Table = this.Table;
            column.Number = 0;
            return column;
        }

        /// <summary>
        /// Creates and returns a new <see cref="Column"/> for the supplied column index.
        /// </summary>
        /// <param name="columnIndex">The index of the column.</param>
        /// <returns>A new instance of <see cref="Column"/>.</returns>
        public Column CreateColumnFor(int columnIndex)
        {
            if (columnIndex == 0)
                throw new ArgumentOutOfRangeException(@"Column at index 0 does not exist in Mapinfo tables.
                                                        If you are looking for the RowID column consider using CreateColumnForRowId method");

            return this.CreateColumnFor("COL{0}".FormatWith(columnIndex));
        }
    }
}
