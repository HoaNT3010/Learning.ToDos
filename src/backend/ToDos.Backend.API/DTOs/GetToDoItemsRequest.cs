using ToDos.Backend.API.DTOs.Common;

namespace ToDos.Backend.API.DTOs;

public sealed class GetToDoItemsRequest : QueryParameters
{
    public string? Keyword { get; set; }
}
