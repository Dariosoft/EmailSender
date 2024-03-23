namespace Dariosoft.Framework.Types
{
    public record ListQueryModel
    {
        private int page = 1, pageSize = 15;

        public string? Query { get; set; }

        public int Page
        {
            get => page;
            set => page = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value < 1 ? 1 : value > 200 ? 200 : value;
        }

        public string? SortBy { get; set; }

        public bool DescendingSort { get; set; }

        public IDictionary<string, string>? Parameters { get; set; }
    }
}
