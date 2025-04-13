namespace BuildingBlocks.Validation;

public enum ErrorType
{
    Failure = 0,
    Validation = 1,
    BadRequest = 2,
    NotFound = 3,
    Conflict = 4,
    Unauthorized = 5,
}