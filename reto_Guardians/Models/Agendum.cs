using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace reto_Guardians.Models;

public partial class Agendum
{
    [Key]
    [Column("id_evento")]
    public int IdEvento { get; set; }

    [Column("id_heroe")]
    public int IdHeroe { get; set; }

    [Column("evento")]
    [StringLength(50)]
    [Unicode(false)]
    public string Evento { get; set; } = null!;

    [Column("fecha", TypeName = "date")]
    public DateTime Fecha { get; set; }

    [Column("descripcion")]
    [StringLength(50)]
    [Unicode(false)]
    public string Descripcion { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdHeroe")]
    [InverseProperty("Agenda")]
    public virtual Heroe IdHeroeNavigation { get; set; } = null!;
}
