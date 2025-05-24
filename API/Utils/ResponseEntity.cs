namespace API.Utils;

public class ResponseEntity<T>
{
    public string Status { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}