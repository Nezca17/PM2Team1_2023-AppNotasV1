﻿using Firebase.Database;
using PM2Team1_2023_AppNotasV1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using Firebase.Database.Query;
using PM2Team1_2023_AppNotasV1.Views;

namespace PM2Team1_2023_AppNotasV1.Services
{
    public class FirebaseHelper
    {
        FirebaseClient firebase;

        FechaConverter fechaConver;

        public FirebaseHelper() {

            firebase = new FirebaseClient("https://pm2team1-2023-default-rtdb.firebaseio.com/");
            fechaConver = new FechaConverter();
        }

        public async Task<List<Nota>> GetNotas()
        {
           // Uri conversor;
            return (await firebase
                .Child("Notas")
                .OnceAsync<Nota>()).Select(Item => new Nota
                {

                    Id = Item.Object.Id,
                    Titulo = Item.Object.Titulo,
                    Detalles = Item.Object.Detalles,
                    FechaIngreso = Item.Object.FechaIngreso,
                    IsRecordatorio = Item.Object.IsRecordatorio,
                    Fecha = Item.Object.Fecha,
                    Hora = Item.Object.Hora,
                    RutaImagenFile = Item.Object.RutaImagenFile,
                    RutaAudioFile = Item.Object.RutaAudioFile,
                    longitud = Item.Object.longitud,
                    latitude = Item.Object.latitude,
                    RutaAudioFileUri = new Uri(Item.Object.RutaAudioFile),
                    RutaImagenFileUri =  new Uri(Item.Object.RutaImagenFile),
                    IdNoti = Item.Object.IdNoti
                }).ToList();

        }

        public async Task AddNota(Nota _Nota)
        {
            await firebase.Child("Notas").PostAsync(new Nota()
            {

                Id = Guid.NewGuid(),
                Titulo = _Nota.Titulo,
                Detalles = _Nota.Detalles,
                FechaIngreso = _Nota.FechaIngreso,
                IsRecordatorio = _Nota.IsRecordatorio,
                Fecha = _Nota.Fecha,
                Hora = _Nota.Hora,
                RutaImagenFile = _Nota.RutaImagenFile,
                RutaAudioFile = _Nota.RutaAudioFile,
                longitud = _Nota.longitud,
                latitude = _Nota.latitude,
                IdNoti = _Nota.IdNoti

            });

        }

        public async Task UpdateNota(Nota _Nota) {

            var toUpdateNota = (await firebase.Child("Notas")
                .OnceAsync<Nota>()).Where(a => a.Object.Id == _Nota.Id).FirstOrDefault();

            await firebase.Child("Notas")
                .Child(toUpdateNota.Key)
                .PutAsync(new Nota() { Id = _Nota.Id, Titulo = _Nota.Titulo, Detalles = _Nota.Detalles, FechaIngreso = _Nota.FechaIngreso, 
                    RutaImagenFile = _Nota.RutaImagenFile, IsRecordatorio = _Nota.IsRecordatorio, Fecha = _Nota.Fecha, Hora= _Nota.Hora,
                    RutaAudioFile = _Nota.RutaAudioFile, longitud = _Nota.longitud, latitude = _Nota.latitude
                });


        }

        public async Task DeleteNota(Guid IdNota)
        {
            var toDeleteNota = (await firebase.Child("Notas").OnceAsync<Nota>()).Where(a => a.Object.Id == IdNota).FirstOrDefault();

            await firebase.Child("Notas").Child(toDeleteNota.Key).DeleteAsync();

        }


    }
}
