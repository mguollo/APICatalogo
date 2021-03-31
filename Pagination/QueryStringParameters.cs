namespace APICatalogo.Pagination
{
    public class QueryStringParameters
    {
        const int maxPageSize = 50;
        const int minPageNumber = 1;
        private int _pageNumber = minPageNumber;
        public int PageNumber 
        { 
            get
            {
                return _pageNumber;
            }

            set
            {
                _pageNumber = (value > 0) ? value : minPageNumber;
            }
        }
        private int _pageSize = 5;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
        
    }
}