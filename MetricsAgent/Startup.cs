using AutoMapper;
using MetricsAgent.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Data.SQLite;

namespace MetricsAgent
{
    public class Startup
    {
        private int tableSize = 10;
        private Random random = new Random();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            ConfigureSqlLiteConnection(services); 
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
            services.AddScoped<ICpuMetricsRepository, CpuMetricsRepository>();
            services.AddScoped<IDotNetMetricsRepository, DotNetMetricsRepository>();
            services.AddScoped<IHddMetricsRepository, HddMetricsRepository>();
            services.AddScoped<INetworkMetricsRepository, NetworkMetricsRepository>();
            services.AddScoped<IRamMetricsRepository, RamMetricsRepository>();
        }

        private void ConfigureSqlLiteConnection(IServiceCollection services)
        {
            string connectionString = "Data Source=metrics.db";
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            PrepareSchema(connection);
            services.AddSingleton(connection);
        }

        private void PrepareSchema(SQLiteConnection connection)
        {
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = "DROP TABLE IF EXISTS cpumetrics";
                command.ExecuteNonQuery();
                command.CommandText = "DROP TABLE IF EXISTS dotnetmetrics";
                command.ExecuteNonQuery();
                command.CommandText = "DROP TABLE IF EXISTS hddmetrics";
                command.ExecuteNonQuery();
                command.CommandText = "DROP TABLE IF EXISTS networkmetrics";
                command.ExecuteNonQuery();
                command.CommandText = "DROP TABLE IF EXISTS rammetrics";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE cpumetrics(id INTEGER PRIMARY KEY,
                    value INT, time INT)";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE dotnetmetrics(id INTEGER PRIMARY KEY,
                    value INT, time INT)";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE hddmetrics(id INTEGER PRIMARY KEY,
                    value INT, time INT)";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE networkmetrics(id INTEGER PRIMARY KEY,
                    value INT, time INT)";
                command.ExecuteNonQuery();
                command.CommandText = @"CREATE TABLE rammetrics(id INTEGER PRIMARY KEY,
                    value INT, time INT)";
                command.ExecuteNonQuery();

                for (int i = 0; i < tableSize; i++)
                {
                    command.CommandText = $"INSERT INTO cpumetrics(value, time) VALUES({random.Next(100)}, {random.Next(100)})";
                    command.ExecuteNonQuery();
                    command.CommandText = $"INSERT INTO dotnetmetrics(value, time) VALUES({random.Next(100)}, {random.Next(100)})";
                    command.ExecuteNonQuery();
                    command.CommandText = $"INSERT INTO hddmetrics(value, time) VALUES({random.Next(100)}, {random.Next(100)})";
                    command.ExecuteNonQuery();
                    command.CommandText = $"INSERT INTO networkmetrics(value, time) VALUES({random.Next(100)}, {random.Next(100)})";
                    command.ExecuteNonQuery();
                    command.CommandText = $"INSERT INTO rammetrics(value, time) VALUES({random.Next(100)}, {random.Next(100)})";
                    command.ExecuteNonQuery();
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}