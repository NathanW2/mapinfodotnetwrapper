namespace MapinfoWrapper.Wrapper.Geometries
{
    using System;
    using MapinfoWrapper.Core.Extensions;
    using MapinfoWrapper.Geometries;
    using MapinfoWrapper.Wrapper.MapbasicOperations;

    /// <summary>
    /// Represents a point object.
    /// </summary>
    [Serializable]
    public class Line : Geometry
    {
        public Line()
            : this(0.0,0.0,0.0,0.0)
        { }

        public Line(double startx, double starty, double endx, double endy)
            : this(new Coordinate(startx, starty), new Coordinate(endx, endy))
        { }        

        public Line(Coordinate start, Coordinate end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Returns the start coordinates for the line.
        /// </summary>
        public Coordinate Start { get; set; }
        
        /// <summary>
        /// Returns the end coordinates for the line.
        /// </summary>
        public Coordinate End { get; set; }

        public override Coordinate Centroid()
        {
            throw new NotImplementedException();
        }

        private LineStyle style;
        public LineStyle Style
        {
            get
            {
                return this.style;
            }
            set 
            {
                this.style = value;
            }
        }

        public string ToExtendedCreateString()
        {
            throw new NotImplementedException();
        }

        internal MapbasicCommand ToCreateCommand()
        {
            MapbasicCommand CreateCommand = new MapbasicCommand();
            CreateCommand = "CreateLine({0},{1})".FormatWith(this.Start.ToString(), this.End.ToString());
            return CreateCommand;
        }
    }

    public class LineStyle
    { 
        
    }
}
