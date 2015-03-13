## The following code only applies to Version 1.0 of the wrapper, the code has changed Version 2.0 ##

_Create a new Mapinfo instace_
```
 COMMapinfo mapinfo = COMMapinfo.CreateInstance();
```

_Opening a table_
```
 //We can call the open table command on the table type.
 Table table = Table.OpenTable(@"c:\Temp\TestTable.Tab");
```
_or_
```
 //Or we can supply a entity template for the table using generics.
 Table<TestEntity> table = Table.OpenTable<TestEntity>(@"C:\Temp\TestTable.Tab");
        
 //where TestEntity is:
 public class TestEntity : BaseEntity
 {
    public int AssetId {get;set;}
    public string AssetName {get;set;}
 }
```

_Using LINQ against a table_
```
  //After a table has been opened we can use a LINQ query to query the table in Mapinfo.
  //The LINQ query will map the return results into the supplied entity type.
  IQueryable<TestEntity> query = from row in table
				 where row.AssetName == "TestAsset"
				 select row;
```
