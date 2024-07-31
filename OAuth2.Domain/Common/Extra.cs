namespace OAuth2.Domain.Common
{
    public class Extra
    {
        public int? CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int? PageSize { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public Extra(int count = 0, int? currentPage = 1, int? pageSize = 10)
        {
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages == 0 ? 1 : totalPages;
            TotalCount = count;
        }
    }
}
