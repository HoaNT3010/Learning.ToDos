using ToDos.Backend.API.DTOs.Enums;

namespace ToDos.Backend.API.DTOs.Common;

public class QueryParameters
{
    public string? SortBy { get; set; }
    public SortDirection? SortDirection { get; set; }
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; } = 20;
}
