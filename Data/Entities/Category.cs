﻿using System.Text.Json.Serialization;

namespace ASP_P22.Data.Entities
{
    public record Category
    {
        public Guid    Id          { get; set; }
        public String  Name        { get; set; } = String.Empty;
        public String  Description { get; set; } = String.Empty;
        public String? ImagesCsv   { get; set; }
        public String  Slug        { get; set; } = null!;

        // [JsonIgnore]
        public List<Product> Products { get; set; } = [];
    }
}
