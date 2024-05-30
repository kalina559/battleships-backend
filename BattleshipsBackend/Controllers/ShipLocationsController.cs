using Battleships.Services;
using Battleships.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class ShipLocationsController : ControllerBase
{
    private readonly IShipLocationService _shipLocationService;

    public ShipLocationsController(IShipLocationService shipLocationService)
    {
        _shipLocationService = shipLocationService;
    }

    [HttpGet("opponent")]
    public ActionResult<List<Ship>> GetOpponentShips()
    {
        var ships = _shipLocationService.GenerateOpponentShips();
        return Ok(ships);
    }
}