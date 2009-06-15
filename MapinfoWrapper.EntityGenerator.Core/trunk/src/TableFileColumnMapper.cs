using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MapinfoEntityGen
{
	public class TableFileColumnMapper
	{
		Dictionary<String, Type> tabmapping = new Dictionary<string, Type>() 
		{
			{"Char",typeof(string)},
			{"Decimal",typeof(decimal)}
			
		};
		
			public Dictionary<String, Type> MapColumnsAndTypes(string fileData)
			{
				Dictionary<String, Type> mapping = new Dictionary<string, Type>();
				using (StringReader reader = new StringReader(fileData))
				{
					bool infieldDef = false;
					string line;
					while ((line = reader.ReadLine()) != null)
					{
						if (line.Contains("Fields") && !infieldDef)
						{
							infieldDef = true;
							continue;
						}
						
						if (infieldDef)
						{
							string trimedstring = line.Trim();
							string[] splitstring = Regex.Split(trimedstring,@"\s+");
							Type type = this.GetTypeFromString(splitstring[1]);
							mapping.Add(splitstring[0],type);
						}
					}
				}
				return mapping;
			}
			
			public Type GetTypeFromString(string typeString)
			{
				if (tabmapping.ContainsKey(typeString)) {
				    	return tabmapping[typeString];
				}
				else
				{
					throw new ArgumentOutOfRangeException();
				}
			}
	}
}
