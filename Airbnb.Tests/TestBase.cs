using Airbnb.Application.Services;
using Airbnb.Domain.DataTransferObjects.User;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Infrastructure.Data;
using Airbnb.Infrastructure.Repositories;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;

namespace Airbnb.Tests
{
    public class TestBase : IDisposable
    {
        private readonly string _dbName;
        protected readonly DbContextOptions<AirbnbDbContext> _dbContextOptions;
        protected readonly ServiceProvider _serviceProvider;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly WebApplicationBuilder _builder;

        public TestBase()
        {
            // Create New Dat

            _dbContextOptions = new DbContextOptionsBuilder<AirbnbDbContext>()
            .UseInMemoryDatabase(_dbName = Guid.NewGuid().ToString()).Options;

            var services = new ServiceCollection();

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

            var mockMediator = new Mock<IMediator>();

            _builder = WebApplication.CreateBuilder();
            _builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            AddServices(services, mockWebHostEnvironment, _builder.Configuration, mockMediator);

            _serviceProvider = services.BuildServiceProvider();
            _scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();

            InitializeDatabase().GetAwaiter();

        }
        private void AddServices(ServiceCollection services, Mock<IWebHostEnvironment> mockWebHostEnvironment, IConfiguration configuration, Mock<IMediator> mockMediator)
        {
            services.AddIdentity<AppUser, IdentityRole>()
               .AddEntityFrameworkStores<AirbnbDbContext>()
               .AddSignInManager<SignInManager<AppUser>>()
               .AddDefaultTokenProviders();

            var context = new Mock<IHttpContextAccessor>();
            context.SetupGet(x => x.HttpContext)
                            .Returns(new DefaultHttpContext());

            services.AddDbContext<AirbnbDbContext>(options =>
            {
                options.UseInMemoryDatabase(_dbName);
            });

            // what do?
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetAssembly(new Application.Mapester.BookingMap().GetType())!);
            services.AddSingleton(config);
            // services.AddScoped<IMapper, ServiceMapper>(); why this is used in ICPC Test Base
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IReviewService, ReviewServices>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMemoryCache();


        }
        public Mock<SignInManager<AppUser>> GetMockSignInManager()
        {
            var mockUserStore = Mock.Of<IUserStore<AppUser>>();
            var userManagerMock = new Mock<UserManager<AppUser>>(mockUserStore, null, null, null, null, null, null, null, null);

            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
            var optionsMock = new Mock<IOptions<IdentityOptions>>();
            var loggerMock = new Mock<ILogger<SignInManager<AppUser>>>();
            var schemesMock = new Mock<IAuthenticationSchemeProvider>();
            var confirmationMock = new Mock<IUserConfirmation<AppUser>>();

            var MockSignInManager = new Mock<SignInManager<AppUser>>(
                userManagerMock.Object,
                contextAccessorMock.Object,
                userPrincipalFactoryMock.Object,
                optionsMock.Object,
                loggerMock.Object,
                schemesMock.Object,
                confirmationMock.Object
                );
            return MockSignInManager;
        }
        public Mock<IHttpContextAccessor> GetMockHttpContextAccessor(string userId)
        {
            var identity = new GenericIdentity("Account");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
            var httpContext = new DefaultHttpContext();
            httpContext.User.AddIdentity(identity);
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            return httpContextAccessor;
        }
        public SignInManager<AppUser> GetSignInManager()
        {
            return _serviceProvider.GetRequiredService<SignInManager<AppUser>>();
        }
        public IUnitOfWork GetUnitOfWork()
        {
            return _serviceProvider.GetRequiredService<IUnitOfWork>();
        }
        public UserManager<AppUser> GetUserManager()
        {
            return _serviceProvider.GetRequiredService<UserManager<AppUser>>();
        }
        public RoleManager<IdentityRole> GetRoleManager()
        {
            return _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        }
        public IAuthService GetAuthService()
        {
            return _serviceProvider.GetRequiredService<IAuthService>();
        }

        public void Dispose()
        {
            var context = _serviceProvider.GetRequiredService<AirbnbDbContext>();
            context.Database.EnsureDeleted();
        }
        private async Task InitializeDatabase()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AirbnbDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await context.Database.EnsureCreatedAsync();
                var res = await roleManager.CreateAsync(new IdentityRole(Role.Customer.ToString()) { NormalizedName = Role.Customer.ToString().ToUpper() });
                await roleManager.CreateAsync(new IdentityRole(Role.Owner.ToString()) { NormalizedName = Role.Owner.ToString().ToUpper() });
                await roleManager.CreateAsync(new IdentityRole("Admin") { NormalizedName = "Admin".ToUpper() });
                var data = await roleManager.Roles.ToListAsync();
            }
        }
    }

}
