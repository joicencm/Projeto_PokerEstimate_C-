using Microsoft.AspNetCore.Mvc;
using PokerEstimate.Models;
using System.Collections.Generic;
using System.Linq;

namespace PokerEstimate.Controllers
{
    public class HomeController : Controller
    {
        // Lista est�tica para armazenar todas as salas
        private static List<Sala> salas = new List<Sala>();

        // A��o para exibir a p�gina inicial
        public IActionResult Index()
        {
            return View();
        }

        // A��o para criar uma nova sala
        [HttpPost]
        public IActionResult CriarSala(string nomeCriador)
        {
            if (!string.IsNullOrEmpty(nomeCriador))
            {
                var sala = new Sala { Criador = nomeCriador };
                salas.Add(sala);  // Adiciona a sala � lista de salas
                return RedirectToAction("Painel", new { id = sala.Id });  // Redireciona para o painel da sala
            }
            return RedirectToAction("Index");  // Redireciona para a p�gina inicial se n�o houver nome
        }

        // A��o para exibir o painel de estimativas de uma sala
        public IActionResult Painel(string id)
        {
            var sala = salas.FirstOrDefault(s => s.Id == id);
            if (sala == null)
            {
                return NotFound("Sala n�o encontrada.");
            }

            ViewBag.Sala = sala;  // Passa a sala para a view
            return View();
        }

        // A��o para adicionar um novo usu�rio � sala
        [HttpPost]
        public IActionResult AdicionarUsuario(string id, string nome)
        {
            var sala = salas.FirstOrDefault(s => s.Id == id);
            if (sala != null && !string.IsNullOrEmpty(nome))
            {
                sala.Usuarios.Add(new Usuario { Nome = nome });  // Adiciona o usu�rio
            }
            return RedirectToAction("Painel", new { id });
        }

        // A��o para registrar a estimativa (ponto) de um usu�rio
        [HttpPost]
        public IActionResult RegistrarEstimativa(string id, string nome, int ponto)
        {
            var sala = salas.FirstOrDefault(s => s.Id == id);
            var usuario = sala?.Usuarios.FirstOrDefault(u => u.Nome == nome);
            if (usuario != null)
            {
                usuario.Ponto = ponto;  // Registra o ponto
            }
            return RedirectToAction("Painel", new { id });
        }

        // A��o para deletar todos os votos de uma sala (apenas o criador pode fazer isso)
        [HttpPost]
        public IActionResult DeletarVotos(string id, string nomeCriador)
        {
            var sala = salas.FirstOrDefault(s => s.Id == id);
            if (sala != null && sala.Criador == nomeCriador)
            {
                // Limpa os votos de todos os usu�rios
                foreach (var usuario in sala.Usuarios)
                {
                    usuario.Ponto = null;
                }
            }
            return RedirectToAction("Painel", new { id });
        }

        // A��o para exibir as estimativas feitas pelos usu�rios
        public IActionResult MostrarEstimativas(string id)
        {
            var sala = salas.FirstOrDefault(s => s.Id == id);
            if (sala == null)
            {
                return NotFound("Sala n�o encontrada.");
            }

            ViewBag.Sala = sala;
            return View("Estimativas", sala.Usuarios);  // Exibe as estimativas feitas pelos usu�rios
        }
    }
}
