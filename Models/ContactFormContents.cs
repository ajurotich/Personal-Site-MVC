using System.ComponentModel.DataAnnotations;

namespace MVC_Site.Models;

public class ContactFormContents {

	[Required(ErrorMessage = "Missing name.")]
	public string Name { get; set; } = null!;

	[Required(ErrorMessage = "Missing email.")]
	[DataType(DataType.EmailAddress)]
	public string Email { get; set; } = null!;

	[Required(ErrorMessage = "Missing subject.")]
	public string Subject { get; set; } = null!;

	[Required(ErrorMessage = "Missing message.")]
	[DataType(DataType.MultilineText)]
	public string Message { get; set; } = null!;

}
