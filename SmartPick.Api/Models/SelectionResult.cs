using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartPick.Api.Interfaces;

namespace SmartPick.Api.Models
{
    public struct SelectionResult : ISelectionResult
    {
        public string Selections { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
