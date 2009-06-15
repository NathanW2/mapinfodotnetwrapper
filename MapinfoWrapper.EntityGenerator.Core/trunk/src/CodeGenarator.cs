using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.CodeDom.Compiler;
using System.CodeDom;

namespace MapinfoEntityGen
{
public class CodeGenarator
	{
		public string GenerateCode(string projectName, string tableName,
		                          Dictionary<String, Type> columnMappings)
		{
			CodeDomProvider provider = CreateProvider("C#");
    		System.CodeDom.Compiler.ICodeGenerator codeGenerator = null;
 
		    System.CodeDom.Compiler.CodeGeneratorOptions codeGeneratorOptions =
        												new CodeGeneratorOptions();
		    codeGeneratorOptions.BlankLinesBetweenMembers = true;
		    codeGeneratorOptions.BracingStyle = "C";
		    codeGeneratorOptions.IndentString = "   ";    // 3 spaces.
		    
		    codeGenerator = provider.CreateGenerator();
		    
		    System.Text.StringBuilder code = new StringBuilder();
		    System.IO.StringWriter stringWriter = new StringWriter(code);
		    
		    CodeNamespace gencode = GenerateSkeleton(projectName,tableName,columnMappings);
		    
		    codeGenerator.GenerateCodeFromNamespace (gencode,
		        stringWriter, codeGeneratorOptions);
		   
		    return stringWriter.ToString();
		}

		
		public CodeNamespace CreateNameSpaceHeaders(string entityNameSpace)
		{
			CodeNamespace returnValue = new CodeNamespace(entityNameSpace);
		    returnValue.Imports.Add(new CodeNamespaceImport("System"));
		    returnValue.Imports.Add(new CodeNamespaceImport("System.Text"));
		    returnValue.Imports.Add(new CodeNamespaceImport("MapinfoWrapper.TableOperations.RowOperations.Entities"));
		    return returnValue;
		}
		
		public CodeDomProvider CreateProvider(string Language)
		{	
   			if (Language == "VB")
    		{
        		return new  Microsoft.VisualBasic.VBCodeProvider();
    		}
    		else
    		{
     		   return new Microsoft.CSharp.CSharpCodeProvider ();
    		}	
		}
		
		public CodeTypeDeclaration CreateType (string name)
		{
		    CodeTypeDeclaration returnValue = new CodeTypeDeclaration(name);
		    returnValue.IsPartial = true;
		    returnValue.IsClass = true;
		    returnValue.BaseTypes.Add (new CodeTypeReference("BaseEntity",null));
		    return returnValue;
		}
		
		public CodeNamespace GenerateSkeleton (string projectName, string tableName,
		                                      Dictionary<String, Type> columnMappings)
		{
			string namespacestring = String.Format("{0}.Entities",projectName);
		    CodeNamespace returnvalue = CreateNameSpaceHeaders(namespacestring);
		    string typestring = String.Format("{0}Entity",tableName);
		    CodeTypeDeclaration generatedType =  CreateType(typestring);
		    returnvalue.Types.Add(generatedType);
		    
		    foreach (var column in columnMappings)
		    {
		    	string fieldName = "m" + column.Key.ToLower();
		    	generatedType.Members.Add(CreateProperty(column.Key, column.Value,fieldName));
		    	generatedType.Members.Add(new CodeMemberField(column.Value, fieldName));
		    }
		    
		    
		    return returnvalue;
		}
		
		public CodeMemberProperty CreateProperty(string Name, Type propertyType,string fieldName)
		{
		    CodeMemberProperty returnValue = new CodeMemberProperty();
		    returnValue.Attributes = MemberAttributes.Public;
		    returnValue.Name = Name;
		    returnValue.Type = new CodeTypeReference(propertyType);
		    returnValue.GetStatements.Add(
		        new CodeMethodReturnStatement(new
		    	                              CodeVariableReferenceExpression(fieldName)));
		    returnValue.SetStatements.Add(new CodeAssignStatement(
		    	new CodeVariableReferenceExpression(fieldName),
		           new CodeVariableReferenceExpression("value")));
		    return returnValue;
		}
	}
}
