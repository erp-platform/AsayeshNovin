namespace UMS.Authentication.Application.Dtos;

public class ResponseDto<T>
{
    public T? Data { get; set; }

    public ErrorDto? Error { get; set; }

    public double? ProcessTime { get; set; }
}