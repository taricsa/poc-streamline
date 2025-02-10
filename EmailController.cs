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
