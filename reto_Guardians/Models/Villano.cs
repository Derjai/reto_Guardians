using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace reto_Guardians.Models;

[Table("Villano")]
public partial class Villano
{
    [Key]
    [Column("id_villano")]
    public int IdVillano { get; set; }

    [Column("id_persona")]
    public int IdPersona { get; set; }

    [Column("alias")]
    [StringLength(50)]
    [Unicode(false)]
    public string Alias { get; set; } = null!;

    [Column("origen")]
    [StringLength(50)]
    [Unicode(false)]
    public string Origen { get; set; } = null!;

    [Column("poder")]
    [StringLength(50)]
    [Unicode(false)]
    public string Poder { get; set; } = null!;

    [Column("debilidad")]
    [StringLength(50)]
    [Unicode(false)]
    public string Debilidad { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("IdVillanoNavigation")]
    public virtual ICollection<Combate> Combates { get; set; } = new List<Combate>();

    [JsonIgnore]
    [ForeignKey("IdPersona")]
    [InverseProperty("Villanos")]
    public virtual Persona IdPersonaNavigation { get; set; } = null!;
}
