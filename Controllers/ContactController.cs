using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options; //grants access to IOptions
using MVC_Site.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace MVC_Site.Controllers;

public class ContactController : Controller {

	private readonly CredentialSettings _credSettings;

	public ContactController(IOptions<CredentialSettings> credsettings) { 
		_credSettings = credsettings.Value;
	}

	public IActionResult ContactForm() => View();

	[HttpPost]
	public IActionResult ContactForm(ContactFormContents cfc) {

		if(!ModelState.IsValid) return View(cfc);

		//ViewBag.ContactStateColor = "text-warning";
		ViewBag.ContactState = "Processing...";

		//Create the format for the message content we will receive from the contact form
		string message =
			$"You have received a new email from your site's contact form!<br/>" +
			$"Sender: {cfc.Name}<br/>" +
			$"Email: {cfc.Email}<br/>" +
			$"Subject: {cfc.Subject}<br/>" +
			$"Message: {cfc.Message}";


		//Create a MimeMessage object to assist with storing/transporting the email
		//information from the contact form.
		MimeMessage mm = new MimeMessage();

		//Even though the user is the one attempting to send a message to us, the actual sender 
		//of the email is the email user we set up with our hosting provider.

		//We can access the credentials for this email user from our appsettings.json file as shown below.
		mm.From.Add(new MailboxAddress("Sender", _credSettings.email.Username));

		//The recipient of this email will be our personal email address, also stored in appsettings.json.
		mm.To.Add(new MailboxAddress("Personal", _credSettings.email.Recipient));

		//The subject will be the one provided by the user, which we stored in our cfc object.
		mm.Subject = cfc.Subject;

		//The body of the message will be formatted with the string we created above.
		mm.Body = new TextPart("HTML") { Text = message };

		//We can set the priority of the message as "urgent" so it will be flagged in our email client.
		mm.Priority = MessagePriority.Urgent;

		//We can also add the user's provided email address to the list of ReplyTo addresses
		//so our replies can be sent directly to them instead of the email user on our hosting provider.
		mm.ReplyTo.Add(new MailboxAddress("User", cfc.Email));


		using(SmtpClient client = new SmtpClient()) {
			//It's possible the mail server may be down when the user attmpts to contact us,
			// so we can "encapsulate" our code to send the message in a try/catch
			try {
				//Connect to the mail server using credentials in our appsettings.json & port 8889
				client.Connect(_credSettings.email.Client, 8889);

				//Log in to the mail server using the credentials for our email user
				client.Authenticate(_credSettings.email.Username, _credSettings.email.Password);

				//Try to send the email
				client.Send(mm);
			}
			catch(Exception ex) {
				//If there is an issue, we can stroe an error message in a ViewBag variable
				// to be displayed in the View
				ViewBag.ContactStateColor = "text-danger";
				ViewBag.ContactState = "An error occured when attempting to process your email request.\n" +
					$"Error Message: {ex.Message}"
					+ $"\nError Location: {ex.StackTrace}";

				//Return user back to contact page with form intact
				return View(cfc);
			}

		}
		ViewBag.ContactStateColor = "text-success";
		ViewBag.ContactState = "Your message has been sent. Thank you!";

		return View();
	}


}
