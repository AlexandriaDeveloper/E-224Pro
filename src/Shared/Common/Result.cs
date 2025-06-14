namespace Shared.Common;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }
    public string? ErrorMessage { get; private set; }
    public List<string> Errors { get; private set; } = new();

    private Result(bool isSuccess, T? data, string? errorMessage)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Success(T data) => new(true, data, null);
    public static Result<T> Failure(string error) => new(false, default, error);
    public static Result<T> Failure(List<string> errors) => new(false, default, null) { Errors = errors };
}

public class PaginatedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }



    public PaginatedResult(List<T> Items, int PageIndex = 0, int PageSize = 30, int TotalCount = 0)
    {
        this.Items = Items;
        this.PageIndex = PageIndex;
        this.PageSize = PageSize;
        this.TotalCount = TotalCount;

    }
    public static PaginatedResult<T> Create(List<T> Items, int? CurrentPage, int? PageSize, int? TotalCount = 0)
    {
        return new PaginatedResult<T>(Items, CurrentPage ?? 0, PageSize ?? 30, TotalCount ?? 0);
    }


}