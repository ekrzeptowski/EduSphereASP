using EduSphere.Domain;
using EduSphere.Domain.Constants;
using EduSphere.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EduSphere.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger,
        ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var roles = new[] { Roles.Administrator, Roles.Teacher, Roles.Student };

        foreach (var role in roles)
        {
            await CreateRoleIfNotExists(role);
        }

        async Task CreateRoleIfNotExists(string roleName)
        {
            if (_roleManager.Roles.All(r => r.Name != roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Default users
        var users = new[]
        {
            new
            {
                User = new ApplicationUser
                {
                    UserName = "administrator@localhost", Email = "administrator@localhost"
                },
                Role = Roles.Administrator,
                Password = "Administrator1!"
            },
            new
            {
                User = new ApplicationUser { UserName = "teacher@localhost", Email = "teacher@localhost" },
                Role = Roles.Teacher,
                Password = "Teacher1!"
            },
            new
            {
                User = new ApplicationUser { UserName = "student@localhost", Email = "student@localhost" },
                Role = Roles.Student,
                Password = "Student1!"
            }
        };

        foreach (var user in users)
        {
            if (_userManager.Users.All(u => u.UserName != user.User.UserName))
            {
                await _userManager.CreateAsync(user.User, user.Password);
                await _userManager.AddToRoleAsync(user.User, user.Role);
            }
        }

        // Default data
        // Seed, if necessary
        if (!_context.Courses.Any())
        {
            var courses = new[]
            {
                new Course
                {
                    Title = "Kurs 1",
                    Description = "Opis kursu nr 1",
                    Lessons =
                    {
                        new Lesson
                        {
                            Title = "Lekcja 1",
                            Content =
                                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Massa vitae tortor condimentum lacinia quis vel eros donec. Platea dictumst quisque sagittis purus sit amet volutpat. Id cursus metus aliquam eleifend mi in nulla posuere sollicitudin. Enim blandit volutpat maecenas volutpat. Sodales neque sodales ut etiam sit. Enim blandit volutpat maecenas volutpat blandit. Sollicitudin tempor id eu nisl nunc mi ipsum. Ut venenatis tellus in metus vulputate eu scelerisque. Non nisi est sit amet. Habitant morbi tristique senectus et. Diam phasellus vestibulum lorem sed risus. Pretium aenean pharetra magna ac placerat vestibulum. Non consectetur a erat nam at. Adipiscing bibendum est ultricies integer quis auctor. Aenean pharetra magna ac placerat vestibulum."
                        },
                        new Lesson
                        {
                            Title = "Lekcja 2",
                            Content =
                                "Habitant morbi tristique senectus et netus et malesuada fames ac. Tempus iaculis urna id volutpat. Commodo elit at imperdiet dui accumsan sit amet nulla facilisi. Ut etiam sit amet nisl purus in. Morbi blandit cursus risus at. Amet nulla facilisi morbi tempus iaculis urna. A erat nam at lectus urna duis. Tincidunt ornare massa eget egestas purus viverra accumsan. Non diam phasellus vestibulum lorem sed risus ultricies. Lectus magna fringilla urna porttitor rhoncus dolor purus non. Tincidunt tortor aliquam nulla facilisi cras. Sed viverra tellus in hac habitasse."
                        },
                        new Lesson
                        {
                            Title = "Lekcja 3",
                            Content =
                                "Odio morbi quis commodo odio aenean. Sodales ut etiam sit amet nisl. Eget gravida cum sociis natoque penatibus et magnis dis. Vitae elementum curabitur vitae nunc sed velit dignissim. Egestas tellus rutrum tellus pellentesque eu tincidunt. Eget sit amet tellus cras adipiscing enim. Mi ipsum faucibus vitae aliquet nec ullamcorper. At volutpat diam ut venenatis tellus. Rhoncus urna neque viverra justo nec. Vitae congue eu consequat ac felis donec et odio. Sed risus ultricies tristique nulla aliquet. Vitae sapien pellentesque habitant morbi tristique senectus et netus."
                        }
                    }
                },
                new Course
                {
                    Title = "Kurs 2",
                    Description = "Opis kursu nr 2",
                    Lessons =
                    {
                        new Lesson
                        {
                            Title = "Lesson 1",
                            Content =
                                "Sed egestas egestas fringilla phasellus faucibus scelerisque eleifend donec pretium. Quisque non tellus orci ac. Lobortis mattis aliquam faucibus purus in massa. Mauris cursus mattis molestie a iaculis at erat pellentesque. Pretium fusce id velit ut tortor pretium viverra suspendisse. Interdum velit euismod in pellentesque massa placerat duis. Egestas dui id ornare arcu odio ut sem. Mauris pellentesque pulvinar pellentesque habitant morbi tristique senectus. Nunc lobortis mattis aliquam faucibus purus. Erat nam at lectus urna duis convallis. Massa eget egestas purus viverra accumsan in nisl nisi."
                        },
                        new Lesson
                        {
                            Title = "Lesson 2",
                            Content =
                                "Massa tincidunt nunc pulvinar sapien et. Cursus vitae congue mauris rhoncus aenean vel. Amet luctus venenatis lectus magna fringilla urna porttitor rhoncus. Ullamcorper malesuada proin libero nunc consequat interdum varius sit amet. Commodo elit at imperdiet dui. Feugiat sed lectus vestibulum mattis ullamcorper. Neque gravida in fermentum et sollicitudin ac orci phasellus. Vulputate eu scelerisque felis imperdiet proin fermentum leo vel. Risus pretium quam vulputate dignissim suspendisse. Pellentesque nec nam aliquam sem et. Purus viverra accumsan in nisl nisi. Sit amet nisl purus in. Ac turpis egestas maecenas pharetra convallis posuere. Iaculis at erat pellentesque adipiscing. Volutpat sed cras ornare arcu dui vivamus. Nulla malesuada pellentesque elit eget gravida cum sociis natoque penatibus. Sagittis vitae et leo duis ut diam quam. Suspendisse ultrices gravida dictum fusce ut placerat orci nulla pellentesque. Enim facilisis gravida neque convallis a cras semper."
                        },
                        new Lesson
                        {
                            Title = "Lesson 3",
                            Content =
                                "Tortor consequat id porta nibh venenatis cras. Nulla facilisi cras fermentum odio eu feugiat pretium. Ipsum dolor sit amet consectetur adipiscing elit pellentesque habitant morbi. Et malesuada fames ac turpis egestas maecenas pharetra convallis. Mauris augue neque gravida in. Quis ipsum suspendisse ultrices gravida dictum fusce. Morbi non arcu risus quis varius quam quisque id. Sed blandit libero volutpat sed. Id venenatis a condimentum vitae. Arcu vitae elementum curabitur vitae nunc sed velit. Leo duis ut diam quam. Imperdiet proin fermentum leo vel. Ac tincidunt vitae semper quis lectus nulla at volutpat diam. Risus feugiat in ante metus dictum at tempor commodo ullamcorper. Dolor sit amet consectetur adipiscing elit duis tristique. Odio euismod lacinia at quis risus sed. Dictum varius duis at consectetur lorem donec."
                        }
                    }
                },
                new Course
                {
                    Title = "Kurs 3",
                    Description = "Opis kursu nr 3",
                }
            };

            foreach (Course course in courses)
            {
                _context.Courses.Add(course);
                _context.SaveChanges();
            }
        }
    }
}
