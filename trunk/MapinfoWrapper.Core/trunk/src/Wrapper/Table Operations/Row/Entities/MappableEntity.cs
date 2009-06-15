using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapinfoWrapper.Geometries;

namespace MapinfoWrapper.TableOperations.RowOperations.Entities
{
    public class MappableEntity : BaseEntity
    {
        public IGeometry obj { get; set; }
    }
}
