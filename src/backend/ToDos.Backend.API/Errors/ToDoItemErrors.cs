using ErrorOr;

namespace ToDos.Backend.API.Errors;

public static class ToDoItemErrors
{
    public static readonly Error ItemNotFound = Error.NotFound(
        "ToDoItem.ItemNotFound",
        "Cannot find to do item with the given identifier."
    );

    public static readonly Error InvalidIdentifier = Error.Validation(
        "ToDoItem.InvalidIdentifier",
        "Invalid to do item identifier."
    );

    public static readonly Error CannotMarkAsCompleted = Error.Validation(
        "ToDoItem.CannotMarkAsCompleted",
        "Item does not satisfy the criteria to be marked as completed."
    );
}
