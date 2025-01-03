using System;
using System.Collections.Generic;

namespace PokerEstimate.Models
{
    public class Sala
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Criador { get; set; }
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
