using Microsoft.AspNetCore.Mvc;

namespace MVC_Site.Models;

public class CredentialSettings : Controller {

	public Email email { get; set; } = null!;

}

public class Email {

	public string Client { get; set; } = null!;
	public string Username { get; set; } = null!;
	public string Password { get; set; } = null!;
	public string Recipient { get; set; } = null!;

}