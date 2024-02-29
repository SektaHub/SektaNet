namespace backend.Models.Common
{
    public class PaginatedResponseDto<TDto>
    {
        public IEnumerable<TDto> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
