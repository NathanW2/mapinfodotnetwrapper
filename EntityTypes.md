**Note:  The following information only currently applies to the upcoming version 2.0 of the wrapper and may be incomplete or have changed since writing.**

# Information about entity types in the Mapinfo wrapper. #



## Introduction ##

In the Mapinfo .Net OLE wrapper you can have the option to represent a row in a table as a strong typed entity, anyone who has used LINQ-To-SQL, Entity Framework, NHibernate or any other ORM will feel familiar with this idea.

An entity object is basiclly a object that contains properties that represent the columns in the table that the entity is for.

_For example_, say we have a Mapinfo table called nodes.tab that has the following structure:

```
!table
!version 300
!charset WindowsLatin1

Definition Table
  Type NATIVE Charset "Neutral"
  Fields 6
    Dist Decimal (10, 3) ;
    LineID Integer ;
    Name Char (64) ;
    Locality Char (32) ;
    X Decimal (10, 6) ;
    Y Decimal (10, 6) ;
```

We can represent this table structure with the following class:

```
public class Node
{
   public Decimal Dist {get; set;}
   public int LineID {get; set;}
   public String Name {get; set;}
   public String Locality {get; set;}
   public Decimal X {get; set;}
   public Decimal Y {get; set;}
}
```

notice how the properties in the class match the column names **exactly**, this is because the wrapper uses the name of the property to look up the column information and data. At the moment if a property name does not match any columns in the table an exception will be thrown, there are no safe guards in place to stop this so for now just be extra careful.

Before we can go ahead and use this in the wrapper there are a few more things we have to go over.

In the wrapper there are two inbuilt entity types, _BaseEntity_ and _MappableEntity_.

  * **BaseEntity** Is the main entity type that you **must** inherit from if you wish to use                                                       your entity objects with the Mapinfo wrapper, BaseEntity exposes a few properties which are needed by the internals of the wrapper.

> The properties it exposes are the following:
    * RowID <- Represents the row id for this record in the Mapinfo table.
    * Table <- Contains a referance to the [Table](TableType.md) object this object belongs to.
    * State <- Represents the current state of the entity.

  * **MappableEntity** Is an optional entity type that exposes the obj column for the row, MappableEntity inherits from BaseEntity so by inheriting from MappableEntity you will still get access to all the properties from BaseEntity.

> _One important thing to note_ is you do not have inherit from MappableEntity if you do not intend to use the object column for that table, eg you are just going to update the names of all the nodes in our example table.

Taking all the above information on board we can now go ahead and change our node class to look like the following:

```
using MapinfoWrapper.DataAccess.RowOperations.Entities;

public class Node : BaseEntity
{
   public Decimal Dist {get; set;}
   public int LineID {get; set;}
   public String Name {get; set;}
   public String Locality {get; set;}
   public Decimal X {get; set;}
   public Decimal Y {get; set;}
}
```

The entity is now ready to be used with the Table type in the wrapper, how to use entity with the Table type can be found at [Using entity types with tables](TableAndEntities.md).


---


## Performance Issues ##
The are two major performance issues to take note of.

**1)** As each property is loaded with data each time it is returned from a table(more information at [Using entity types with tables](TableAndEntities.md)) it is recommended to keep your entities as thin as possible so not a lot of data is needed to be queried from the table, transformed and loaded into the properties.

_An example of this would be_, if you only intended to use the _Name_ column from the nodes.tab table you would only include that property in your class:

```
using MapinfoWrapper.DataAccess.RowOperations.Entities;

public class Node : BaseEntity
{
   public String Name {get; set;}
}
```

This way the wrapper only has to get information about the name column plus the row id in BaseEntity.

**2)** The current implementation<sub>(75% chance this will change)</sub> of BaseEntity includes a backingstore which is used by the index property on BaseEntity to allow string based column access to columns for that row.

_eg_

`instanceofNode["Locality"]`

would return a string containing the data for the locality column.

Due to the above, all entities be default are loaded with a backstore that maps column name and data.  This adds a large overhead due to every column having to be evaluated regardless of the size of the entity.

However there is a work around for this; the [MapinfoSession](MapinfoSession.md) object exposes a property called _LoadOptions_ which allows you to supply rules the wrapper will use when loading the entity with data, the syntax is a follows:

```
            MapinfoSession session = MapinfoSession.CreateCOMInstance();

            EntityLoadOptions options = new EntityLoadOptions();
            options.NoBackingStore<Node>();
            session.LoadOptions = options;
```

the `NoBackingStore<Node>();` call, tells the wrapper to not load the backing store for this entity type when it is being loaded.  You can replace _Node_ with the type of you entity.

Now when entity objects are loaded only the properties that are supplied in the type plus those in BaseEntity will be loaded and not the backingstore.

This workaround will increase the performance of loading entities dynamically.

I have done rough timing with and without the backing store:

**Looping though 3000 records, entity with one string property.**
  * _With backing store_ ~1 minute
  * _Without backing store_ ~8 seconds