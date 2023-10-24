using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace reto_Guardians.Models;

[Table("Heroe")]
public partial class Heroe
{
    [Key]
    [Column("id_heroe")]
    public int IdHeroe { get; set; }

    [Column("id_persona")]
    public int IdPersona { get; set; }

    [Column("alias")]
    [StringLength(50)]
    [Unicode(false)]
    public string Alias { get; set; } = null!;

    [Column("poder")]
    [StringLength(50)]
    [Unicode(false)]
    public string Poder { get; set; } = null!;

    [Column("debilidad")]
    [StringLength(50)]
    [Unicode(false)]
    public string Debilidad { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("IdHeroeNavigation")]
    public virtual ICollection<Agendum> Agenda { get; set; } = new List<Agendum>();

    [JsonIgnore]
    [InverseProperty("IdHeroeNavigation")]
    public virtual ICollection<Combate> Combates { get; set; } = new List<Combate>();

    [JsonIgnore]
    [ForeignKey("IdPersona")]
    [InverseProperty("Heroes")]
    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("IdHeroeNavigation")]
    public virtual ICollection<Patrocinador> Patrocinadors { get; set; } = new List<Patrocinador>();
}
