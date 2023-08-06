﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PM2Team1_2023_AppNotasV1.Models
{
    public class Nota
{
        

        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Detalles { get; set; }
        public DateTime FechaIngreso { get; set; }    
        public bool isRecordatorio { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public string audioFile { get; set; }
        public string imagenFile { get; set; }  
        public double longitud { get; set; }
        public double latitude { get; set; }

        
}
}
