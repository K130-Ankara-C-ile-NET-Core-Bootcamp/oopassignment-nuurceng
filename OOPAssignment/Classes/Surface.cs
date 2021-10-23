﻿using OOPAssignment.Interfaces;
using OOPAssignment.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPAssignment.Classes
{
    public class Surface : ISurface, ICollidableSurface, Interfaces.IObserver<CarInfo>
    {
        public long Width { get; set; }

        public long Height { get; set; }

        private List<CarInfo> ObservableCars = new List<CarInfo>();

        public Surface(long width, long height)
        {
            Width = width;
            Height = height;
        }

        public List<CarInfo> GetObservables()
        {
            List<CarInfo> _infos = new List<CarInfo>();
            _infos.AddRange(ObservableCars);
            return _infos;
        }

        public bool IsCoordinatesEmpty(Coordinates coordinates)
        {
            // Listede aynı konuma ait araç var mı
            var _car = ObservableCars
                .FirstOrDefault(x => x.Coordinates.X == coordinates.X && x.Coordinates.Y == coordinates.Y);
            
            if (_car is not null)
                return false;
            else
                return true;
        }

        public bool IsCoordinatesInBounds(Coordinates coordinates)
        {
            // Araç sınırlar içinde mi
            bool status = false;
            if ((coordinates.X >= 0 && coordinates.X <= Width) && (coordinates.Y >= 0 && coordinates.Y <= Height))
                status = true;

            return status;
        }

        public void Update(CarInfo provider)
        {
            var _car = ObservableCars.FirstOrDefault(x => x.CarId == provider.CarId);

            Coordinates _coordinates = provider.Coordinates;
            // sınır kontrolü
            if (IsCoordinatesInBounds(_coordinates) == false)
                throw new Exception("Araç sýnýrlar dýþýnda.");
            // konum kontrolü
            if (IsCoordinatesEmpty(_coordinates) == false)
            {
                var car = ObservableCars
                    .FirstOrDefault(x => x.CarId != provider.CarId
                    && x.Coordinates.X == provider.Coordinates.X
                    && x.Coordinates.Y == provider.Coordinates.Y);

                if (car is not null)
                    throw new Exception("Konumda idsi farklý araç bulunuyor.");
            }
            // Arac var ise güncelleme yapılsın
            else if (_car is not null)
            {
                _car = provider;
            }
            // Arac yok ise yeni arac eklensin
            else
                ObservableCars.Add(provider);
        }
    }
}
