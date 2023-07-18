# Welcome to my client-server web application!

## In this readme file, I provide you with some information about the frontend side of my application.

I hope it will help you truly appreciate the executed work. I really invest a piece of yourself in this project, and I hope you will really enjoy it. :) So, let's start with an introduction!

I've decided to develop a web tamagotchi application (so it was a good task from the Innowise team) where you can store and manage your virtual pets. During the process of completing this task, I've improved my knowledge in .NET and MVC technologies. Let's talk a little bit about the technology stack I used in my application.

## Technology Stack

So, the project has been written on .NET 6 using the ASP .NET MVC project type.

![Project Type](/Images/mvc.jpg)

Also, I used IHttpClientFactory to make http call to backend side: 
```csharp
[Route("user")]
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;
        public UserController(IHttpClientFactory factory, ITokenService tokenService)
        {
            _httpClient = factory.CreateClient("Client");
            _tokenService = tokenService;
        }

        [Route("collaborators")]
        public async Task<IActionResult> GetMyColaborators()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);

            HttpResponseMessage response = await _httpClient.GetAsync($"api/user/collaborators");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }

            List<UserDto>? collaborators = JsonSerializer.Deserialize<List<UserDto>>(response.Content.ReadAsStringAsync().Result);

            return View("Collaborators", collaborators);
        }
        //Other endpoints
     }
```

Then I added model validation using Fluent Validation:
```csharp
 public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserName).NotNull();
            RuleFor(x => x.FirstName).NotNull();
            RuleFor(x => x.LastName).NotNull();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }

    }
```
It's a good way to validate model befor sending to backend side. After model validation we must authorize in aplication. So, for this purpose I used JWT security token. After authorization backend side send us JWT token.I store them in cookie files.
 ```csharp
        public async Task<ActionResult> LogIn(UserDto dto)
        {
            UserValidator validator = new UserValidator();

            if (!validator.Validate(dto).IsValid)
            {
                return View("Index", dto);
            }

            JsonContent content = JsonContent.Create(dto);

            HttpResponseMessage response = await _httpClient.PostAsync("api/authorization/login", content);

            if (!response.IsSuccessStatusCode)
            {
                string error = await response.Content.ReadAsStringAsync();
                
                ViewBag.Message = error;

                return View("Index", dto);
            }

            _tokenService.AddTokenToCookie(await response.Content.ReadAsStringAsync(),HttpContext,"token",1);

            return RedirectToAction("personal-info", "account");
        }
```
A little bit boring, right? So, let's finish our theoretical part and moove to practice. Now I provide you how my frontend part of application looks like. I show you all functionality of my application. Let's start!

## Practice

In my frontend part I used Razor pages, CSS and a little bit JS. So, we gonna start with Home Page of my application.

![Project Type](/Images/homepage.jpg)

So, dinozaur move from left to right side due to JS! I hope looks good. After getting a home page we can choose: log in or registrate.

After pressing registrate button we have simple form.

![Project Type](/Images/registration.jpg)

On this Image you can see two bloks. Left side appeared after pressing button "Registration". Right side - registration form after pressing button "Create" without filling information. So, after adding model validation it's impossible to send to the server empty model.

After filling all information and pressing create button you are registered in application! Now you can log in. For log in you need email and password you've used in registration form.

![Project Type](/Images/logIn.jpg)