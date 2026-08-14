namespace ToDos.Backend.API.Models.Abstractions;

public interface IHasKey<TKey>
{
    TKey Id { get; set; }
}
