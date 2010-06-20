using Mapinfo.Wrapper.Geometries.Points;
using System;
using Mapinfo.Wrapper.Mapinfo;

namespace Mapinfo.Wrapper.Geometries
{
    class GeometryBuilder
    {
        public GeometryBuilder(string tableName, MapinfoSession MISession)
        {
            this.TableName = tableName;
            this.MapinfoSession = MISession;
        }

        public string TableName { get; set; }
        public MapinfoSession MapinfoSession { get; set; }

        public Geometry CreateGeometry()
        {
            string objecttype = this.MapinfoSession.Eval("ObjectInfo({0}.obj,1)".FormatWith(this.TableName));
            ObjectType type = (ObjectType)Convert.ToInt32(objecttype);
            switch (type)
            {
                case ObjectType.Arc:
                    break;
                case ObjectType.Ellipse:
                    break;
                case ObjectType.Line:
                    break;
                case ObjectType.Polyline:
                    break;
                case ObjectType.Point:
                    string sx = this.MapinfoSession.Eval("CentroidX({0}.Obj)".FormatWith(this.TableName));
                    string sy = this.MapinfoSession.Eval("CentroidY({0}.Obj)".FormatWith(this.TableName));
                    double x = Convert.ToDouble(sx);
                    double y = Convert.ToDouble(sy);
                    return new Point(x, y);
                case ObjectType.Frame:
                    break;
                case ObjectType.Region:
                    break;
                case ObjectType.Rectangle:
                    break;
                case ObjectType.RoundRectangle:
                    break;
                case ObjectType.Text:
                    break;
                case ObjectType.Multipoint:
                    break;
                case ObjectType.Collection:
                    break;
                default:
                    break;
            }
            return null;

        }
    }

    enum ObjectType
    {
        Arc = 1,
        Ellipse = 2,
        Line = 3,
        Polyline = 4,
        Point = 5,
        Frame = 6,
        Region = 7,
        Rectangle = 8,
        RoundRectangle = 9,
        Text = 10,
        Multipoint = 11,
        Collection = 12
    }
}
