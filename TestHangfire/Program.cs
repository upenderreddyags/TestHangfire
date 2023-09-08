using Hangfire;
using TestHangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//hangfire
builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("OSTConnection")));
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DisplayStorageConnectionString = false,
});
RecurringJob.AddOrUpdate<TestClass>("Test Method", job => job.TestMethod(), Cron.Daily(15, 59), new RecurringJobOptions { TimeZone = TimeZoneInfo.Local, QueueName = "test", MisfireHandling = MisfireHandlingMode.Strict });


app.MapControllers();

app.Run();
