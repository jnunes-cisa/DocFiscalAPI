namespace Repositories.Query
{

    public class FilterQuery
    {

        public FilterQuery(string key, string value, FilterOperations operation)
        {

            Key = key;
            Value = value;
            Operation = operation;

        }
        
        public string Key { get; set; }

        public string Value { get; set; }

        public FilterOperations Operation { get; set; }

        public bool IsAttributeOfRelationship => Key.Contains(".");

    }

}