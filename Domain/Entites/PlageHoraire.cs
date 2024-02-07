﻿using System.ComponentModel.DataAnnotations.Schema;

namespace SPU.Domain.Entites
{
    public class PlageHoraire
    {
        public Guid Id { get; set; }

        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }

        public bool ConfirmationPresence { get; set; }
        public string? Commentaire { get; set; }

        public Guid HoraireId { get; set; }

        [ForeignKey(nameof(Id))] virtual
        public Horaire horaire { get; set; }
    }
}
