# Welcome to my client-server web application!

## In this readme file, I provide you with some information about the frontend side of my application.

I hope it will help you to truly appreciate the executed work.  I have put my heart and soul into this project, and I hope you will really enjoy it. :) Not a moment to lose, let's start with an introduction!

I've decided to develop a web tamagotchi application (Thanks Innowise team for this fascinating assignment) where you can store and manage your virtual pets. During the process of completing this task, I've improved my knowledge in .NET and MVC technologies. Let's talk a little bit about the technology stack I used in my application.

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
It's a good way to validate model befor sending to backend side. After model validation we must authorize in application. So, for this purpose I used JWT security token. After authorization backend side send us JWT token.I keep in cookie files.
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
A little bit boring, right? So, let's finish our theoretical part and move to practice. Now I provide you how my frontend part of application looks like. I will show you all functionality of my application. Let's get down!

## Practice

In my front end part I used Razor pages, CSS and a little bit JS. So, we are going to start with Home Page of my application.

![Project Type](/Images/homepage.jpg)

So, dinozaur move from left to right side due to JS! Looks great, doesn't it? After navigating a home page we can choose: log in or registration.

After pressing registrate button we have simple form.

![Project Type](/Images/registration.jpg)

On this Image you can see two bloñks. Left blok appeared after pressing button "Registration". Right bloñk - registration form after pressing button "Create" without filling information. So, after adding model validation it's impossible to send to the server empty model.

After filling all information and pressing create button you are registered in application! Now you can log in. For log in you need email and password you've used in registration form.

![Project Type](/Images/logIn.jpg)

After pressing log in button you authorized in application. Congratulations! Automatically you will redirect to personal-info page.

![Project Type](/Images/personalinfo.jpg)

Here you can find all your personal information. As you can see, you have "Edit Profile" button. So, if you fill your information wrong, you can change it after registration.

![Project Type](/Images/personalinfo2.jpg)

So, you can change all information except your email. After pressing "Update" button your information will update. Also you can change your password, after pressing "Change password" button.

![Project Type](/Images/changepassword.jpg)

I hope you are enjoying our small trip to my project. But it was only beginning :). Let's show you some gameplay and business logic of my application.

Let's start with "Farms Overview" section. After pressing "Farms Overview" button you can see two blocks: "My own farm" with all information about your farm or "Farms, where i'm collaborator".

### Farms Overview

![Project Type](/Images/farmoverview.jpg)

In "my own farm" section you can see different pages. If you have only created your account and haven't got you pet farm, application offer you create your own farm.

![Project Type](/Images/createfarm.jpg)

Farm name should be unique during all application. After creating farm you will see Farm overview page with information related to your farm.

![Project Type](/Images/farm.jpg)

Here you can see two buttons: "Create pet" and "My pets". After pressing "Create pet" button you will go to pet constructor, where you should choose all parts of your pet: body, nose, eyes etc. Don't forget to choose pet name that should be unique during all application.

![Project Type](/Images/contractor.jpg)

After choosing all pet components you can see how your pet looks like:

![Project Type](/Images/amebaa.jpg)

As you can see, it's mandatory to choose pet's name. After creating a pet, you can see all you pets in pet-list page.

![Project Type](/Images/petlist.jpg)

Here you can Feed or Give a drink to your pet. After pressing "Feed" or "Give a drink" button your pet status will update:

![Project Type](/Images/petlist2.jpg)

Hungry and Thirsty level changed from "Normal" to "Full". Also you can see your pet after pressing "View" button in a pet list.

Now get back to "Farms overview page" and go to "Farms, where i'm collaborator".

![Project Type](/Images/imcollab.jpg)

As you can see, hear you can find list of users, added you to collaborators. As can be seen, only one user added me to collaborators. Now, I can see his farm overview and his pet list.

![Project Type](/Images/foreignpetlist.jpg)

Also I can Feed or Give a drink to his pet. I can't create or manage his pet. Only give a drink or feed. And with view button you can see his pets.

Now, moving to "Farm Detail" section of my application.

### Farm Detail

Here you can find three blocks: "Invite friend", "My collaborators" and "Farm statistics".

![Project Type](/Images/farmsdetailpage.jpg)

Let's show you one by one all functionality in farms detail section. First one is "Invite friend" section.

![Project Type](/Images/invitefriend.jpg)

On this page you can invite users to your collaborators. After adding, collaborators can feed or give a drink to your pets. At the top of the page you can find search row, where you can write users name.All your collaborators you can find in "My collaborators" section.

![Project Type](/Images/collab.jpg)

As you can see, you can remove any user from the collaborators list.

Now let's show you third part of "Farm Detail" - Farm statistic.

![Project Type](/Images/statistic.jpg)

Here you can see two blocks. Pie chart of alive/dead pets and other statistics. In other statistics you can find: Average happy days count,average pets age and others. Looks good, right? :) Now move to last, but not least section. 

### All innogotchi list

In this section you can find all pets across all application.

![Project Type](/Images/listpage.jpg)

At the top of page you can see list of filters, you can use for sorting all pets. By default it sorts by happy days count.
## Conclusion

So, it was a (not)small description of client side of my application. Please, keep in mind, that I just make my first steps in software development and I hope you have enjoyed this small trip. :) I will be glad to receive any advice and suggestions that can make my code better. Thanks for your attention!