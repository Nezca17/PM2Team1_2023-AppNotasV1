using FirebaseAdmin.Messaging;
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
        public DateTime FechaIngreso { get; set; }    
        public bool IsRecordatorio { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public byte[] audioFile { get; set; }
        public byte[] ImagenFile { get; set; }  
        public double longitud { get; set; }
        public double latitude { get; set; }

        public string RutaAudioFile { get; set; }
        public string RutaImagenFile { get; set; }

        public Uri RutaAudioFileUri { get; set; }
        public Uri RutaImagenFileUri { get; set; }

        public int IdNoti { get; set; }

    }
}
