namespace MapinfoWrapper.DataAccess.LINQ.SQL
{
    class UpdateExpression
    {

        public UpdateExpression(string tableName)
        {
            this.TableName = tableName;
        }

        public string TableName { get; private set; }

    }
}
