using Moq;
using Lab1_1.Contracts;
using Lab1_1.Data.Model;
namespace Lab1_1.Test
{
    internal static class MockRepo
    {
        public static Mock<IRepository<N018Dictionary>> SetupMock()
        {
            return new Mock<IRepository<N018Dictionary>>();
        }
    }
}
