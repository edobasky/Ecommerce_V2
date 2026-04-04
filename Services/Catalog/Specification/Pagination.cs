namespace Catalog.Specification
{
    public class Pagination<T> where T : class
    {
        public Pagination()
        {
        }
        public Pagination(int pageIndex,int pageSize,int count, IReadOnlyCollection<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        public int PageIndex { get; }
        public int PageSize { get; }
        public int Count { get; }
        public IReadOnlyCollection<T> Data { get; }
    }
}
