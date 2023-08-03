using System;
using System.Collections.Generic;
using System.Text;

namespace PM2Team1_2023_AppNotasV1.Models
{
    public class Nota
{
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Detalles { get; set; }
        public string fechaIngreso { get; set; }    
        public int isRecordatorio { get; set; }
        public DateTime fecha { get; set; }
        public DateTime hora { get; set; }
        public string audioFile { get; set; }
        public string imagenFile { get; set; }  
        public double longitur { get; set; }
        public double latitude { get; set; }

        
}
}
