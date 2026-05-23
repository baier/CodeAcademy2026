using System.ComponentModel.DataAnnotations;

public record CreateOrderRequest(
	[param: Required]
	Guid CoffeeId,

	[param: Required]
	Size size,

	[param: Required]
	Milktype milkType,

	bool? ExtraShot,

	[param: Required]
    [param: RegularExpression(@"^(?!\s*$).+", ErrorMessage = "CustomerName cannot be blank.")]
	[param: StringLength(50, MinimumLength = 2)]
	string CustomerName,

	[param: Range(1, 10)]
	int Quantity
);
