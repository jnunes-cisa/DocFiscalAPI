namespace Repositories.Query
{

    public class SortQuery
    {

        public SortQuery(SortDirection direction, string sortedAttribute)
        {

            Direction = direction;
            SortedAttribute = sortedAttribute;

        }

        public SortDirection Direction { get; set; }

        public string SortedAttribute { get; set; }

    }

}