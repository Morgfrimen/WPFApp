namespace LogicalWork
{
    public interface IWork
    {
        ViewTableData GetResult();
        ViewTableData GetResultAsync();
    }
}