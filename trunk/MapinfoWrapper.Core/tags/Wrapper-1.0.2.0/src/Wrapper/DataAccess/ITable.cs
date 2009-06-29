using System;
using System.Linq;
namespace MapinfoWrapper.DataAccess
{
    public interface ITable : IQueryable
    {
        string Name { get; }
        int Number { get; }
        bool IsMappable { get; }
        void Close();
    }
}
