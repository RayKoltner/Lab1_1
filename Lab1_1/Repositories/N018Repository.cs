using Lab1_1.Data;
using Lab1_1.Data.Model;
using Lab1_1.Share.DTOs;

namespace Lab1_1.Repositories
{
    public class N018Repository<T> : GenericRepository<T> where T : N018Dictionary
    {
        public N018Repository(ApplicationContext contextFactory) : base(contextFactory)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}
