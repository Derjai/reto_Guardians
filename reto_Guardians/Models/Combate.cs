using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace reto_Guardians.Models;

[Table("Combate")]
public partial class Combate
{
    [Key]
    [Column("id_combate")]
    public int IdCombate { get; set; }

    [Column("id_heroe")]
    public int IdHeroe { get; set; }

    [Column("id_villano")]
    public int IdVillano { get; set; }

    [Column("lugar")]
    [StringLength(50)]
    [Unicode(false)]
    public string Lugar { get; set; } = null!;

    [Column("fecha", TypeName = "date")]
    public DateTime Fecha { get; set; }

    [Column("resultado")]
    [StringLength(50)]
    [Unicode(false)]
    public string Resultado { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdHeroe")]
    [InverseProperty("Combates")]
    public virtual Heroe IdHeroeNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdVillano")]
    [InverseProperty("Combates")]
    public virtual Villano IdVillanoNavigation { get; set; } = null!;
}
