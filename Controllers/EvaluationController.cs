using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using SPU.Models;
using SPU.Domain;
using SPU.Domain.Entites;

namespace SPU.Controllers;

public class EvaluationController : Controller
{
    private readonly string _loggedUserId;
    private readonly SpuContext _context;

    public EvaluationController(SpuContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        var claim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        _loggedUserId = claim?.Value;
    }

    [Authorize]
    public IActionResult Index()
    {
        Utilisateur user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);

        return View(user);
    }
}
