# poc-streamline-sendgrid


# Email Sender POC

This project is a Proof of Concept (POC) for sending predefined emails using .NET 8 for the backend and Blazor for the frontend, integrated with SendGrid for email delivery.

## Tech Stack

- **Backend**: .NET 8
- **Frontend**: Blazor (Server or WebAssembly)
- **Email Service**: SendGrid

## User Journey

1. **Access Page**: User accesses the page to send a predefined email.
2. **Form Fields**: The page will have the following fields:
   - Claim Number
   - To (email)
   - PHN
   - Dropdown to select the template to be sent
   - Send button
3. **Send Email**: On clicking the send button, the information will be sent to SendGrid.
4. **Confirmation**: Once SendGrid confirms, the user will be redirected to a confirmation page.

## Setup Instructions

### Backend (.NET 8)

1. **Create a new .NET 8 project**:
   ```bash
   dotnet new webapi -n EmailSenderAPI
   cd EmailSenderAPI
   ```

2. **Install SendGrid NuGet package**:
   ```bash
   dotnet add package SendGrid
   ```

3. **Configure SendGrid in `appsettings.json`**:
   ```json
   {
     "SendGrid": {
       "ApiKey": "YOUR_SENDGRID_API_KEY"
     }
   }
   ```

4. **Create `EmailService.cs` to send emails**:
   ```csharp
   using SendGrid;
   using SendGrid.Helpers.Mail;
   using Microsoft.Extensions.Configuration;

   public class EmailService
   {
       private readonly IConfiguration _configuration;

       public EmailService(IConfiguration configuration)
       {
           _configuration = configuration;
       }

       public async Task SendEmailAsync(string toEmail, string templateId, Dictionary<string, string> templateData)
       {
           var client = new SendGridClient(_configuration["SendGrid:ApiKey"]);
           var from = new EmailAddress("your-email@example.com", "Your Name");
           var to = new EmailAddress(toEmail);
           var msg = MailHelper.CreateSingleTemplateEmail(from, to, templateId, templateData);
           var response = await client.SendEmailAsync(msg);
       }
   }
   ```

5. **Create `EmailController.cs` to handle email sending**:
   ```csharp
   using Microsoft.AspNetCore.Mvc;

   [ApiController]
   [Route("api/[controller]")]
   public class EmailController : ControllerBase
   {
       private readonly EmailService _emailService;

       public EmailController(EmailService emailService)
       {
           _emailService = emailService;
       }

       [HttpPost("send")]
       public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
       {
           await _emailService.SendEmailAsync(request.To, request.TemplateId, request.TemplateData);
           return Ok(new { message = "Email sent successfully" });
       }
   }

   public class EmailRequest
   {
       public string To { get; set; }
       public string TemplateId { get; set; }
       public Dictionary<string, string> TemplateData { get; set; }
   }
   ```

### Frontend (Blazor)

1. **Create a new Blazor project**:
   ```bash
   dotnet new blazorserver -n EmailSenderBlazor
   cd EmailSenderBlazor
   ```

2. **Create a form component**:
   ```bash
   dotnet new razorcomponent -n EmailForm
   ```

3. **Add form fields in `EmailForm.razor`**:
   ```razor
   @page "/email-form"
   @inject HttpClient Http

   <EditForm Model="@emailData" OnValidSubmit="@HandleValidSubmit">
     <DataAnnotationsValidator />
     <ValidationSummary />

     <div>
       <label for="claimNumber">Claim Number:</label>
       <InputText id="claimNumber" @bind-Value="emailData.ClaimNumber" />
     </div>

     <div>
       <label for="toEmail">To (email):</label>
       <InputText id="toEmail" @bind-Value="emailData.To" />
     </div>

     <div>
       <label for="phn">PHN:</label>
       <InputText id="phn" @bind-Value="emailData.PHN" />
     </div>

     <div>
       <label for="template">Template:</label>
       <InputSelect id="template" @bind-Value="emailData.TemplateId">
         @foreach (var template in templates)
         {
           <option value="@template.Id">@template.Name</option>
         }
       </InputSelect>
     </div>

     <button type="submit">Send</button>
   </EditForm>

   @code {
     private EmailData emailData = new EmailData();
     private List<Template> templates = new List<Template>
     {
       new Template { Id = "template1", Name = "Template 1" },
       new Template { Id = "template2", Name = "Template 2" }
     };

     private async Task HandleValidSubmit()
     {
       var response = await Http.PostAsJsonAsync("/api/email/send", emailData);
       if (response.IsSuccessStatusCode)
       {
         // Redirect to confirmation page
       }
     }

     public class EmailData
     {
       public string ClaimNumber { get; set; }
       public string To { get; set; }
       public string PHN { get; set; }
       public string TemplateId { get; set; }
     }

     public class Template
     {
       public string Id { get; set; }
       public string Name { get; set; }
     }
   }
   ```

4. **Add HttpClient to `Program.cs`**:
   ```csharp
   var builder = WebApplication.CreateBuilder(args);

   // Add services to the container.
   builder.Services.AddRazorComponents();
   builder.Services.AddHttpClient();

   var app = builder.Build();

   // Configure the HTTP request pipeline.
   if (!app.Environment.IsDevelopment())
   {
       app.UseExceptionHandler("/Error");
       app.UseHsts();
   }

   app.UseHttpsRedirection();
   app.UseStaticFiles();
   app.UseRouting app.MapRazorComponents<App>("/");

   app.Run();
   ```

## Running the Application

1. **Start the backend**:
   ```bash
   cd EmailSenderAPI
   dotnet run
   ```

2. **Start the frontend**:
   ```bash
   cd EmailSenderBlazor
   dotnet run
   ```

3. **Access the application**:
   Open your browser and navigate to `https://localhost:5001/email-form`.

## Conclusion

This POC demonstrates how to integrate .NET 8, Blazor, and SendGrid to send predefined emails. Feel free to extend and modify this project to suit your needs.

---

Let me know if you need any more details or adjustments!
