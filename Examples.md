## Code Examples ##
This page contains some sample code examples which show the difference between using the wrapper and not.

### Open a table,loop through all rows and print the column values to the console ###
**Without Mapinfo OLE wrapper**
```
      MapInfo.MapInfoApplicationClass mapinfoinstance = new MapInfo.MapInfoApplicationClass();

      //Open the table in mapinfo using the string open table command
      mapinfoinstance.Do(String.Format("OpenTable({0})", @"C:\Temp\Lines"));
      
      //Get the number of rows from the currently open mapinfo table, because the eval function just
      //returns a string we convert it to an integer so that we can use it in the loop below.
      string tablename = "Lines";
      int noofrows = Convert.ToInt32(mapinfoinstance.Eval(string.Format("Tableinfo({0},8)", tablename)));

      //Pass the command to mapinfo to fetch the first record from the table in mapinfo.
      mapinfoinstance.Do("Fetch First From Lines");
      for (int row = 0; row < NoOfRows - 1; row++)
      {
         //Retrun the value of the selected row for the Road column.

         String value = mapinfoinstance.Eval("Lines.Road");
         Console.WriteLine(value);

         //Pass the command to mapinfo to fetch the next record from the table in mapinfo.
         mapinfoinstance.Do("Fetch Next From Lines");
       }
```

**With Mapinfo OLE wrapper** - Method 1

```
     //Create an instance of Mapinfo's COM object.
     COMMapinfo wrapper = COMMapinfo.CreateInstance();
     //Open a table in Mapinfo using wrapped OpenTable command
     Table table = Table.OpenTable(@"C:\Temp\Lines");
     //Loop through each row in the open table and print the value of the road column to the console.
     foreach (BaseEntity item in table.Rows)
     {
        //You can access the rowId column by calling the RowId property which is built into the wrapper.        
        Console.WriteLine(item.RowId);
     }
```

**With Mapinfo OLE wrapper** - Method 2 - using entity object(strong typed)

You can also define a entity for the table, which will be used to get strong typed access to the columns in table.

_Note! I am in the process of writing a tool that will generate entities based on the columns in a Mapinfo TAB file._

```
    //Define a entity as a POCO(Plain old CLR object) and inherit from BaseEntity or                                                            
    //MappableRow.  The MappableRow base object will give you access to the RowID and obj
    //column in the table.
    class RoadEntity : MappableEntity
    {
        public string Road { get; set;}
        public int AssetID {get; set:}    
    }

```

_Using the Entity_

```
   //Open a table in Mapinfo using wrapped OpenTable command, and using our LineEntity as 
   //the generic type.
   Table<LineEntity> table = Table.OpenTable<LineEntity>(@"C:\Temp\Lines");
   //We can now loop through all the rows in the table, but we now have a strong typed
   //RoadEntity rather then a just a BaseEntity object.
   foreach (LineEntity item in table.Rows)
   {
        //Because we are getting back a LineEntity we have access to the Road property, 
        //which matches the Road column in the table and has been casted into a string for 
        //us in our entity.
        string roadname = item.Road;   //<-item.Road will return a string not object.         
        Console.WriteLine(roadname);        
   }
```

_Using LINQ against the table_

```
   //Because we gave our open table command an entity object we can use LINQ against the 
   //table.
   IQueryable<RoadEntity>query = from row in table
                                 where row.Road == "Some Road Name"
                                 select row;

   //We can print out the string that the query will generate using an extension method.
   Console.WriteLine(query.ToQueryString());

   //Loop
   foreach (RoadEntity item in query)
   {
     Console.WriteLine(item.Road);
   }
```