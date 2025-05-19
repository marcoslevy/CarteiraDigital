using CarteiraDigital.Application.Usuarios.Commands;
using CarteiraDigital.Application.Usuarios.Results;
using CarteiraDigital.Core.Entities.Usuarios;
using CarteiraDigital.Core.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarteiraDigital.Application.Usuarios.Handlers;

public class LoginHandler : IRequestHandler<LoginCommand, ResultadoOperacao<LoginResult>>
{
    private readonly SignInManager<Usuario> _signInManager;
    private readonly UserManager<Usuario> _userManager;
    private readonly IConfiguration _configuration;

    public LoginHandler(SignInManager<Usuario> signInManager, UserManager<Usuario> userManager, IConfiguration configuration)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<ResultadoOperacao<LoginResult>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(
            request.Email, request.Senha, isPersistent: false, lockoutOnFailure: false);

        if (!result.Succeeded)
            return ResultadoOperacao<LoginResult>.Falha(result.ToString());

        var user = await _userManager.FindByEmailAsync(request.Email);

        var token = GenerateJwtToken(user, null);

        var loginResult = new LoginResult()
        {
            Token = token,
            ExpiraEm = DateTime.UtcNow.AddHours(1),
            UsuarioId = user.Id,
            UsuarioEmail = user.Email
        };

        return ResultadoOperacao<LoginResult>.SucessoResultado(loginResult);
    }

    private string GenerateJwtToken(IdentityUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["JwtSettings:ExpirationHours"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
