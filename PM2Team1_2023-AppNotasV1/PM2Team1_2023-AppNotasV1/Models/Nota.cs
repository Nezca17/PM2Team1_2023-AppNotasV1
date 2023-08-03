using System;
using System.Collections.Generic;
using System.Text;

namespace PM2Team1_2023_AppNotasV1.Models
{
    public class Nota
{
        

        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Detalles { get; set; }
        public DateTime fechaIngreso { get; set; }    
        public bool isRecordatorio { get; set; }
        public DateTime fecha { get; set; }
        public TimeSpan hora { get; set; }
        public string audioFile { get; set; }
        public string imagenFile { get; set; }  
        public double longitud { get; set; }
        public double latitude { get; set; }

        
}
}
