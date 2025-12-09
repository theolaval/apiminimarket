using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniMarket.API.DTOs;
using MiniMarket.API.Mappers;
using MiniMarket.API.Services;
using MiniMarket.BLL.Services.Interfaces;
using MiniMarket.Domain.Models;
using System.Security.Claims;

namespace MiniMarket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUtilisateurService _utilisateurService;
        private readonly AuthService _authService;

        public AuthController(IUtilisateurService utilisateurService, AuthService authService)
        {
            _utilisateurService = utilisateurService;
            _authService = authService;
        }

        [HttpPost("register", Name = "Register")]
        public ActionResult Register([FromBody] AuthRegisterForm registerForm)
        {
            if (registerForm is null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            Utilisateur newUser = registerForm.ToUtilisateur();
            _utilisateurService.Create(newUser);

            return Created();
        }

        [HttpPost("login", Name = "Login")]
        public ActionResult Login([FromBody] AuthLoginForm loginForm)
        {
            if (loginForm is null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            Utilisateur utilisateur = _utilisateurService.Login(loginForm.Username, loginForm.Password);

            string token = _authService.GenerateToken(utilisateur);
            return Ok(new TokenResponse { Token = token });
        }

        // Récupération des infos du user pour l'espace client
        [Authorize]
        [HttpGet("me", Name = "GetCurrentUser")]
        public ActionResult GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized(new { message = "Token invalide" });
            }

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest(new { message = "ID utilisateur invalide" });
            }

            Utilisateur? user = _utilisateurService.GetById(userId);

            if (user == null)
            {
                return NotFound(new { message = "Utilisateur non trouvé" });
            }

            return Ok(new
            {
                user = new
                {
                    id = user.Id,
                    username = user.Username,
                    email = user.Email,
                    birthdate = user.Birthdate.ToString("yyyy-MM-dd"),
                    role = user.Role.ToString()
                }
            });
        }
    }
}