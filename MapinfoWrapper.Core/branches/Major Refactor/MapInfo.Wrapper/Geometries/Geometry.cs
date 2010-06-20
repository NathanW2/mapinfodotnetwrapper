using Mapinfo.Wrapper.Core.Extensions;
using Mapinfo.Wrapper.MapbasicOperations;
using Mapinfo.Wrapper.Mapinfo;

namespace Mapinfo.Wrapper.Geometries
{
    public class Geometry
    {
        public Geometry(Variable variable, IMapinfoWrapper mapinfoSession) 
        {
            this.Variable = variable;
            this.Mapinfo = mapinfoSession;
        }

        public bool Contains(Geometry compareTo)
        {
            return "{0} Contains {1}".FormatWith(this.Variable.Name, compareTo.Variable.Name)
                                     .ToBoolean();
        }

        public bool Contains(Coordinate compareTo)
        {
            return "{0} Contains CreatePoint({1},{2})".FormatWith(this.Variable.Name, compareTo.X,compareTo.Y)
                                                      .ToBoolean();
        }

        public bool Intersects(Geometry compareTo)
        {
            return "{0} Intersects {1}".FormatWith(this.Variable.Name, compareTo.Variable.Name)
                                       .ToBoolean();
        }

        public bool Intersects(Coordinate compareTo)
        {
            return "{0} Intersects CreatePoint({1},{2})".FormatWith(this.Variable.Name, compareTo.X, compareTo.Y)
                                                        .ToBoolean();
        }

        public bool IsWithin(Geometry compareTo)
        {
            return "{0} Within {1}".FormatWith(this.Variable.Name, compareTo.Variable.Name)
                                   .ToBoolean();
        }

        public bool IsWithin(Coordinate compareTo)
        {
            return "{0} Within CreatePoint({1},{2})".FormatWith(this.Variable.Name, compareTo.X, compareTo.Y)
                                                    .ToBoolean();
        }

        /// <summary>
        /// The variable that is linked to this <see cref="Geometry"/>.
        /// </summary>
        public Variable Variable { get; set; }

        /// <summary>
        /// The <see cref="MapinfoSession"/> that the <see cref="Geometery"/> belongs to.
        /// </summary>
        public IMapinfoWrapper Mapinfo { get; set; }
    }
}