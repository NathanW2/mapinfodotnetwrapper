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
        public Coordinate Start
        {
            get { return base.Nodes[0]; }
            set { base.Nodes[0] = value; }
        }
        
        /// <summary>
        /// Returns the end coordinates for the line.
        /// </summary>
        public Coordinate End
        {
            get { return base.Nodes[1]; }
            set { base.Nodes[1] = value; }
        }

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

        public override string ToBasicCreateCommand()
        {
            return "CreateLine({0},{1})".FormatWith(this.Start.ToString(), this.End.ToString());
        }

        public override string ToExtendedCreateString(string variableName)
        {
            return "Create Line Into Variable {0} ({1}) ({2}) {3}".FormatWith(variableName,
                                                                         this.Start.ToString(),
                                                                         this.End.ToString(),
                                                                         this.Style);
        }

        public override string ToExtendedCreateString(int windowID)
        {
            return "Create Line Into Window {0} ({1}) ({2}) {3}".FormatWith(windowID,
                                                             this.Start.ToString(),
                                                             this.End.ToString(),
                                                             this.Style);
        }
    }

    public class LineStyle
    { 
        
    }
}
