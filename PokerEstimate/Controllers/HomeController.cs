using Microsoft.AspNetCore.Mvc;
using PokerEstimate.Models;
using System.Collections.Generic;
using System.Linq;

namespace PokerEstimate.Controllers
{
    public class HomeController : Controller
    {
        // Lista estática para armazenar as salas
        private static List<Sala> salas = new List<Sala>();

        // Página inicial: Criação de sala ou entrada na sala existente
        public IActionResult Index()
        {
            return View();
        }

        // Criação de uma nova sala pelo criador
        [HttpPost]
        public IActionResult CriarSala(string nomeCriador)
        {
            if (!string.IsNullOrEmpty(nomeCriador))
            {
                var sala = new Sala { Criador = nomeCriador };
                salas.Add(sala);
                return RedirectToAction("Painel", new { id = sala.Id, nome = nomeCriador });
            }
            return RedirectToAction("Index");
        }

        // Entrada na sala existente com ID e nome
        [HttpPost]
        public IActionResult EntrarSala(string id, string nome)
        {
            var sala = salas.FirstOrDefault(s => s.Id == id);
            if (sala != null && !string.IsNullOrEmpty(nome))
            {
                if (!sala.Usuarios.Any(u => u.Nome == nome))
                {
                    sala.Usuarios.Add(new Usuario { Nome = nome });
                }
                return RedirectToAction("Painel", new { id = sala.Id, nome });
            }
            return RedirectToAction("Index");
        }

        // Exibição do painel da sala
        public IActionResult Painel(string id, string nome)
        {
            var sala = salas.FirstOrDefault(s => s.Id == id);
            if (sala == null)
            {
                return NotFound("Sala não encontrada.");
            }

            ViewBag.Sala = sala;
            ViewBag.NomeUsuario = nome;
            ViewBag.EhCriador = sala.Criador == nome;  // Verifica se o usuário é o criador
            return View();
        }

        // Registro de estimativa por um participante
        [HttpPost]
        public IActionResult RegistrarEstimativa(string id, string nome, int ponto)
        {
            var sala = salas.FirstOrDefault(s => s.Id == id);
            var usuario = sala?.Usuarios.FirstOrDefault(u => u.Nome == nome);
            if (usuario != null)
            {
                usuario.Ponto = ponto;
            }
            return RedirectToAction("Painel", new { id, nome });
        }

        // Limpeza de votos (apenas pelo criador da sala)
        [HttpPost]
        public IActionResult DeletarVotos(string id, string nomeCriador)
        {
            var sala = salas.FirstOrDefault(s => s.Id == id);
            if (sala != null && sala.Criador == nomeCriador)
            {
                foreach (var usuario in sala.Usuarios)
                {
                    usuario.Ponto = null;
                }
            }
            return RedirectToAction("Painel", new { id, nome = nomeCriador });
        }
    }
}
