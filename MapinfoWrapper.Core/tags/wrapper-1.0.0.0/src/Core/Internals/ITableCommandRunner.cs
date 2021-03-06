﻿using MapinfoWrapper.DataAccess;

namespace MapinfoWrapper.Core.Internals
{
    internal interface ITableCommandRunner
    {
        string GetName(int tableNumber);
        string GetName(string tableName);
        void OpenTable(string path);
        string GetPath(string tableName);
        object RunTableInfo(string tableName, TableInfo attribute);
        void RunCommand(string command);
        string Evaluate(string command);
    }
}
