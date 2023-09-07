using Service;
using Service.Stores;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddTransient<PlannerSettings>()
            .AddSingleton<MongoDbContext>()
            .AddSingleton<IPlannedTaskStore, MongoDbPlannedTaskStore>()
            .AddSingleton<IPlannerUserStore, InMemoryPlannerUserStore>();

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStaticFiles();

        app.MapControllers();

        app.Run();
    }
}
