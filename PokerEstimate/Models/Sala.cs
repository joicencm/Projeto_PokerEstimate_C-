using System;
using System.Collections.Generic;

namespace PokerEstimate.Models
{
    public class Sala
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();  // ID único da sala
        public string Criador { get; set; }  // Nome do criador
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();  // Lista de usuários da sala
    }
}
