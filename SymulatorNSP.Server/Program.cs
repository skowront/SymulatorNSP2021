
using SymulatorNSP.Core.Converters;
using SymulatorNSP.Server.Models;
using SymulatorNSP.Server.Services.Implementations;
using SymulatorNSP.Server.Services.Interfaces;

namespace SymulatorNSP.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoStoreDatabase"));

            // Add services to the container.

            //builder.Services.AddControllers().AddJsonOptions(opts => { opts.JsonSerializerOptions.Converters.Add(new JsonTimeSpanConverter()); });
            builder.Services.AddControllers().AddNewtonsoftJson();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<ILeaderboardService, MongoLeaderboardService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(cors => cors
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
            );

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}