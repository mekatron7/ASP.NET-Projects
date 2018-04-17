using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CarDealership.Data
{
    public class EFRepo : ICarRepo
    {
        public void AddCar(Car carToAdd)
        {
            var context = new CarDealershipContext();
            var car = new Car();

            car.VIN_ = carToAdd.VIN_;
            car.Model = context.Models.Single(m => m.ModelId == carToAdd.ModelId);
            car.ModelId = carToAdd.ModelId;
            car.BodyStyle = context.BodyStyles.Single(bs => bs.BSId == carToAdd.BSId);
            car.BSId = carToAdd.BSId;
            car.CarDescription = carToAdd.CarDescription;
            car.CarPicture = carToAdd.CarPicture;
            car.CarType = carToAdd.CarType;
            car.CarYear = carToAdd.CarYear;
            car.ExteriorColor = context.ExteriorColors.Single(ex => ex.ExtColorId == carToAdd.ExtColorId);
            car.ExtColorId = carToAdd.ExtColorId;
            car.Featured = carToAdd.Featured;
            car.InteriorColor = context.InteriorColors.Single(i => i.IntColorId == carToAdd.IntColorId);
            car.IntColorId = carToAdd.IntColorId;
            car.Mileage = carToAdd.Mileage;
            car.MSRP = carToAdd.MSRP;
            car.Purchased = carToAdd.Purchased;
            car.SalePrice = carToAdd.SalePrice;
            car.Transmission = context.Transmissions.Single(t => t.TransmissionId == carToAdd.TransmissionId);
            car.TransmissionId = carToAdd.TransmissionId;

            context.Cars.Add(car);
            context.SaveChanges();
        }

        public void AddContact(Contact contact)
        {
            var context = new CarDealershipContext();
            context.Contacts.Add(contact);
            context.SaveChanges();
        }

        public void AddMake(Make make)
        {
            var context = new CarDealershipContext();
            context.Makes.Add(make);
            context.SaveChanges();
        }

        public void AddModel(Model model)
        {
            var context = new CarDealershipContext();
            model.MakeId = model.Make.MakeId;
            model.Make = context.Makes.Single(m => m.MakeId == model.Make.MakeId);
            context.Models.Add(model);
            context.SaveChanges();
        }

        public void AddPurchase(Purchase purchase)
        {
            var context = new CarDealershipContext();
            context.Purchases.Add(purchase);
            context.SaveChanges();
        }

        public void AddSpecial(Special special)
        {
            var context = new CarDealershipContext();
            context.Specials.Add(special);
            context.SaveChanges();
        }

        public void DeleteCar(string vin)
        {
            var context = new CarDealershipContext();
            var car = context.Cars.First(c => c.VIN_ == vin);
            context.Cars.Remove(car);
            context.SaveChanges();
        }

        public void DeleteSpecial(int id)
        {
            var context = new CarDealershipContext();
            var special = context.Specials.First(s => s.SpecialId == id);
            context.Specials.Remove(special);
            context.SaveChanges();
        }

        public void EditCar(Car carToEdit)
        {
            var context = new CarDealershipContext();
            var car = context.Cars.Single(c => c.VIN_ == carToEdit.VIN_);

            car.VIN_ = carToEdit.VIN_;
            car.Model = context.Models.Single(m => m.ModelId == carToEdit.ModelId);
            car.ModelId = carToEdit.ModelId;
            car.BodyStyle = context.BodyStyles.Single(bs => bs.BSId == carToEdit.BSId);
            car.BSId = carToEdit.BSId;
            car.CarDescription = carToEdit.CarDescription;
            car.CarPicture = carToEdit.CarPicture;
            car.CarType = carToEdit.CarType;
            car.CarYear = carToEdit.CarYear;
            car.ExteriorColor = context.ExteriorColors.Single(ex => ex.ExtColorId == carToEdit.ExtColorId);
            car.ExtColorId = carToEdit.ExtColorId;
            car.Featured = carToEdit.Featured;
            car.InteriorColor = context.InteriorColors.Single(i => i.IntColorId == carToEdit.IntColorId);
            car.IntColorId = carToEdit.IntColorId;
            car.Mileage = carToEdit.Mileage;
            car.MSRP = carToEdit.MSRP;
            car.Purchased = carToEdit.Purchased;
            car.SalePrice = carToEdit.SalePrice;
            car.Transmission = context.Transmissions.Single(t => t.TransmissionId == carToEdit.TransmissionId);
            car.TransmissionId = carToEdit.TransmissionId;

            context.Entry(car).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void EditSpecial(Special special)
        {
            var context = new CarDealershipContext();
            context.Entry(special).State = EntityState.Modified;
            context.SaveChanges();
        }

        public Car Get(string vin)
        {
            var context = new CarDealershipContext();
            return context.Cars.FirstOrDefault(c => c.VIN_ == vin);
        }

        public List<Car> GetAllCars()
        {
            var context = new CarDealershipContext();
            return context.Cars.ToList();
        }

        public List<Make> GetAllMakes()
        {
            var context = new CarDealershipContext();
            return context.Makes.OrderBy(m => m.MakeName).ToList();
        }

        public List<Model> GetAllModels(Make make)
        {
            var context = new CarDealershipContext();
            return context.Models.Where(m => m.MakeId == make.MakeId).ToList();
        }

        public List<Model> GetAllModels()
        {
            var context = new CarDealershipContext();
            return context.Models.ToList();
        }

        public List<Car> GetAllNewCars()
        {
            var context = new CarDealershipContext();
            return context.Cars.Where(c => c.CarType == "New").ToList();
        }

        public List<Special> GetAllSpecials()
        {
            var context = new CarDealershipContext();
            return context.Specials.ToList();
        }

        public List<Car> GetAllUsedCars()
        {
            var context = new CarDealershipContext();
            return context.Cars.Where(c => c.CarType == "Used").ToList();
        }

        public List<string> GetBodyStyles()
        {
            var context = new CarDealershipContext();
            return context.BodyStyles.Select(b => b.BSName).ToList();
        }

        public List<string> GetExtColors()
        {
            var context = new CarDealershipContext();
            return context.ExteriorColors.Select(c => c.ExtColorName).Distinct().ToList();
        }

        public List<string> GetIntColors()
        {
            var context = new CarDealershipContext();
            return context.InteriorColors.Select(c => c.IntColorName).Distinct().ToList();
        }

        public Special GetSpecialById(int id)
        {
            var context = new CarDealershipContext();
            var special = context.Specials.FirstOrDefault(s => s.SpecialId == id);
            return special;
        }

        public List<string> GetTransmissions()
        {
            var context = new CarDealershipContext();
            return context.Transmissions.Select(c => c.TransmissionType).ToList();
        }

        public BodyStyle GetBodyStyle(string bodyStyle)
        {
            var context = new CarDealershipContext();
            return context.BodyStyles.First(b => b.BSName == bodyStyle);
        }

        public ExteriorColor GetExtColor(string color, string type)
        {
            var context = new CarDealershipContext();
            return context.ExteriorColors.First(c => c.ExtColorName == color && c.ExtColorType == type);
        }

        public InteriorColor GetIntColor(string color, string type)
        {
            var context = new CarDealershipContext();
            return context.InteriorColors.First(c => c.IntColorName == color && c.IntColorType == type);
        }

        public Transmission GetTransmission(string trans)
        {
            var context = new CarDealershipContext();
            return context.Transmissions.First(t => t.TransmissionType == trans);
        }
    }
}