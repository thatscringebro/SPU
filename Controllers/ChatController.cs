using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SPU.Models;
using SPU.Domain;
using SPU.Domain.Entites;

namespace SPU.Controllers;

public class ChatController : Controller
{
    private readonly string _loggedUserId;
    private readonly SpuContext _context;
    private UserManager<Utilisateur> _userManager;

    public ChatController(SpuContext context, UserManager<Utilisateur> userManager)
    {
        _context = context;
        _userManager = userManager;
        _loggedUserId = _userManager.GetUserId(User);
    }

    public IActionResult Index()
    {
        Utilisateur user = _context.Utilisateurs.FirstOrDefault(x => x.Id.ToString() == _loggedUserId);

        return View(user);
    }
}
