using Microsoft.EntityFrameworkCore;
using stackApi.Data;
namespace stackApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            

            builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<FlashDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FlashDbConnectionString")));

            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); //needed, so API is not restricted and hinder other applications from using it!!!
            app.UseAuthorization();



            app.MapControllers();

            app.Run();
        }
    }
}
