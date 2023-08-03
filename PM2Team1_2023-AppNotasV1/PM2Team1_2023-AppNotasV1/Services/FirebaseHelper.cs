using Firebase.Database;
using PM2Team1_2023_AppNotasV1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Firebase.Database.Query;

namespace PM2Team1_2023_AppNotasV1.Services
{
    public class FirebaseHelper
    {
        FirebaseClient firebase;

        public FirebaseHelper() {

            firebase = new FirebaseClient("https://pm2team1-2023-default-rtdb.firebaseio.com/");
        }

        public async Task<List<Nota>> GetNotas()
        {

            return (await firebase
                .Child("Notas")
                .OnceAsync<Nota>()).Select(Item => new Nota
                {

                    Id = Item.Object.Id,
                    Titulo = Item.Object.Titulo,
                    Detalles = Item.Object.Detalles,
                    fechaIngreso = Item.Object.fechaIngreso,
                    isRecordatorio = Item.Object.isRecordatorio,
                    fecha = Item.Object.fecha,
                    hora = Item.Object.hora,
                    audioFile = Item.Object.audioFile,
                    imagenFile = Item.Object.imagenFile,
                    longitud = Item.Object.longitud,
                    latitude = Item.Object.latitude,

                }).ToList();

        }

        public async Task AddNota(Nota _Nota)
        {
            await firebase.Child("Notas").PostAsync(new Nota()
            {

                Id = Guid.NewGuid(),
                Titulo = _Nota.Titulo,
                Detalles = _Nota.Detalles,
                fechaIngreso = _Nota.fechaIngreso,
                isRecordatorio = _Nota.isRecordatorio,
                fecha = _Nota.fecha,
                hora = _Nota.hora,
                audioFile = _Nota.audioFile,
                imagenFile = _Nota.imagenFile,
                longitud = _Nota.longitud,
                latitude = _Nota.latitude,


            });

        }

        public async Task UpdateNota(Nota _Nota) {

            var toUpdateNota = (await firebase.Child("Notas")
                .OnceAsync<Nota>()).Where(a => a.Object.Id == _Nota.Id).FirstOrDefault();

            await firebase.Child("Notas")
                .Child(toUpdateNota.Key)
                .PutAsync(new Nota() { Id = _Nota.Id, Titulo = _Nota.Titulo, Detalles = _Nota.Detalles, fechaIngreso = _Nota.fechaIngreso, 
                    audioFile = _Nota.audioFile, isRecordatorio = _Nota.isRecordatorio, fecha = _Nota.fecha, hora = _Nota.hora,
                    imagenFile = _Nota.audioFile, longitud = _Nota.longitud, latitude = _Nota.latitude
                });


        }

        public async Task DeleteNota(Guid IdNota)
        {
            var toDeleteNota = (await firebase.Child("Notas").OnceAsync<Nota>()).Where(a => a.Object.Id == IdNota).FirstOrDefault();

            await firebase.Child("Notas").Child(toDeleteNota.Key).DeleteAsync();

        }


    }
}
