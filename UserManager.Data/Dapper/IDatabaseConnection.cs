using System.Data;

namespace UserManager.Data.Dapper
{
    public interface IDatabaseConnection
    {
        IDbConnection Connection();
    }
}