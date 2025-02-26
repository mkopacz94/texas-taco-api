namespace TexasTaco.Shared.Pagination
{
    public sealed class PagedResult<T>(
        IEnumerable<T> items,
        int totalCount,
        int pageSize,
        int currentPage)
    {
        public IEnumerable<T> Items { get; } = items;
        public int TotalCount { get; } = totalCount;
        public int PageSize { get; } = pageSize;
        public int CurrentPage { get; } = currentPage;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
