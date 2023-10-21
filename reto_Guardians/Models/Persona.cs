using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace reto_Guardians.Models;

[Table("Persona")]
public partial class Persona
{
    [Key]
    [Column("id_persona")]
    public int IdPersona { get; set; }

    [Column("nombre")]
    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [Column("edad")]
    public int Edad { get; set; }

    [JsonIgnore]
    [InverseProperty("IdPersonaNavigation")]
    public virtual ICollection<Heroe> Heroes { get; set; } = new List<Heroe>();

    [JsonIgnore]
    [InverseProperty("IdPersona1Navigation")]
    public virtual ICollection<RelacionPersonal> RelacionPersonalIdPersona1Navigations { get; set; } = new List<RelacionPersonal>();

    [JsonIgnore]
    [InverseProperty("IdPersona2Navigation")]
    public virtual ICollection<RelacionPersonal> RelacionPersonalIdPersona2Navigations { get; set; } = new List<RelacionPersonal>();

    [JsonIgnore]
    [InverseProperty("IdPersonaNavigation")]
    public virtual ICollection<Villano> Villanos { get; set; } = new List<Villano>();
}
