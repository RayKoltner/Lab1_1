using Lab1_1.Data;
using Lab1_1.Data.Model;
using Lab1_1.Repositories;
using Lab1_1.Contracts;

namespace Lab1_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //DI
            builder.Services.AddDbContext<ApplicationContext>();
            builder.Services.AddScoped<IRepository<N018Dictionary>, N018Repository>();

            builder.Services.AddHttpClient();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
