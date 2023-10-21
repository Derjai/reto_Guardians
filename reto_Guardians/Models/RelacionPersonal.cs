using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace reto_Guardians.Models;

[Table("Relacion_Personal")]
public partial class RelacionPersonal
{
    [Key]
    [Column("id_relacion")]
    public int IdRelacion { get; set; }

    [Column("id_persona1")]
    public int IdPersona1 { get; set; }

    [Column("id_persona2")]
    public int IdPersona2 { get; set; }

    [Column("tipo_relacion")]
    [StringLength(50)]
    [Unicode(false)]
    public string TipoRelacion { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdPersona1")]
    [InverseProperty("RelacionPersonalIdPersona1Navigations")]
    public virtual Persona IdPersona1Navigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdPersona2")]
    [InverseProperty("RelacionPersonalIdPersona2Navigations")]
    public virtual Persona IdPersona2Navigation { get; set; } = null!;
}
