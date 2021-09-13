using Core.Auxiliar;
using Infraestrutura;
using Infraestrutura.Entidades;
using Infraestrutura.Interfaces;
using Infraestrutura.Repositorios;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Servicos.Interfaces;
using Servicos.Operadores;
using System;
using System.IO;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var caminhoSqLite = $"DataSource={Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}{Path.DirectorySeparatorChar}notfis.db";
            services.AddDbContext<ContextoNotfis>(options => options.UseSqlite(caminhoSqLite));
            
            services.AddScoped<IRepositorioIntercambio, RepositorioIntercambio>();
            services.AddScoped<IRepositorioNotaFiscal, RepositorioNotaFiscal>();
            services.AddScoped<IRepositorioNotaFiscalVolume, RepositorioNotaFiscalVolume>();
            services.AddScoped<IRepositorioNotaFiscalParticipante, RepositorioNotaFiscalParticipante>();
            services.AddScoped<IOperadorArquivo, OperadorArquivo>();
            services.AddScoped<IRepositorioCEP, RepositorioCEP>();
            services.AddScoped<NOTFIS>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ContextoNotfis>();
                context.Database.Migrate();
            }
        }
    }
}
