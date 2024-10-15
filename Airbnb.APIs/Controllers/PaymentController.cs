using MediatR;

namespace Airbnb.APIs.Controllers
{
	public class PaymentController:APIBaseController
	{
		private readonly IMediator _mediator;

		public PaymentController(IMediator mediator)
		{
			_mediator = mediator;
		}

		//[http]

	}
}
