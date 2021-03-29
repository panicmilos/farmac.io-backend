using AutoMapper;
using Farmacio_API.Contracts.Requests.Reservations;
using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Farmacio_API.Controllers
{
    [ApiController]
    [Route("reservations")]
    [Produces("application/json")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;

        public ReservationsController(IReservationService reservationService, IMapper mapper)
        {
            _reservationService = reservationService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult CreateReservation(CreateReservationRequest request)
        {
            var reservation = _mapper.Map<Reservation>(request);
            _reservationService.Create(reservation);

            return Ok();
        }
    }
}