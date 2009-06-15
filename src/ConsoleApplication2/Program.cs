using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper.MapinfoFactory;
using Wrapper.TableOperations;
using Wrapper;
using Wrapper.TableOperations.RowOperations;
using Wrapper.ObjectOperations;
using Wrapper.Extensions;
using Wrapper.MapOperations;
using Wrapper.ObjectOperations.Points;
using Wrapper.MapbasicOperations;
using System.Data;
using Wrapper.TableOperations.LINQ;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main()
        {
            IMapinfoWrapper mapinfo = COMMapinfo.CreateInstance();
            Table<LineEntity> table = Table.OpenTable<LineEntity>(mapinfo, @"c:\Temp\Lines.TAB");

            var linq = table.Where(row => row.Road == "Test Road Name")
                            .Select(row => new 
                                              { 
                                              Name = row.Road,
                                              MyObjectType = row.obj.ObjectType,
                                              Seg = row.SegmentID
                                              });

            Console.WriteLine(linq.ToQueryString());

            foreach (var item in linq)
            {
                Console.WriteLine(item);
            }
        }
    }

   

    //    static void Main(string[] args)
    //    {
    //        // Create a new instance of Mapinfo.
    //        IMapinfoWrapper mapinfo = COMMapinfo.CreateInstance();
    //        // Open a table, supplying a table template to get strong typed access to columns.
    //        Table<LineEntity> table = Table.OpenTable<LineEntity>(mapinfo, @"c:\Temp\Lines.TAB");

    //        Console.WriteLine("Row Count:{0}", table.Rows.Count());
    //        foreach (var row in table.Rows)
    //        {
    //            string road = row.Road;
    //            int lineid = Convert.ToInt32(row.GetValue(col => col.LineID));

    //            Console.WriteLine("Road {0} || Line ID {1}",row.Road, row.GetValue(col => col.LineID));
    //        }

    //        Console.WriteLine("======Inserting new row===============");

    //        // Create a new point object.
    //        MapbasicVariable variable = MapbasicVariable.Declare(mapinfo, "Test", "Object");
    //        Coordinate location = new Coordinate(503160.10, 10019452.17);
    //        Point point = Point.CreatePoint(mapinfo,location, variable);

    //        LineEntity line = new LineEntity(mapinfo);
    //        line.SetValue(col => col.LineID, 9999999);
    //        line.Road = "This is a new object using the new obj accessor".InQuotes();
    //        line.obj = point;

    //        table.Rows.Add(line);

    //        Console.WriteLine("Row Count:{0}", table.Rows.Count());
    //        foreach (var row in table.Rows)
    //        {
    //            Console.Write("Road {0} || Line ID {1}", row.Road, row.GetValue(col => col.LineID));
    //            Console.Write(" || Type {0}".FormatWith(((MapbasicObject)row.obj).ObjectType.ToString()));
    //            Console.WriteLine("");
    //        }

    //        table.SaveChanges();
    //    }
    //}

    interface ILineEntity
    {
        int LineID { get; set; }
        int SegmentID { get; set; }
        string Road { get; set; }
        string Locality { get; set; }
        int StartX { get; set; }
        int StartY { get; set; }
        int EndX { get; set; }
        int EndY { get; set; }
        int StartDist { get; set; }
        int EndDist { get; set; }
    }

    class LineEntity : MappableRow<ILineEntity>
    {
        public string Road
        {
            get
            {
                object retured = base.Get("Road");
                return (string)retured;
            }
            set
            {
                base.Set("Road", value);
            }
        }

        public int SegmentID
        {
            get
            {
                object returned = base.Get("SegmentID");
                return Convert.ToInt32(returned);
            }
            set
            {
                base.Set("SegmentID", value);
            }
        }
    }
}
