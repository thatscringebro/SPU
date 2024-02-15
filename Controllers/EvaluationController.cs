using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using SPU.Models;
using SPU.Domain;
using SPU.Domain.Entites;
using Microsoft.EntityFrameworkCore;

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

    [Authorize]
    public IActionResult Create(string lienGoogleForm, bool actif, bool estStagiaire)
    {
        Guid idEnseignant = _context.Enseignants.FirstOrDefault(x => x.UtilisateurId.ToString() == _loggedUserId).Id;
        List<Stagiaire> stagiaires = _context.Stagiaires.Where(x => x.EnseignantId == idEnseignant).ToList();
        
        // si c'est pour les stagiaires
        if(estStagiaire)
        {
          foreach (Stagiaire stag in stagiaires)
          {
              Evaluation eval = new Evaluation()
              {
                lienGoogleForm = lienGoogleForm,
                StagiaireId = stag.Id,
                MDSId = null,
                EnseignantId = idEnseignant,
                consulter = false,
                actif = actif,
                estStagiaire = true
              };

              _context.Evaluations.Add(eval);
          }

          _context.SaveChanges();
        }
        // c'est pour les mds
        else 
        {
          List<MDS> mds = (from stag in stagiaires
                 join md in _context.MDS on stag.Id equals md.StagiaireId
                 select md).ToList();

          foreach (MDS ms in mds)
          {
              Evaluation eval = new Evaluation()
              {
                lienGoogleForm = lienGoogleForm,
                MDSId = ms.Id,
                StagiaireId = null,
                EnseignantId = idEnseignant,
                consulter = false,
                actif = actif,
                estStagiaire = false
              };

              _context.Evaluations.Add(eval);
          }

          _context.SaveChanges();
        }

        return Ok();
    }

    [Authorize]
    public IActionResult Delete(string lien) 
    {
      List<Evaluation> evals = _context.Evaluations.Where(x => x.lienGoogleForm == lien).ToList();

      _context.Evaluations.RemoveRange(evals);

      _context.SaveChanges();

      return Ok();
    }

    [Authorize]
    public IActionResult setActif(Guid idEval, bool status)
    {
      Evaluation eval = _context.Evaluations.FirstOrDefault(x => x.Id == idEval);

      eval.actif = status;

      _context.SaveChanges();

      return Ok();
    }

    [Authorize]
    public IActionResult setConsultation(Guid idEval, bool status)
    {
      Console.WriteLine(idEval);
      Evaluation eval = _context.Evaluations.FirstOrDefault(x => x.Id == idEval);

      eval.consulter = status;

      _context.SaveChanges();

      return Ok();
    }

    [Authorize]
    public IActionResult GetEvaluationsByURL(string url)
    {
      List<Evaluation> evals = _context.Evaluations.Include(x => x.mds).ThenInclude(x => x.utilisateur).Include(x => x.stagiaire).ThenInclude(x => x.utilisateur).Where(x => x.lienGoogleForm == url).ToList();

      return Ok(Json(new { data = evals }));
    }
}
