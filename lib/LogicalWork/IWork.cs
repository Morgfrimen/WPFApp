using LogicalWork;

namespace LoadData
{
    public interface IWork
    {
        ViewTableData GetResult();
        ViewTableData GetResultAsync();
    }
}